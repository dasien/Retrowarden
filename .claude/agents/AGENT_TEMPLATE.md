# AGENT.md Template

Copy and customize the template below to create a new specialized agent:

```markdown
---
name: agent-name
display-name: Agent Name
role: agent-role
description: One-line description of what this agent does and when to use it
tools: ["Tool1", "Tool2", "Tool3"]
skills: ["skill-1", "skill-2"]
validations:
  metadata_required: true
---

# Agent Name

## Role and Purpose

You are a specialized [Agent Name] agent responsible for [primary responsibility area].

**Key Principle**: [Core operating principle - one sentence defining the agent's fundamental approach]

**Workflow Integration**: This agent is invoked by workflows that specify its input sources and required outputs.

## Core Responsibilities

### 1. [Primary Responsibility Area]
- [Specific task or capability]
- [Specific task or capability]
- [Specific task or capability]
- [Specific task or capability]

### 2. [Secondary Responsibility Area]
- [Specific task or capability]
- [Specific task or capability]
- [Specific task or capability]
- [Specific task or capability]

### 3. [Tertiary Responsibility Area]
- [Specific task or capability]
- [Specific task or capability]
- [Specific task or capability]

### 4. [Additional Responsibility Area]
- [Specific task or capability]
- [Specific task or capability]
- [Specific task or capability]

## When to Use This Agent

### ✅ Use [agent-name] when:
- [Specific scenario or trigger]
- [Specific scenario or trigger]
- [Specific scenario or trigger]
- [Specific scenario or trigger]
- [Specific scenario or trigger]

### ❌ Don't use [agent-name] when:
- [Inappropriate scenario]
- [Inappropriate scenario]
- [Inappropriate scenario]
- [Inappropriate scenario]

## Output Standards

### [Output Type] Should Include:
- **[Component 1]**: [Description of what this should contain]
- **[Component 2]**: [Description of what this should contain]
- **[Component 3]**: [Description of what this should contain]
- **[Component 4]**: [Description of what this should contain]
- **[Component 5]**: [Description of what this should contain]

### Quality Standards:
- ✅ **[Quality Attribute 1]**: [Description]
- ✅ **[Quality Attribute 2]**: [Description]
- ✅ **[Quality Attribute 3]**: [Description]
- ✅ **[Quality Attribute 4]**: [Description]
- ✅ **[Quality Attribute 5]**: [Description]

## Success Criteria

- ✅ [Measurable success criterion]
- ✅ [Measurable success criterion]
- ✅ [Measurable success criterion]
- ✅ [Measurable success criterion]
- ✅ [Measurable success criterion]
- ✅ [Measurable success criterion]

## Scope Boundaries

### ✅ DO:
- [Action within scope]
- [Action within scope]
- [Action within scope]
- [Action within scope]
- [Action within scope]
- [Action within scope]
- [Action within scope]

### ❌ DO NOT:
- [Action outside scope]
- [Action outside scope]
- [Action outside scope]
- [Action outside scope]
- [Action outside scope]
- [Action outside scope]
- [Action outside scope]

## Project-Specific Customization

[**NOTE TO TEMPLATE USER**: Customize this section for your project]

**Example customizations**:
- [Project-specific setting or convention]
- [Project-specific setting or convention]
- [Project-specific setting or convention]
- [Project-specific setting or convention]
- [Project-specific setting or convention]
- [Project-specific setting or convention]

## [Key Practice Area] Best Practices

### [Sub-area 1]
- [Specific best practice]
- [Specific best practice]
- [Specific best practice]
- [Specific best practice]

### [Sub-area 2]
- [Specific best practice]
- [Specific best practice]
- [Specific best practice]
- [Specific best practice]

### [Sub-area 3]
- [Specific best practice]
- [Specific best practice]
- [Specific best practice]
- [Specific best practice]

## Communication

- [Communication guideline]
- [Communication guideline]
- [Communication guideline]
- [Communication guideline]
- [Communication guideline]
- [Communication guideline]
- [Communication guideline]
```

## Agent Configuration (agents.json)

When you create a new agent file, you must also add an entry to `.claude/agents/agents.json`:

```json
{
  "name": "agent-name",
  "display_name": "Agent Name",
  "agent-file": "agent-filename",
  "role": "agent-role",
  "tools": [
    "Tool1",
    "Tool2"
  ],
  "skills": [
    "skill-1",
    "skill-2"
  ],
  "description": "One-line description matching the agent file",
  "validations": {
    "metadata_required": true
  }
}
```

### Field Descriptions:

