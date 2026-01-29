---
name: "Load Testing"
description: "Design and execute load tests to validate performance under stress, identify scalability limits, and ensure SLA compliance"
category: "performance"
required_tools: ["Bash", "Write", "WebSearch"]
---

# Load Testing

## Purpose
Validate application performance under realistic and peak load conditions, identify scalability bottlenecks, and ensure systems meet performance SLAs.

## When to Use
- Before production deployment
- After performance optimizations
- Capacity planning
- Validating scalability
- Testing under expected peak loads

## Key Capabilities

1. **Load Test Design** - Create realistic test scenarios and user patterns
2. **Performance Validation** - Measure response times, throughput, and resource usage
3. **Scalability Analysis** - Identify breaking points and bottlenecks

## Approach

1. **Define Performance Requirements**
   - Target response times (p50, p95, p99)
   - Expected throughput (requests/second)
   - Concurrent users
   - Resource limits (CPU, memory)

2. **Design Test Scenarios**
   - User workflows (login → browse → checkout)
   - Traffic patterns (gradual ramp, spike, sustained)
   - Data variations (small/large payloads)
   - Think time between requests

3. **Select Tools**
   - **k6**: Modern, JavaScript-based, great reporting
   - **JMeter**: Feature-rich, GUI-based
   - **Locust**: Python-based, distributed testing
   - **Gatling**: Scala-based, detailed reports
   - **ab/wrk**: Simple command-line tools

4. **Execute Tests**
   - Start with baseline (single user)
   - Ramp up gradually
   - Test at target load
   - Spike test (sudden traffic increase)
   - Endurance test (sustained load)

5. **Analyze Results**
   - Response time percentiles
   - Error rates
   - Throughput degradation
   - Resource utilization
   - Database connection pool exhaustion

## Example

**Context**: Load testing an e-commerce API

**Test Scenario (k6)**:
```javascript
// load-test.js
import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate } from 'k6/metrics';

const errorRate = new Rate('errors');

export const options = {
  stages: [
    { duration: '2m', target: 100 },  // Ramp to 100 users
    { duration: '5m', target: 100 },  // Stay at 100
    { duration: '2m', target: 200 },  // Ramp to 200
    { duration: '5m', target: 200 },  // Stay at 200
    { duration: '2m', target: 0 },    // Ramp down
  ],
  thresholds: {
    http_req_duration: ['p(95)<500'], // 95% under 500ms
    http_req_failed: ['rate<0.01'],    // <1% errors
  },
};

export default function () {
  // Browse products
  let res = http.get('https://api.example.com/products');
  check(res, {
    'products loaded': (r) => r.status === 200,
    'response < 500ms': (r) => r.timings.duration < 500,
  }) || errorRate.add(1);
  
  sleep(1); // Think time
  
  // View product detail
  res = http.get('https://api.example.com/products/123');
  check(res, {
    'product detail loaded': (r) => r.status === 200,
  }) || errorRate.add(1);
  
  sleep(2);
  
  // Add to cart
  res = http.post('https://api.example.com/cart', JSON.stringify({
    product_id: 123,
    quantity: 1
  }), {
    headers: { 'Content-Type': 'application/json' },
  });
  check(res, {
    'added to cart': (r) => r.status === 201,
  }) || errorRate.add(1);
  
  sleep(1);
}
```

**Run Test**:
```bash
k6 run --out json=results.json load-test.js
```

**Results Analysis**:
```
scenarios: (100.00%) 1 scenario, 200 max VUs, 18m0s max duration

     ✓ products loaded
     ✓ response < 500ms
     ✓ product detail loaded
     ✓ added to cart

     checks.........................: 98.50% ✓ 19700  ✗ 300
     data_received..................: 45 MB  2.8 MB/s
     data_sent......................: 8.5 MB 530 kB/s
     http_req_blocked...............: avg=1.2ms    min=0s     med=0s      max=150ms   p(95)=5ms    p(99)=25ms
     http_req_duration..............: avg=245ms    min=45ms   med=180ms   max=2.5s    p(95)=480ms  p(99)=850ms
     http_req_failed................: 1.50%  ✓ 300   ✗ 19700
     http_reqs......................: 20000  1250/s
     iteration_duration.............: avg=4.8s     min=4s     med=4.5s    max=8s
     iterations.....................: 5000   312.5/s
     vus............................: 200    min=0    max=200
     vus_max........................: 200    min=200  max=200
```

**Analysis**:
- ✅ p95 response time: 480ms (meets <500ms threshold)
- ⚠️ p99 response time: 850ms (exceeds threshold)
- ⚠️ Error rate: 1.5% (exceeds <1% threshold)
- Throughput: 1250 requests/second
- System starts degrading above 200 concurrent users

**Bottleneck Investigation**:
- Errors occur during "add to cart" operation
- Database connection pool exhausted at peak load
- CPU usage spikes to 95% at 200 users

**Recommendations**:
1. Increase database connection pool size
2. Add caching for product catalog
3. Optimize cart operations
4. Consider horizontal scaling

## Best Practices

- ✅ Test in production-like environment
- ✅ Use realistic user scenarios, not just endpoint hammering
- ✅ Ramp up gradually (don't spike immediately)
- ✅ Monitor system metrics during tests (CPU, memory, DB connections)
- ✅ Test at 2-3x expected peak load
- ✅ Include think time between requests
- ✅ Vary request payloads and parameters
- ✅ Run endurance tests (24+ hours for stability)
- ❌ Avoid: Testing from same network as application
- ❌ Avoid: Ignoring failed requests in results
- ❌ Avoid: Testing only happy paths
- ❌ Avoid: Testing with empty databases