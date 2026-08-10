# Job Aggregation Platform — Visual Studio Copilot Agents + Prompts

This package is structured for Visual Studio GitHub Copilot.

## Custom Agents — use with @

Agents are located in:

.github/agents/

Use them with the `@` syntax, for example:

@Job Aggregator Orchestrator
@Clean Architecture Architect
@EF Core Code First Database Architect
@Angular 20 Frontend Engineer

## Custom Prompts — use with /

Prompts are located in:

.github/prompts/

Type `/` in Copilot Chat and the custom prompt files should appear in the IntelliSense list.

Examples:
- /01-orchestrator
- /02-requirements
- /03-architecture
- /04-database-code-first
- /05-backend-foundation
- /06-provider-framework
- /07-linkedin
- /08-indeed
- /09-rozee
- /10-jobi
- /11-normalization-deduplication
- /12-search
- /13-background-ingestion
- /14-angular
- /15-auth-security
- /16-alerts-notifications
- /17-testing
- /18-performance-observability
- /19-devops-deployment
- /20-deployment-validation

## Architecture

- .NET 10
- ASP.NET Core
- Angular 20
- Clean Architecture
- EF Core Code First
- SQL Server
- CQRS/MediatR where appropriate
- Provider abstraction
- Background ingestion
- Normalization and deduplication
- Testing
- Azure deployment

## Provider safety

Provider agents/prompts require authorized/permitted access. They must not bypass CAPTCHA, authentication, anti-bot controls, rate limits, robots restrictions or other access controls.

## Installation

Extract the ZIP into the root of the repository so that `.github/agents/` and `.github/prompts/` are directly under the repository root.

After extraction, reopen the solution or restart/refresh Copilot Chat if the new prompts are not immediately shown.
