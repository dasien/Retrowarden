---
name: "Metrics & Monitoring"
description: "Implement application metrics (RED, USE), alerting strategies, and monitoring dashboards"
category: "observability"
required_tools: ["Read", "Write", "Bash"]
---

# Metrics & Monitoring

## Purpose
Instrument applications with meaningful metrics, set up monitoring dashboards, and configure alerts to detect issues before users do.

## When to Use
- Deploying to production
- Performance monitoring
- Capacity planning
- Incident detection and response
- SLA/SLO tracking
- Understanding system behavior

## Key Capabilities

1. **Metric Collection** - Instrument code with RED, USE, Four Golden Signals
2. **Dashboard Creation** - Visualize system health and trends
3. **Alerting** - Detect anomalies and trigger notifications

## Approach

1. **Choose Metric Methodology**
   - **RED**: Rate, Errors, Duration (for services/requests)
   - **USE**: Utilization, Saturation, Errors (for resources)
   - **Four Golden Signals**: Latency, Traffic, Errors, Saturation

2. **Instrument Application**
   - Add counters for events (requests, errors)
   - Add gauges for current values (connections, memory)
   - Add histograms for distributions (latency)
   - Add summaries for quantiles (p95, p99)

3. **Set Up Collection**
   - Prometheus for metrics
   - StatsD for application metrics
   - CloudWatch for AWS
   - DataDog for full-stack

4. **Create Dashboards**
   - System overview (health at a glance)
   - Service-specific (RED metrics per endpoint)
   - Resource usage (USE metrics)
   - Business metrics (orders, revenue)

5. **Configure Alerts**
   - Error rate > threshold
   - Latency > SLO
   - Resource saturation > 80%
   - Service unavailable

## Example

**Context**: Monitoring a web API with Prometheus

```python
from prometheus_client import Counter, Histogram, Gauge, Summary
from flask import Flask, request
import time
import psutil

app = Flask(__name__)

# RED Metrics (Rate, Errors, Duration)

# Rate: Request count
request_count = Counter(
    'http_requests_total',
    'Total HTTP requests',
    ['method', 'endpoint', 'status']
)

# Errors: Error count
error_count = Counter(
    'http_errors_total',
    'Total HTTP errors',
    ['method', 'endpoint', 'error_type']
)

# Duration: Request latency
request_latency = Histogram(
    'http_request_duration_seconds',
    'HTTP request latency',
    ['method', 'endpoint'],
    buckets=[0.01, 0.05, 0.1, 0.5, 1.0, 2.0, 5.0, 10.0]
)

# Alternative: Summary with quantiles
request_latency_summary = Summary(
    'http_request_duration_summary',
    'HTTP request latency summary',
    ['method', 'endpoint']
)

# USE Metrics (Utilization, Saturation, Errors)

# Utilization: Current resource usage
cpu_usage = Gauge('cpu_usage_percent', 'CPU usage percentage')
memory_usage = Gauge('memory_usage_bytes', 'Memory usage in bytes')
disk_usage = Gauge('disk_usage_percent', 'Disk usage percentage')

# Saturation: Queue depths, connection pools
db_connection_pool_usage = Gauge(
    'db_connection_pool_usage',
    'Database connections in use'
)
db_connection_pool_max = Gauge(
    'db_connection_pool_max',
    'Maximum database connections'
)

# Application-specific metrics
active_users = Gauge('active_users', 'Currently active users')
cache_hits = Counter('cache_hits_total', 'Cache hits')
cache_misses = Counter('cache_misses_total', 'Cache misses')

# Business metrics
orders_total = Counter('orders_total', 'Total orders', ['status'])
revenue_total = Counter('revenue_total', 'Total revenue in cents')

# Middleware to track requests
@app.before_request
def before_request():
    request.start_time = time.time()

@app.after_request
def after_request(response):
    # Track request
    method = request.method
    endpoint = request.endpoint or 'unknown'
    status = response.status_code
    
    # Update metrics
    request_count.labels(method, endpoint, status).inc()
    
    # Track latency
    if hasattr(request, 'start_time'):
        duration = time.time() - request.start_time
        request_latency.labels(method, endpoint).observe(duration)
        request_latency_summary.labels(method, endpoint).observe(duration)
    
    return response

# Track errors
@app.errorhandler(Exception)
def handle_error(error):
    method = request.method
    endpoint = request.endpoint or 'unknown'
    error_type = type(error).__name__
    
    error_count.labels(method, endpoint, error_type).inc()
    request_count.labels(method, endpoint, 500).inc()
    
    return {'error': str(error)}, 500

# Expose metrics endpoint
from prometheus_client import generate_latest, CONTENT_TYPE_LATEST

@app.route('/metrics')
def metrics():
    return generate_latest(), 200, {'Content-Type': CONTENT_TYPE_LATEST}

# Background job to update resource metrics
import threading

def update_system_metrics():
    while True:
        # CPU usage
        cpu_percent = psutil.cpu_percent(interval=1)
        cpu_usage.set(cpu_percent)
        
        # Memory usage
        memory = psutil.virtual_memory()
        memory_usage.set(memory.used)
        
        # Disk usage
        disk = psutil.disk_usage('/')
        disk_usage.set(disk.percent)
        
        time.sleep(15)  # Update every 15 seconds

# Start background metrics updater
metrics_thread = threading.Thread(target=update_system_metrics, daemon=True)
metrics_thread.start()

# Example: Tracking business metrics
@app.route('/api/orders', methods=['POST'])
def create_order():
    try:
        order_data = request.json
        
        # Process order
        order = process_order(order_data)
        
        # Track metrics
        orders_total.labels(status='success').inc()
        revenue_total.inc(order.amount_cents)
        
        return {'order_id': order.id}, 201
        
    except Exception as e:
        orders_total.labels(status='failed').inc()
        raise
```

