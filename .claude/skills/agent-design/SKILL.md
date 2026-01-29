---
name: "Agent Design"
description: "Design AI agents with appropriate capabilities, tools, and personas for specific software development tasks"
category: "architecture"
required_tools: ["Read", "Write", "Grep", "Glob"]
---

# Agent Design

## Purpose
Design effective AI agents for the CMAT (Claude Multi-Agent Template) system by selecting appropriate tools, skills, and defining clear personas that enable agents to accomplish specific software development tasks.

## When to Use
- Creating new agents for workflows
- Defining agent capabilities and responsibilities
- Selecting tools and skills for agent tasks
- Designing agent collaboration patterns
- Refining existing agent definitions

## Key Capabilities
1. **Capability Mapping** - Match requirements to agent tools and skills
2. **Persona Design** - Define clear agent roles and instructions
3. **Tool Selection** - Choose appropriate tools for agent capabilities

## Approach
1. **Understand the Need** - What task should this agent accomplish?
2. **Define Role & Responsibilities** - What is the agent's purpose and scope?
3. **Select Tools** - What capabilities does the agent need?
   - Read/Write/List: File operations
   - Glob/Grep: Code searching
   - Edit/MultiEdit: Code modifications
   - Bash: Running commands/tests
   - WebSearch/WebFetch: External research
4. **Choose Skills** - What domain expertise is needed?
5. **Write Persona** - Clear instructions, examples, best practices
6. **Define Outputs** - What artifacts should the agent produce?

## Example
**Context**: Need an agent to analyze API designs

**Agent Design**:
```markdown
---
name: API Reviewer
role: API Design Analysis & Review
description: Reviews API designs for REST compliance and best practices
tools:
  - Read       # Read API specs
  - Write      # Write review reports
  - Grep       # Search for patterns
  - WebSearch  # Research API standards
skills:
  - api-design
  - technical-writing
---

# API Reviewer Agent

You are an expert API architect reviewing API designs.

## Your Responsibilities
- Analyze API endpoint designs
- Check REST compliance
- Identify inconsistencies
- Suggest improvements

## Review Checklist
1. RESTful design (nouns, not verbs)
2. Proper HTTP methods
3. Consistent error handling
4. Clear documentation

## Output
Create a review report in markdown with:
- Summary assessment
- Specific findings
- Recommendations
```

**Why These Choices**:
- **Tools**: Read (specs), Write (reports), Grep (patterns), WebSearch (standards)
- **Skills**: api-design (domain expertise), technical-writing (clear reports)
- **Persona**: Focused on review checklist and report format

## Best Practices
- ✅ Give agents single, clear responsibilities
- ✅ Match tools to actual capabilities needed
- ✅ Include relevant skills for domain knowledge
- ✅ Provide examples and templates in persona
- ✅ Define expected output format clearly
- ✅ Keep personas focused (200-500 lines)
- ❌ Avoid: Agents with too many unrelated responsibilities
- ❌ Avoid: Giving all tools "just in case"
- ❌ Avoid: Vague persona instructions