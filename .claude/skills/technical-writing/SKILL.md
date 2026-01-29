---
name: "Technical Writing"
description: "Create clear, accessible documentation for technical and non-technical audiences with practical examples and logical structure"
category: "documentation"
required_tools: ["Read", "Write", "Edit"]
---

# Technical Writing

## Purpose
Create clear, accurate documentation that helps users understand and use software effectively, regardless of their technical background.

## When to Use
- Writing user guides and tutorials
- Creating README files
- Documenting features
- Explaining complex concepts

## Key Capabilities
1. **Clarity** - Write simple, jargon-free explanations
2. **Structure** - Organize information logically
3. **Examples** - Provide practical, working examples

## Approach
1. Know your audience (developers vs end-users)
2. Start with the "why" before the "how"
3. Use clear headings and sections
4. Provide concrete examples
5. Include troubleshooting for common issues

## Example
**Bad**: "The API utilizes RESTful paradigms for CRUD operations"

**Good**: 
````markdown
## Creating a Task

To create a new task, send a POST request:
```bash
POST /api/tasks
{
  "title": "Fix login bug",
  "priority": "high"
}
```

The API returns the created task with an ID you can use to track progress.
````

## Best Practices
- ✅ Use active voice ("Click the button" not "The button should be clicked")
- ✅ Include working code examples
- ✅ Explain error messages users might see
- ❌ Avoid: Assuming prior knowledge without explanation