**Prometheus Configuration** (`prometheus.yml`):
```yaml
global:
  scrape_interval: 15s
  evaluation_interval: 15s

scrape_configs:
  - job_name: 'web-api'
    static_configs:
      - targets: ['localhost:5000']
    metrics_path: '/metrics'

# Alerting rules
rule_files:
  - 'alerts.yml'

alerting:
  alertmanagers:
    - static_configs:
        - targets: ['localhost:9093']
```

**Alert Rules** (`alerts.yml`):
```yaml
groups:
  - name: api_alerts
    interval: 30s
    rules:
      # High error rate
      - alert: HighErrorRate
        expr: |
          sum(rate(http_requests_total{status=~"5.."}[5m]))
          /
          sum(rate(http_requests_total[5m]))
          > 0.05
        for: 5m
        labels:
          severity: critical
        annotations:
          summary: "High error rate ({{ $value | humanizePercentage }})"
          description: "Error rate is above 5% for 5 minutes"
      
      # High latency
      - alert: HighLatency
        expr: |
          histogram_quantile(0.95,
            sum(rate(http_request_duration_seconds_bucket[5m])) by (le, endpoint)
          ) > 1.0
        for: 10m
        labels:
          severity: warning
        annotations:
          summary: "High latency on {{ $labels.endpoint }}"
          description: "P95 latency is {{ $value }}s (threshold: 1s)"
      
      # High CPU usage
      - alert: HighCPUUsage
        expr: cpu_usage_percent > 80
        for: 10m
        labels:
          severity: warning
        annotations:
          summary: "High CPU usage ({{ $value }}%)"
          description: "CPU usage above 80% for 10 minutes"
      
      # Database connection pool exhaustion
      - alert: DBConnectionPoolNearLimit
        expr: |
          db_connection_pool_usage / db_connection_pool_max > 0.9
        for: 5m
        labels:
          severity: critical
        annotations:
          summary: "Database connection pool near limit"
          description: "Using {{ $value | humanizePercentage }} of connection pool"
```

**Grafana Dashboard** (JSON):
```json
{
  "dashboard": {
    "title": "API Monitoring",
    "panels": [
      {
        "title": "Request Rate",
        "targets": [
          {
            "expr": "sum(rate(http_requests_total[5m])) by (endpoint)"
          }
        ],
        "type": "graph"
      },
      {
        "title": "Error Rate",
        "targets": [
          {
            "expr": "sum(rate(http_requests_total{status=~\"5..\"}[5m])) / sum(rate(http_requests_total[5m]))"
          }
        ],
        "type": "graph",
        "alert": {
          "conditions": [
            {
              "evaluator": {
                "params": [0.05],
                "type": "gt"
              }
            }
          ]
        }
      },
      {
        "title": "Request Latency (P95)",
        "targets": [
          {
            "expr": "histogram_quantile(0.95, sum(rate(http_request_duration_seconds_bucket[5m])) by (le, endpoint))"
          }
        ],
        "type": "graph"
      },
      {
        "title": "Active Connections",
        "targets": [
          {
            "expr": "db_connection_pool_usage"
          }
        ],
        "type": "gauge"
      }
    ]
  }
}
```

**Custom Decorator for Automatic Instrumentation**:
```python
from functools import wraps

def monitor(metric_name=None):
    """Decorator to automatically monitor function calls"""
    def decorator(func):
        name = metric_name or func.__name__
        
        # Create metrics for this function
        calls = Counter(f'{name}_calls_total', f'Total calls to {name}')
        errors = Counter(f'{name}_errors_total', f'Errors in {name}')
        duration = Histogram(f'{name}_duration_seconds', f'Duration of {name}')
        
        @wraps(func)
        def wrapper(*args, **kwargs):
            calls.inc()
            
            with duration.time():
                try:
                    result = func(*args, **kwargs)
                    return result
                except Exception as e:
                    errors.inc()
                    raise
        
        return wrapper
    return decorator

# Usage
@monitor('process_payment')
def process_payment(order_id):
    # Function automatically instrumented
    pass
```

## Best Practices

- ✅ Use RED metrics for request-driven services
- ✅ Use USE metrics for resource monitoring
- ✅ Monitor both technical and business metrics
- ✅ Set up alerts on symptoms, not causes
- ✅ Define SLOs and alert on SLO violations
- ✅ Use percentiles (p95, p99) not averages for latency
- ✅ Include cardinality limits (don't track unbounded labels)
- ✅ Create runbooks for each alert
- ✅ Test alerts (trigger them intentionally)
- ✅ Review and tune alerts regularly
- ❌ Avoid: Too many alerts (alert fatigue)
- ❌ Avoid: Alerts without actionable responses
- ❌ Avoid: High-cardinality labels (user IDs, timestamps)
- ❌ Avoid: Monitoring without SLOs