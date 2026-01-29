---
name: "Webhook Design"
description: "Design secure webhooks with HMAC validation, idempotency patterns, and event-driven architecture"
category: "integration"
required_tools: ["Read", "Write", "Edit"]
---

# Webhook Design

## Purpose
Design and implement secure webhook receivers that handle events from external systems reliably with proper security, idempotency, and error handling.

## When to Use
- Receiving events from third-party services (Stripe, GitHub, Slack)
- Building event-driven architectures
- Implementing real-time integrations
- Processing asynchronous notifications
- Handling payment/order updates

## Key Capabilities

1. **Security** - Validate webhook signatures (HMAC, JWT)
2. **Idempotency** - Handle duplicate events safely
3. **Reliability** - Retry failed processing, handle errors gracefully

## Approach

1. **Implement Signature Verification**
   - Validate HMAC signature from provider
   - Use timing-safe comparison
   - Reject unsigned or invalid requests
   - Log verification failures

2. **Design for Idempotency**
   - Store event IDs to detect duplicates
   - Use database transactions
   - Make operations idempotent
   - Return 200 for already-processed events

3. **Process Asynchronously**
   - Return 200 OK immediately
   - Queue event for background processing
   - Don't block webhook response
   - Timeout: < 5 seconds total

4. **Handle Errors Gracefully**
   - Log all errors with context
   - Return appropriate status codes
   - Provider will retry on 5xx errors
   - Don't retry on invalid data (4xx)

5. **Monitor and Alert**
   - Track webhook delivery success/failure
   - Alert on repeated failures
   - Monitor processing delays
   - Track event types received

## Example

**Context**: Stripe webhook receiver for payment events

```python
from flask import Flask, request, jsonify
import hmac
import hashlib
import os
import logging
from datetime import datetime
from redis import Redis
from rq import Queue

app = Flask(__name__)
logger = logging.getLogger(__name__)

# Configuration
WEBHOOK_SECRET = os.environ['STRIPE_WEBHOOK_SECRET']
redis_conn = Redis()
task_queue = Queue('webhooks', connection=redis_conn)

# Event ID storage (Redis for distributed systems)
processed_events = Redis(db=1)

def verify_stripe_signature(payload: bytes, signature: str) -> bool:
    """
    Verify Stripe webhook signature
    """
    try:
        # Extract timestamp and signatures
        elements = signature.split(',')
        timestamp = None
        signatures = []
        
        for element in elements:
            key, value = element.split('=')
            if key == 't':
                timestamp = value
            elif key.startswith('v'):
                signatures.append(value)
        
        if not timestamp or not signatures:
            logger.warning("Missing timestamp or signature")
            return False
        
        # Construct signed payload
        signed_payload = f"{timestamp}.{payload.decode('utf-8')}"
        
        # Compute expected signature
        expected_sig = hmac.new(
            WEBHOOK_SECRET.encode('utf-8'),
            signed_payload.encode('utf-8'),
            hashlib.sha256
        ).hexdigest()
        
        # Timing-safe comparison
        return any(hmac.compare_digest(expected_sig, sig) for sig in signatures)
        
    except Exception as e:
        logger.error(f"Signature verification error: {e}")
        return False

def is_event_processed(event_id: str) -> bool:
    """Check if event was already processed"""
    return processed_events.exists(event_id)

def mark_event_processed(event_id: str, ttl_hours: int = 72):
    """Mark event as processed with TTL"""
    processed_events.setex(
        event_id,
        ttl_hours * 3600,
        datetime.utcnow().isoformat()
    )

@app.route('/webhooks/stripe', methods=['POST'])
def stripe_webhook():
    """
    Stripe webhook endpoint
    """
    # 1. Get payload and signature
    payload = request.data
    signature = request.headers.get('Stripe-Signature')
    
    if not signature:
        logger.warning("Missing Stripe-Signature header")
        return jsonify({'error': 'Missing signature'}), 400
    
    # 2. Verify signature
    if not verify_stripe_signature(payload, signature):
        logger.warning("Invalid signature")
        return jsonify({'error': 'Invalid signature'}), 401
    
    # 3. Parse event
    try:
        event = request.json
    except Exception as e:
        logger.error(f"Invalid JSON: {e}")
        return jsonify({'error': 'Invalid JSON'}), 400
    
    event_id = event.get('id')
    event_type = event.get('type')
    
    if not event_id or not event_type:
        logger.error("Missing event id or type")
        return jsonify({'error': 'Invalid event'}), 400
    
    # 4. Check idempotency - already processed?
    if is_event_processed(event_id):
        logger.info(f"Event {event_id} already processed")
        return jsonify({'status': 'already_processed'}), 200
    
    # 5. Log event receipt
    logger.info(f"Received webhook: {event_type} ({event_id})")
    
    # 6. Queue for async processing (don't block webhook)
    try:
        task_queue.enqueue(
            process_stripe_event,
            event,
            job_timeout='5m',
            result_ttl=86400  # Keep result for 24h
        )
    except Exception as e:
        logger.error(f"Failed to queue event: {e}")
        # Still return 200 to avoid retries
        # Alert ops team for queue issues
        return jsonify({'status': 'queued_with_error'}), 200
    
    # 7. Return 200 immediately (< 5 seconds total)
    return jsonify({'status': 'received'}), 200

def process_stripe_event(event: dict):
    """
    Process Stripe webhook event (runs asynchronously)
    """
    event_id = event['id']
    event_type = event['type']
    
    try:
        logger.info(f"Processing event {event_id}: {event_type}")
        
        # Double-check idempotency in worker
        if is_event_processed(event_id):
            logger.info(f"Event {event_id} already processed (worker check)")
            return
        
        # Process based on event type
        if event_type == 'payment_intent.succeeded':
            handle_payment_success(event['data']['object'])
        
        elif event_type == 'payment_intent.payment_failed':
            handle_payment_failure(event['data']['object'])
        
        elif event_type == 'customer.subscription.created':
            handle_subscription_created(event['data']['object'])
        
        elif event_type == 'customer.subscription.deleted':
            handle_subscription_canceled(event['data']['object'])
        
        else:
            logger.info(f"Unhandled event type: {event_type}")
        
        # Mark as processed
        mark_event_processed(event_id)
        logger.info(f"Successfully processed event {event_id}")
        
    except Exception as e:
        logger.error(f"Error processing event {event_id}: {e}", exc_info=True)
        # Don't mark as processed - will be retried
        # Alert ops team
        raise

def handle_payment_success(payment_intent: dict):
    """Handle successful payment"""
    amount = payment_intent['amount']
    customer_id = payment_intent['customer']
    
    # Update database
    from models import db, Order
    
    order = Order.query.filter_by(
        stripe_payment_intent_id=payment_intent['id']
    ).first()
    
    if order:
        order.status = 'paid'
        order.paid_at = datetime.utcnow()
        db.session.commit()
        
        # Send confirmation email
        send_order_confirmation_email(order)
        
        logger.info(f"Order {order.id} marked as paid")
    else:
        logger.warning(f"No order found for payment intent {payment_intent['id']}")

def handle_payment_failure(payment_intent: dict):
    """Handle failed payment"""
    from models import db, Order
    
    order = Order.query.filter_by(
        stripe_payment_intent_id=payment_intent['id']
    ).first()
    
    if order:
        order.status = 'payment_failed'
        order.payment_failure_reason = payment_intent.get('last_payment_error', {}).get('message')
        db.session.commit()
        
        # Notify customer
        send_payment_failed_email(order)
        
        logger.info(f"Order {order.id} payment failed")

# Webhook testing endpoint (development only)
if os.environ.get('FLASK_ENV') == 'development':
    @app.route('/webhooks/test', methods=['POST'])
    def test_webhook():
        """Test webhook without signature verification"""
        event = request.json
        task_queue.enqueue(process_stripe_event, event)
        return jsonify({'status': 'queued'}), 200
```

