---
name: "Optimization Patterns"
description: "Apply caching strategies, database optimization, and algorithmic improvements to enhance application performance"
category: "performance"
required_tools: ["Read", "Edit", "Bash", "WebSearch"]
---

# Optimization Patterns

## Purpose
Apply proven optimization techniques including caching, query optimization, and algorithm selection to improve application performance systematically.

## When to Use
- After identifying bottlenecks through profiling
- Optimizing database-heavy operations
- Improving API response times
- Reducing resource consumption
- Scaling applications

## Key Capabilities

1. **Caching Strategies** - Implement effective caching at multiple levels
2. **Database Optimization** - Improve query performance and reduce database load
3. **Algorithm Optimization** - Choose efficient algorithms and data structures

## Approach

1. **Identify Optimization Opportunities**
   - Repeated expensive computations
   - Slow database queries
   - Inefficient algorithms
   - Unnecessary data transfers
   - Blocking operations

2. **Apply Caching**
   - **Application cache**: In-memory (Redis, Memcached)
   - **Database cache**: Query result caching
   - **CDN cache**: Static assets
   - **Browser cache**: HTTP caching headers
   - **Computed values**: Memoization

3. **Optimize Database Access**
   - Add indexes on filtered/joined columns
   - Use connection pooling
   - Implement pagination for large result sets
   - Batch operations (bulk insert/update)
   - Denormalize for read-heavy workloads
   - Use materialized views
   - Optimize N+1 queries with eager loading

4. **Algorithm Improvements**
   - Choose appropriate data structures (hash maps vs arrays)
   - Reduce time complexity (O(n²) → O(n log n))
   - Use lazy evaluation
   - Stream large datasets instead of loading all in memory
   - Parallelize independent operations

5. **Reduce Latency**
   - Minimize network round trips
   - Use async/non-blocking I/O
   - Compress data transfers
   - Connection pooling
   - Prefetch predictable data

## Example

**Context**: Optimizing a slow dashboard that shows user statistics

**Before Optimization**:
```python
def get_dashboard_data(user_id):
    # Multiple slow queries, no caching
    user = db.query("SELECT * FROM users WHERE id = ?", user_id)
    
    # O(n) query for each calculation
    total_orders = db.query(
        "SELECT COUNT(*) FROM orders WHERE user_id = ?", user_id
    )[0][0]
    
    revenue = db.query(
        "SELECT SUM(amount) FROM orders WHERE user_id = ?", user_id
    )[0][0]
    
    # Inefficient: loads all orders into memory
    recent_orders = db.query(
        "SELECT * FROM orders WHERE user_id = ? ORDER BY created_at DESC", 
        user_id
    )[:5]  # Only need 5, but fetches all
    
    return {
        'user': user,
        'stats': {
            'total_orders': total_orders,
            'total_revenue': revenue
        },
        'recent_orders': recent_orders
    }

# Performance: 450ms per request
```

**After Optimization**:
```python
import redis
from functools import lru_cache

# Redis for distributed caching
cache = redis.Redis(host='localhost', port=6379, decode_responses=True)

def get_dashboard_data(user_id):
    # Check cache first
    cache_key = f"dashboard:{user_id}"
    cached = cache.get(cache_key)
    if cached:
        return json.loads(cached)
    
    # Single optimized query with indexes
    result = db.query("""
        SELECT 
            u.*,
            COUNT(o.id) as total_orders,
            COALESCE(SUM(o.amount), 0) as total_revenue
        FROM users u
        LEFT JOIN orders o ON o.user_id = u.id
        WHERE u.id = ?
        GROUP BY u.id
    """, user_id)[0]
    
    # Efficient query: LIMIT at database level
    recent_orders = db.query("""
        SELECT id, amount, created_at
        FROM orders
        WHERE user_id = ?
        ORDER BY created_at DESC
        LIMIT 5
    """, user_id)
    
    data = {
        'user': dict(result),
        'stats': {
            'total_orders': result['total_orders'],
            'total_revenue': result['total_revenue']
        },
        'recent_orders': recent_orders
    }
    
    # Cache for 5 minutes
    cache.setex(cache_key, 300, json.dumps(data))
    
    return data

# Performance: 12ms (cache hit) or 45ms (cache miss)
# 90% reduction in query time, 97% with cache
```

**Database Indexes Added**:
```sql
-- Support the JOIN efficiently
CREATE INDEX idx_orders_user_id ON orders(user_id);

-- Support ORDER BY + LIMIT
CREATE INDEX idx_orders_user_created ON orders(user_id, created_at DESC);
```

**Optimization Techniques Applied**:
1. **Query reduction**: 4 queries → 2 queries
2. **Query combining**: Used JOIN to get stats in one query
3. **Caching**: Redis cache with 5-minute TTL
4. **Indexing**: Added database indexes on filtered columns
5. **Limit pushdown**: LIMIT 5 at database, not application
6. **Column selection**: Only fetch needed columns

## Best Practices

- ✅ Cache expensive computations and database queries
- ✅ Use appropriate cache invalidation strategies (TTL, event-based)
- ✅ Add database indexes on WHERE, JOIN, ORDER BY columns
- ✅ Paginate large result sets
- ✅ Use connection pooling for databases
- ✅ Batch database operations when possible
- ✅ Choose O(log n) or O(1) algorithms over O(n²)
- ✅ Measure before and after optimizations
- ✅ Use lazy evaluation for large datasets
- ✅ Compress data in transit when appropriate
- ❌ Avoid: Premature optimization
- ❌ Avoid: Over-caching (stale data issues)
- ❌ Avoid: Missing cache invalidation
- ❌ Avoid: Loading entire tables into memory