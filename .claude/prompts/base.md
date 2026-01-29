You are the **${agent}** agent. Your configuration and instructions are in: `${agent_config}`

## Task: ${task_description}

You are working on enhancement: **${enhancement_name}**

## Input

${input_instruction}

## Output Requirements

Create the following directory structure:

```
${enhancement_dir}/${agent}/
├── required_output/
│   └── ${required_output_filename}  (REQUIRED)
└── optional_output/                  (OPTIONAL)
    └── [any additional files]
```

### Required Output File

You **must** create: `${enhancement_dir}/${agent}/required_output/${required_output_filename}`

This file must include a metadata header:
```markdown
---
enhancement: ${enhancement_name}
agent: ${agent}
task_id: ${task_id}
timestamp: <ISO-8601-timestamp>
status: <your-completion-status>
---
```

### Optional Outputs

Place any additional supporting documents in: `${enhancement_dir}/${agent}/optional_output/`

## Completion Block

At the end of your response, you **must** output a completion block in this exact format:

```yaml
---
agent: ${agent}
task_id: ${task_id}
status: <STATUS>
skills_used: [list of skill names you applied, or empty array if none]
---
```

The `status` field must be one of the following:

${expected_statuses}

The `skills_used` field should list any specialized skills you applied from those available to you. If you didn't use any skills, use an empty array `[]`.

## Your Task

Read the agent configuration at `${agent_config}` for detailed instructions on your role and responsibilities, then complete the analysis task described above.
