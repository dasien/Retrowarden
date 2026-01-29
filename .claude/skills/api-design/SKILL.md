---
name: "API Design"
description: "Design RESTful APIs with proper resource modeling, HTTP methods, error handling, and clear contracts following REST principles"
category: "architecture"
required_tools: ["Read", "Write", "WebSearch"]
---

# API Design

## Purpose
Design clear, consistent, and maintainable REST APIs following industry best practices and conventions.

## When to Use
- Designing new API endpoints
- Refactoring existing APIs
- Creating integration interfaces
- Planning service-to-service communication

## Key Capabilities
1. **REST Principles** - Apply RESTful design patterns correctly
2. **Resource Modeling** - Design clear resource hierarchies
3. **Error Responses** - Define consistent error handling

## Approach
1. Identify resources and their relationships
2. Design URL structure (nouns, not verbs)
3. Choose appropriate HTTP methods (GET, POST, PUT, DELETE)
4. Design request/response formats
5. Plan error handling and status codes
6. Document with examples

## Example
**Context**: User profile management API

**Design**:
````
GET    /users/{id}           # Get user
POST   /users                # Create user
PUT    /users/{id}           # Update user
DELETE /users/{id}           # Delete user
GET    /users/{id}/profile   # Get profile
PUT    /users/{id}/profile   # Update profile
````

## Best Practices
- ✅ Use nouns for resources, HTTP verbs for actions
- ✅ Consistent URL patterns and naming
- ✅ Proper HTTP status codes (200, 201, 404, 500)
- ❌ Avoid: Verbs in URLs (/getUser, /createUser)