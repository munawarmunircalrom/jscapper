# Job Aggregation Platform — Visual Studio Custom Agents

Contains 20 Visual Studio/GitHub Copilot custom agents plus repository-wide Copilot instructions.

## Install into a repository

Extract this ZIP into the root of the Job Aggregator repository. The resulting structure is:

.github/
  agents/
    *.agent.md
  copilot-instructions.md

Visual Studio supports repository custom agents from `.github/agents/`. See Microsoft's current documentation for custom agents.

## User-level alternative

Copy the `.agent.md` files to `%USERPROFILE%\.github\agents` if you want them available across projects.

## Architecture
- .NET 10
- Angular 20
- Clean Architecture
- EF Core Code First
- SQL Server
- CQRS/MediatR where appropriate
- Provider abstraction for LinkedIn, Indeed, Rozee and Jobi

## Provider safety
The provider agents are explicitly restricted to authorized/permitted access. They must not bypass CAPTCHA, authentication, anti-bot systems, rate limits, robots restrictions or other access controls.