**GitHub Webhook Example**:
```python
def verify_github_signature(payload: bytes, signature: str) -> bool:
    """Verify GitHub webhook signature"""
    if not signature:
        return False
    
    # GitHub sends: sha256=<hash>
    expected_sig = 'sha256=' + hmac.new(
        GITHUB_WEBHOOK_SECRET.encode('utf-8'),
        payload,
        hashlib.sha256
    ).hexdigest()
    
    return hmac.compare_digest(expected_sig, signature)

@app.route('/webhooks/github', methods=['POST'])
def github_webhook():
    """GitHub webhook endpoint"""
    payload = request.data
    signature = request.headers.get('X-Hub-Signature-256')
    event_type = request.headers.get('X-GitHub-Event')
    delivery_id = request.headers.get('X-GitHub-Delivery')
    
    # Verify signature
    if not verify_github_signature(payload, signature):
        return jsonify({'error': 'Invalid signature'}), 401
    
    # Check idempotency
    if is_event_processed(delivery_id):
        return jsonify({'status': 'already_processed'}), 200
    
    # Parse event
    event = request.json
    
    # Queue for processing
    task_queue.enqueue(process_github_event, event_type, event, delivery_id)
    
    return jsonify({'status': 'received'}), 200
```

**Monitoring and Alerting**:
```python
from prometheus_client import Counter, Histogram

webhook_received = Counter(
    'webhook_received_total',
    'Total webhooks received',
    ['provider', 'event_type']
)

webhook_processed = Counter(
    'webhook_processed_total',
    'Total webhooks processed',
    ['provider', 'event_type', 'status']
)

webhook_processing_time = Histogram(
    'webhook_processing_seconds',
    'Webhook processing time',
    ['provider', 'event_type']
)

def process_stripe_event(event: dict):
    event_type = event['type']
    
    with webhook_processing_time.labels('stripe', event_type).time():
        try:
            # ... processing logic ...
            webhook_processed.labels('stripe', event_type, 'success').inc()
        except Exception as e:
            webhook_processed.labels('stripe', event_type, 'failure').inc()
            raise
```

## Best Practices

- ✅ Always validate webhook signatures (HMAC, JWT)
- ✅ Return 200 OK quickly (< 5 seconds)
- ✅ Process events asynchronously in background queue
- ✅ Implement idempotency with event ID tracking
- ✅ Use timing-safe comparison for signatures
- ✅ Log all webhook events for debugging
- ✅ Monitor webhook processing success/failure
- ✅ Handle duplicate deliveries gracefully
- ✅ Use database transactions for consistency
- ✅ Set reasonable TTL on processed event IDs (72h)
- ✅ Provide webhook testing endpoints for development
- ❌ Avoid: Blocking operations in webhook handler
- ❌ Avoid: Processing without signature validation
- ❌ Avoid: Failing on duplicate events (return 200)
- ❌ Avoid: Long-running operations synchronously
- ❌ Avoid: Returning 4xx for provider errors (use 5xx)