- **name**: Slug-based identifier (lowercase-with-hyphens, must match filename)
- **display_name**: Display name of the agent for UI (title case, can contain spaces)
- **agent-file**: Filename without the .md extension (must match the actual filename)
- **role**: Agent's role category. Common values:
  - `analysis` - Analyzing requirements, code, or systems
  - `design` - Architecture and design work
  - `implementation` - Writing production code
  - `testing` - Quality assurance and validation
  - `documentation` - Creating documentation
  - `integration` - Managing external platform integrations
- **tools**: Array of Claude Code tools the agent can use. Available tools:
  - `Read` - Read files
  - `Write` - Create new files
  - `Edit` - Modify existing files
  - `Bash` - Execute shell commands
  - `Glob` - Find files by pattern
  - `Grep` - Search file contents
  - `WebSearch` - Search the web
  - `WebFetch` - Fetch web content
  - `Task` - Spawn sub-agents
- **skills**: Array of skill names the agent has access to (must match skill files in `.claude/skills/`)
- **description**: Brief description of the agent's purpose (keep under 120 characters)
- **validations**: Configuration for validation requirements
  - `metadata_required`: Set to `true` if the agent must produce output with metadata headers

## Common Agent Roles

### Analysis Agents
**Purpose**: Examine code, requirements, or systems to understand and document findings
**Common Tools**: Read, Glob, Grep, WebSearch, WebFetch, Write
**Examples**: Requirements Analyst, Code Reviewer, Bug Triager

### Design Agents
**Purpose**: Create architectural plans, technical specifications, and design decisions
**Common Tools**: Read, Write, Edit, Glob, Grep, WebSearch, WebFetch
**Examples**: Architect, API Designer, Database Designer

### Implementation Agents
**Purpose**: Write production code based on specifications
**Common Tools**: Read, Write, Edit, Bash, Glob, Grep, Task
**Examples**: Implementer, Refactorer, Bug Fixer

### Testing Agents
**Purpose**: Validate functionality, quality, and performance
**Common Tools**: Read, Write, Edit, Bash, Glob, Grep, Task
**Examples**: Tester, Performance Tester, Security Tester

### Documentation Agents
**Purpose**: Create and maintain project documentation
**Common Tools**: Read, Write, Edit, Glob, Grep
**Examples**: Documenter, API Documentation Writer, Tutorial Creator

### Integration Agents
**Purpose**: Coordinate with external platforms and services
**Common Tools**: Often none (uses external APIs via specialized mechanisms)
**Examples**: GitHub Integration Coordinator, Jira Integration Coordinator

## Best Practices for Agent Design

### 1. Single Responsibility
Each agent should have one clear, well-defined purpose. If an agent is trying to do too many different things, consider splitting it into multiple specialized agents.

### 2. Clear Handoffs
Define clear success criteria and status outputs so workflows know when to transition to the next agent. Use consistent status patterns across agents.

### 3. Minimal Tool Sets
Only grant the tools an agent actually needs. For example, integration coordinators often need no standard tools, while implementation agents need Read, Write, Edit capabilities.

### 4. Appropriate Skills
Assign skills that directly support the agent's responsibilities. Skills provide domain-specific knowledge and approaches.

### 5. Explicit Boundaries
Clearly document what the agent should and shouldn't do. This prevents scope creep and helps workflows assign work appropriately.

### 6. Metadata Discipline
For agents that produce artifacts consumed by other agents, require metadata headers. For integration agents that just coordinate with external systems, metadata may not be needed.

### 7. Project Customization
Include a section for project-specific customization so teams can adapt the agent to their specific needs, conventions, and standards.

### 8. Workflow Awareness
Design agents to work within workflows. They should accept inputs from specified sources and produce outputs in predictable locations with clear status indicators.

## Testing Your Agent

After creating a new agent:

1. **Validate JSON syntax** in `agents.json`
2. **Test the agent** in isolation with sample inputs
3. **Verify tool access** - ensure the agent can use its assigned tools
4. **Check skill integration** - confirm assigned skills are accessible
5. **Test in a workflow** - ensure the agent integrates properly with workflow orchestration
6. **Verify status transitions** - confirm status outputs trigger correct workflow behavior
7. **Review output quality** - check that outputs meet the documented standards

## Examples

See existing agents for reference implementations:
- **architect.md** - Complex design agent with multiple responsibilities
- **implementer.md** - Code-writing agent with clear scope boundaries
- **tester.md** - Quality assurance agent with comprehensive testing strategies
- **code-reviewer.md** - Analysis agent focused on code quality
- **documenter.md** - Documentation-focused agent
- **requirements-analyst.md** - Analysis and planning agent
- **github-integration-coordinator.md** - External integration agent