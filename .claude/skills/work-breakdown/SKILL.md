---
name: "Work Breakdown"
description: "Break down features and requirements into actionable, implementable tasks with clear scope and acceptance criteria"
category: "analysis"
required_tools: ["Read", "Write", "Grep"]
---

# Work Breakdown

## Purpose
Break down large features, enhancements, or requirements into small, actionable tasks that can be implemented independently with clear success criteria.

## When to Use
- Planning implementation of new features
- Breaking down enhancement specifications
- Creating task lists for workflows
- Organizing complex work into manageable pieces
- Planning sprints or iterations

## Key Capabilities
1. **Task Identification** - Identify discrete, implementable work units
2. **Dependency Analysis** - Determine task order and relationships
3. **Scope Definition** - Create clear, bounded task descriptions

## Approach
1. **Read Requirements** - Understand the full scope of work
2. **Identify Components** - What parts of the system are affected?
3. **Break into Phases** - Architecture → Implementation → Testing → Documentation
4. **Define Tasks** - Each should be:
   - Completable in 2-8 hours
   - Independently testable
   - Has clear acceptance criteria
   - Assigned to appropriate agent
5. **Order by Dependencies** - What must happen first?
6. **Validate Completeness** - Do tasks cover all requirements?

## Example
**Context**: Feature - "Add user authentication with OAuth"

**Work Breakdown**:
```markdown
## Phase 1: Architecture & Design
**Task 1.1**: Design authentication flow
- Agent: architect
- Duration: 2-3 hours
- Output: Authentication sequence diagram
- Acceptance: Covers login, logout, token refresh

**Task 1.2**: Design database schema for users/sessions
- Agent: architect
- Duration: 2 hours
- Output: Schema definition with indexes
- Acceptance: Normalized, handles OAuth tokens

## Phase 2: Implementation
**Task 2.1**: Implement OAuth integration
- Agent: implementer
- Duration: 4-6 hours
- Dependencies: Task 1.1 complete
- Output: OAuth client code
- Acceptance: Can authenticate with Google/GitHub

**Task 2.2**: Create user session management
- Agent: implementer
- Duration: 3-4 hours
- Dependencies: Task 1.2, 2.1 complete
- Output: Session middleware
- Acceptance: Sessions persist, timeout after 24hrs

**Task 2.3**: Add authentication middleware
- Agent: implementer
- Duration: 2-3 hours
- Dependencies: Task 2.2 complete
- Output: Route protection middleware
- Acceptance: Protected routes require auth

## Phase 3: Testing
**Task 3.1**: Write authentication tests
- Agent: tester
- Duration: 3-4 hours
- Dependencies: Phase 2 complete
- Output: Test suite
- Acceptance: >90% coverage, tests auth flows

## Phase 4: Documentation
**Task 4.1**: Document OAuth setup
- Agent: documenter
- Duration: 2 hours
- Dependencies: Phase 2 complete
- Output: Setup guide
- Acceptance: Includes provider config steps
```

**Why This Breakdown Works**:
- Each task is 2-6 hours (right-sized)
- Clear dependencies prevent blocking
- Phases group related work
- Each has specific agent and acceptance criteria
- Can be implemented/tested independently

## Best Practices
- ✅ Tasks should be 2-8 hours of work (not days)
- ✅ Each task has clear acceptance criteria
- ✅ Assign appropriate agent to each task
- ✅ Group related tasks into phases
- ✅ Identify dependencies explicitly
- ✅ Ensure tasks cover all requirements
- ❌ Avoid: Tasks that are too large (>1 day)
- ❌ Avoid: Vague acceptance criteria ("make it work")
- ❌ Avoid: Circular dependencies between tasks