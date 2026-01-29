---
name: "Logging Strategies"
description: "Implement structured logging with appropriate levels, context, and correlation for effective debugging and monitoring"
category: "observability"
required_tools: ["Read", "Write", "Edit"]
---

## Purpose
Create comprehensive logging that enables debugging, monitoring, and analysis of application behavior in production.

## When to Use
- Implementing new features
- Debugging production issues
- Performance monitoring
- Security auditing

## Key Capabilities
1. **Structured Logging** - JSON logs with context
2. **Log Levels** - Appropriate severity classification
3. **Correlation** - Trace requests across services

## Example
```python
import logging
import json
from datetime import datetime
import uuid

class StructuredLogger:
    def __init__(self, service_name):
        self.service = service_name
        self.logger = logging.getLogger(service_name)
    
    def log(self, level, message, **context):
        log_entry = {
            'timestamp': datetime.utcnow().isoformat(),
            'service': self.service,
            'level': level,
            'message': message,
            **context
        }
        
        log_line = json.dumps(log_entry)
        
        if level == 'ERROR':
            self.logger.error(log_line)
        elif level == 'WARNING':
            self.logger.warning(log_line)
        elif level == 'INFO':
            self.logger.info(log_line)
        else:
            self.logger.debug(log_line)
    
    def with_context(self, **context):
        class ContextLogger:
            def __init__(self, parent, ctx):
                self.parent = parent
                self.context = ctx
            
            def info(self, message, **extra):
                self.parent.log('INFO', message, **{**self.context, **extra})
            
            def error(self, message, **extra):
                self.parent.log('ERROR', message, **{**self.context, **extra})
        
        return ContextLogger(self, context)

# Usage
logger = StructuredLogger('api-service')

def handle_request(request_id, user_id):
    request_logger = logger.with_context(
        request_id=request_id,
        user_id=user_id
    )
    
    request_logger.info("Processing request")
    
    try:
        # Business logic
        result = process_order()
        request_logger.info("Order processed", order_id=result.id)
    except Exception as e:
        request_logger.error("Order failed", error=str(e), exc_info=True)
```

## Best Practices
- ✅ Use structured logging (JSON)
- ✅ Include correlation IDs
- ✅ Log at appropriate levels
- ✅ Include context (user_id, request_id)
- ✅ Log errors with stack traces
- ❌ Avoid: Logging sensitive data (passwords, tokens)
- ❌ Avoid: Excessive debug logging in production