---
name: "Performance Profiling"
description: "Profile CPU, memory, and I/O usage to identify bottlenecks, analyze execution traces, and diagnose performance issues"
category: "performance"
required_tools: ["Bash", "Read", "Grep", "WebSearch"]
---

# Performance Profiling

## Purpose
Systematically measure and analyze application performance using profiling tools to identify bottlenecks, hot paths, memory leaks, and inefficient operations.

## When to Use
- Investigating slow operations or high latency
- Optimizing resource usage (CPU, memory, I/O)
- Diagnosing performance degradation
- Before and after performance improvements
- Capacity planning and scalability testing

## Key Capabilities

1. **CPU Profiling** - Identify time-consuming functions and hot paths
2. **Memory Profiling** - Detect leaks, excessive allocation, and memory patterns
3. **I/O Analysis** - Find slow database queries, file operations, network calls

## Approach

1. **Establish Baseline**
   - Measure current performance metrics
   - Document expected vs actual performance
   - Identify performance requirements (SLAs)

2. **Select Profiling Tools**
   - **Python**: cProfile, memory_profiler, py-spy, line_profiler
   - **Node.js**: Node.js built-in profiler, clinic.js, 0x
   - **Java**: JProfiler, VisualVM, YourKit
   - **Go**: pprof, trace
   - **Database**: EXPLAIN, query logs, slow query log
   - **System**: perf, strace, iostat, vmstat

3. **Collect Profiling Data**
   - Run application under realistic load
   - Capture CPU profile (flamegraphs)
   - Capture memory snapshots
   - Record I/O operations
   - Monitor system metrics

4. **Analyze Results**
   - Identify functions taking most CPU time
   - Find memory allocation hotspots
   - Locate slow database queries (N+1 problems)
   - Detect blocking I/O operations
   - Review call graphs and flame graphs

5. **Prioritize Optimizations**
   - Focus on biggest bottlenecks first
   - Consider effort vs impact
   - Measure before and after improvements

## Example

**Context**: Profiling a slow Python web API endpoint

**Step 1: Baseline Measurement**
```bash
# Measure endpoint response time
curl -w "@curl-format.txt" -o /dev/null -s http://localhost:8000/api/users
# Result: Total time: 2.8 seconds (Target: <500ms)
```

**Step 2: CPU Profiling**
```python
# profile_endpoint.py
import cProfile
import pstats
from io import StringIO

def profile_request():
    profiler = cProfile.Profile()
    profiler.enable()
    
    # Execute the slow endpoint
    response = app.test_client().get('/api/users')
    
    profiler.disable()
    
    # Generate report
    s = StringIO()
    ps = pstats.Stats(profiler, stream=s).sort_stats('cumulative')
    ps.print_stats(20)  # Top 20 functions
    print(s.getvalue())

profile_request()
```

**CPU Profile Results**:
```
   ncalls  tottime  percall  cumtime  percall filename:lineno(function)
        1    0.002    0.002    2.756    2.756 views.py:45(get_users)
      500    1.200    0.002    2.450    0.005 database.py:89(get_user_details)
     5000    0.850    0.000    0.850    0.000 {method 'execute' of 'sqlite3.Cursor'}
      500    0.300    0.001    0.300    0.001 serializers.py:22(serialize_user)
        1    0.150    0.150    0.150    0.150 {method 'fetchall' of 'sqlite3.Cursor'}
```

**Analysis**:
- `get_user_details()` called 500 times → N+1 query problem
- Database queries taking 85% of total time
- Each query is fast (0.002s), but 500 of them = 2.45s total

**Step 3: Database Query Analysis**
```python
# Original code (N+1 problem)
def get_users():
    users = User.query.all()  # 1 query
    results = []
    for user in users:
        # N queries (one per user)
        user_details = UserDetail.query.filter_by(user_id=user.id).first()
        results.append({
            'user': user,
            'details': user_details
        })
    return results
```

**Step 4: Memory Profiling**
```python
from memory_profiler import profile

@profile
def get_users():
    users = User.query.all()
    results = []
    for user in users:
        user_details = UserDetail.query.filter_by(user_id=user.id).first()
        results.append({
            'user': user,
            'details': user_details
        })
    return results
```

**Memory Profile Results**:
```
Line #    Mem usage    Increment   Line Contents
================================================
    45     50.2 MiB     50.2 MiB   def get_users():
    46     75.5 MiB     25.3 MiB       users = User.query.all()
    47     75.5 MiB      0.0 MiB       results = []
    48    125.8 MiB     50.3 MiB       for user in users:
    49    125.8 MiB      0.0 MiB           user_details = UserDetail.query...
    50    125.8 MiB      0.0 MiB           results.append(...)
    51    125.8 MiB      0.0 MiB       return results
```

**Analysis**: Loading 500 users with details uses 75 MiB memory

**Step 5: Flame Graph Analysis**
```bash
# Generate flame graph (visual)
py-spy record -o profile.svg --duration 30 -- python app.py
```

**Flame Graph Shows**:
- 87% time in database queries
- 8% time in serialization
- 5% time in framework overhead

**Optimization Applied**:
```python
# Optimized code (single query with join)
def get_users():
    # Use eager loading to fetch users and details in one query
    users = User.query.options(
        joinedload(User.details)
    ).all()
    
    results = []
    for user in users:
        results.append({
            'user': user,
            'details': user.details  # Already loaded, no query
        })
    return results
```

**Step 6: Verify Improvement**
```bash
# Re-measure endpoint response time
curl -w "@curl-format.txt" -o /dev/null -s http://localhost:8000/api/users
# Result: Total time: 0.18 seconds (94% improvement!)
```

**Expected Result**:
- Identified N+1 query as primary bottleneck
- Reduced 500 queries to 1 query
- Improved response time from 2.8s to 0.18s
- Reduced memory usage by using lazy evaluation where possible

## Best Practices

- ✅ Profile in production-like environment with realistic data
- ✅ Focus on user-facing operations first
- ✅ Use flame graphs for visual understanding
- ✅ Profile both CPU and memory together
- ✅ Measure before and after every optimization
- ✅ Profile under load (not just single requests)
- ✅ Keep profiling data for comparison over time
- ✅ Look for low-hanging fruit (N+1 queries, missing indexes)
- ✅ Consider statistical profiling for production (low overhead)
- ❌ Avoid: Optimizing without measuring first
- ❌ Avoid: Micro-optimizations that don't impact overall performance
- ❌ Avoid: Profiling only in development (profile staging/production)
- ❌ Avoid: Ignoring the 80/20 rule (fix biggest bottlenecks first)