---
name: "Rate Limiting Strategies"
description: "Implement exponential backoff, token bucket algorithms, and API quota management"
category: "integration"
required_tools: ["Read", "Write", "Edit"]
---

# Rate Limiting Strategies

## Purpose
Implement rate limiting to respect API quotas, prevent abuse, and ensure system stability through various limiting algorithms.

## When to Use
- Integrating with rate-limited APIs
- Protecting your own APIs from abuse
- Managing concurrent requests
- Implementing fair usage policies
- Preventing resource exhaustion

## Key Capabilities

1. **Client-Side Rate Limiting** - Respect external API quotas
2. **Server-Side Rate Limiting** - Protect your APIs
3. **Backoff Strategies** - Handle rate limit errors gracefully

## Approach

1. **Choose Rate Limiting Algorithm**
   - **Token Bucket**: Smooth rate, allows bursts
   - **Leaky Bucket**: Fixed rate, no bursts
   - **Fixed Window**: Simple but has edge case issues
   - **Sliding Window**: Accurate but more complex

2. **Implement Client-Side Limiting**
   - Track requests per time window
   - Wait before exceeding limit
   - Honor Retry-After headers
   - Queue requests if needed

3. **Implement Server-Side Limiting**
   - Limit by IP, user, API key
   - Return 429 with Retry-After
   - Include rate limit headers
   - Use Redis for distributed systems

4. **Handle Rate Limit Errors**
   - Exponential backoff on 429
   - Use Retry-After header value
   - Maximum retry attempts
   - Circuit breaker for persistent failures

5. **Monitor and Alert**
   - Track rate limit hits
   - Alert before hitting limits
   - Monitor quota usage
   - Adjust limits based on usage

## Example

**Context**: Token bucket rate limiter for API client

```python
import time
import threading
from functools import wraps
from typing import Optional
import redis

class TokenBucket:
    """
    Token bucket rate limiter
    Allows burst traffic up to capacity, then enforces rate
    """
    def __init__(self, rate: float, capacity: int):
        """
        Args:
            rate: Tokens added per second
            capacity: Maximum tokens in bucket
        """
        self.rate = rate  # tokens per second
        self.capacity = capacity
        self.tokens = capacity
        self.last_update = time.time()
        self.lock = threading.Lock()
    
    def consume(self, tokens: int = 1) -> bool:
        """
        Try to consume tokens
        Returns True if tokens available, False otherwise
        """
        with self.lock:
            now = time.time()
            
            # Add tokens based on time passed
            elapsed = now - self.last_update
            self.tokens = min(
                self.capacity,
                self.tokens + elapsed * self.rate
            )
            self.last_update = now
            
            # Try to consume
            if self.tokens >= tokens:
                self.tokens -= tokens
                return True
            
            return False
    
    def wait_for_tokens(self, tokens: int = 1, timeout: Optional[float] = None):
        """
        Wait until tokens are available
        """
        start = time.time()
        
        while True:
            if self.consume(tokens):
                return True
            
            if timeout and (time.time() - start) >= timeout:
                return False
            
            time.sleep(0.1)

def rate_limit(calls_per_second: float, burst_capacity: int = None):
    """
    Decorator for rate limiting function calls
    """
    if burst_capacity is None:
        burst_capacity = int(calls_per_second * 2)
    
    bucket = TokenBucket(calls_per_second, burst_capacity)
    
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            bucket.wait_for_tokens()
            return func(*args, **kwargs)
        return wrapper
    return decorator

# Usage example
@rate_limit(calls_per_second=10)  # 10 calls/second
def call_api(endpoint: str):
    """API call automatically rate limited"""
    response = requests.get(f"https://api.example.com/{endpoint}")
    return response.json()
```

**Exponential Backoff with Rate Limiting**:
```python
import random
import time
import requests
from typing import Optional

class RateLimitedAPIClient:
    """
    API client with rate limiting and exponential backoff
    """
    def __init__(self, base_url: str, calls_per_second: float = 10):
        self.base_url = base_url
        self.bucket = TokenBucket(calls_per_second, calls_per_second * 2)
        self.session = requests.Session()
    
    def make_request(
        self,
        method: str,
        endpoint: str,
        max_retries: int = 5,
        **kwargs
    ):
        """
        Make API request with rate limiting and exponential backoff
        """
        for attempt in range(max_retries):
            # Wait for rate limit token
            self.bucket.wait_for_tokens()
            
            try:
                response = self.session.request(
                    method,
                    f"{self.base_url}/{endpoint}",
                    timeout=30,
                    **kwargs
                )
                
                # Handle rate limiting
                if response.status_code == 429:
                    # Use Retry-After header if provided
                    retry_after = response.headers.get('Retry-After')
                    if retry_after:
                        wait_time = int(retry_after)
                    else:
                        # Exponential backoff with jitter
                        wait_time = self._calculate_backoff(attempt)
                    
                    print(f"Rate limited. Waiting {wait_time}s (attempt {attempt + 1}/{max_retries})")
                    time.sleep(wait_time)
                    continue
                
                # Success or client error (don't retry 4xx)
                response.raise_for_status()
                return response
            
            except requests.exceptions.RequestException as e:
                if attempt == max_retries - 1:
                    raise
                
                wait_time = self._calculate_backoff(attempt)
                print(f"Request failed: {e}. Retrying in {wait_time}s")
                time.sleep(wait_time)
        
        raise Exception(f"Max retries ({max_retries}) exceeded")
    
    def _calculate_backoff(self, attempt: int) -> float:
        """
        Exponential backoff with jitter
        """
        # 2^attempt with random jitter
        base_delay = 2 ** attempt
        jitter = random.uniform(0, 0.1 * base_delay)
        return min(base_delay + jitter, 300)  # Max 5 minutes
```

