# Agents vs Prompts

Visual Studio treats these as two different extensibility mechanisms.

## Agents

Location:
`.github/agents/*.agent.md`

Invocation:
`@Agent Name`

Purpose:
Persistent specialized agent persona/workflow with workspace awareness and optional tools/model configuration.

## Prompts

Location:
`.github/prompts/*.prompt.md`

Invocation:
`/prompt-name`

Purpose:
Reusable prompt templates that can be invoked from the Copilot Chat slash menu.

The same project contains both because they solve different problems.
