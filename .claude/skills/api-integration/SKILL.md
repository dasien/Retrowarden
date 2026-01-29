---
name: "API Integration Patterns"
description: "Implement robust third-party API integrations with proper authentication, error handling, and rate limiting"
category: "integration"
required_tools: ["Read", "Write", "Edit", "WebSearch"]
---

## Purpose
Build reliable integrations with external APIs, handling authentication flows, retries, rate limits, and error conditions gracefully.

## When to Use
- Integrating third-party services
- Building API clients
- Consuming webhooks
- Managing API credentials

## Key Capabilities
1. **Authentication Handling** - OAuth, API keys, JWT
2. **Error Recovery** - Retries with exponential backoff
3. **Rate Limit Management** - Respect API quotas

## Example
```python
import requests
from time import sleep
import logging

class APIClient:
    def __init__(self, base_url, api_key):
        self.base_url = base_url
        self.session = requests.Session()
        self.session.headers.update({
            'Authorization': f'Bearer {api_key}',
            'User-Agent': 'MyApp/1.0'
        })
    
    def make_request(self, method, endpoint, **kwargs):
        url = f"{self.base_url}/{endpoint}"
        max_retries = 3
        
        for attempt in range(max_retries):
            try:
                response = self.session.request(method, url, **kwargs)
                
                # Handle rate limiting
                if response.status_code == 429:
                    retry_after = int(response.headers.get('Retry-After', 60))
                    logging.warning(f"Rate limited. Waiting {retry_after}s")
                    sleep(retry_after)
                    continue
                
                response.raise_for_status()
                return response.json()
            
            except requests.exceptions.RequestException as e:
                if attempt == max_retries - 1:
                    raise
                # Exponential backoff
                wait = 2 ** attempt
                logging.warning(f"Request failed, retrying in {wait}s: {e}")
                sleep(wait)
        
        raise Exception("Max retries exceeded")
```

## Best Practices
- ✅ Implement exponential backoff for retries
- ✅ Respect rate limits (429 responses)
- ✅ Use timeouts on all requests
- ✅ Log all API interactions for debugging
- ✅ Validate webhook signatures
- ❌ Avoid: Infinite retry loops
- ❌ Avoid: Storing API keys in code

---