**Server-Side Rate Limiting (Flask)**:
```python
from flask import Flask, request, jsonify
from flask_limiter import Limiter
from flask_limiter.util import get_remote_address
import redis

app = Flask(__name__)

# Redis-backed rate limiter (for distributed systems)
redis_client = redis.Redis(host='localhost', port=6379, db=0)

limiter = Limiter(
    app=app,
    key_func=get_remote_address,
    storage_uri="redis://localhost:6379",
    default_limits=["200 per day", "50 per hour"]
)

@app.route('/api/data')
@limiter.limit("10 per minute")  # Specific limit for this endpoint
def get_data():
    """API endpoint with rate limiting"""
    return jsonify({'data': 'value'})

@app.route('/api/expensive')
@limiter.limit("5 per minute")
def expensive_operation():
    """More restrictive limit for expensive operations"""
    # Expensive computation
    return jsonify({'result': 'computed'})

# Custom rate limit by API key
def get_api_key():
    return request.headers.get('X-API-Key', 'anonymous')

@app.route('/api/premium')
@limiter.limit("100 per minute", key_func=get_api_key)
def premium_endpoint():
    """Rate limit by API key, not IP"""
    return jsonify({'premium': 'data'})

# Dynamic rate limits based on user tier
def get_user_tier_limit():
    api_key = request.headers.get('X-API-Key')
    user = get_user_from_api_key(api_key)
    
    limits = {
        'free': '10 per minute',
        'basic': '100 per minute',
        'premium': '1000 per minute'
    }
    
    return limits.get(user.tier, '10 per minute')

@app.route('/api/tiered')
@limiter.limit(get_user_tier_limit)
def tiered_endpoint():
    """Dynamic rate limit based on user tier"""
    return jsonify({'data': 'tiered'})

# Custom rate limit exceeded handler
@app.errorhandler(429)
def ratelimit_handler(e):
    """Custom response for rate limit exceeded"""
    return jsonify({
        'error': 'Rate limit exceeded',
        'message': str(e.description),
        'retry_after': e.description.split()[-1] if hasattr(e, 'description') else 60
    }), 429
```

**Distributed Rate Limiting with Redis**:
```python
import redis
import time

class RedisRateLimiter:
    """
    Distributed rate limiter using Redis
    Sliding window log algorithm
    """
    def __init__(self, redis_client: redis.Redis):
        self.redis = redis_client
    
    def is_allowed(
        self,
        key: str,
        max_requests: int,
        window_seconds: int
    ) -> bool:
        """
        Check if request is allowed
        
        Args:
            key: Unique identifier (user_id, ip, api_key)
            max_requests: Maximum requests in window
            window_seconds: Time window in seconds
        """
        now = time.time()
        window_start = now - window_seconds
        
        # Redis pipeline for atomic operations
        pipe = self.redis.pipeline()
        
        # Remove old entries outside window
        pipe.zremrangebyscore(key, 0, window_start)
        
        # Count requests in current window
        pipe.zcard(key)
        
        # Add current request
        pipe.zadd(key, {now: now})
        
        # Set expiry on key
        pipe.expire(key, window_seconds)
        
        results = pipe.execute()
        
        # results[1] is the count before adding current request
        request_count = results[1]
        
        return request_count < max_requests
    
    def get_retry_after(
        self,
        key: str,
        window_seconds: int
    ) -> int:
        """
        Get seconds until rate limit resets
        """
        now = time.time()
        window_start = now - window_seconds
        
        # Get oldest request in window
        oldest = self.redis.zrange(key, 0, 0, withscores=True)
        
        if oldest:
            oldest_time = oldest[0][1]
            retry_after = int(window_start + window_seconds - now)
            return max(retry_after, 0)
        
        return 0

# Usage
redis_client = redis.Redis()
limiter = RedisRateLimiter(redis_client)

def api_endpoint(user_id: str):
    # Check rate limit: 100 requests per minute
    if not limiter.is_allowed(f"api:{user_id}", max_requests=100, window_seconds=60):
        retry_after = limiter.get_retry_after(f"api:{user_id}", window_seconds=60)
        return {
            'error': 'Rate limit exceeded',
            'retry_after': retry_after
        }, 429
    
    # Process request
    return {'data': 'success'}, 200
```

## Best Practices

- ✅ Use token bucket for APIs that allow bursts
- ✅ Use sliding window for accurate rate limiting
- ✅ Respect Retry-After headers from APIs
- ✅ Implement exponential backoff with jitter
- ✅ Use Redis for distributed rate limiting
- ✅ Include rate limit info in response headers
- ✅ Return 429 with Retry-After header
- ✅ Monitor rate limit hits and adjust as needed
- ✅ Implement different limits for different user tiers
- ✅ Track quota usage and alert before hitting limits
- ✅ Use circuit breakers for persistent failures
- ❌ Avoid: Constant retry intervals (causes thundering herd)
- ❌ Avoid: Ignoring rate limit response headers
- ❌ Avoid: No maximum retry limit (infinite loops)
- ❌ Avoid: Same limits for all users (implement tiers)