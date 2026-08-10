# Job Aggregation Platform — Global Copilot Instructions

## Stack
- .NET 10 / ASP.NET Core Web API
- Angular 20, Standalone Components, Signals, RxJS, PrimeNG
- EF Core + SQL Server
- Clean Architecture
- EF Core Code First (mandatory)
- CQRS/MediatR where appropriate
- FluentValidation, Serilog
- Hangfire or Quartz.NET
- Redis where appropriate
- Docker/Azure/CI-CD

## Clean Architecture
Projects:
- JobAggregator.Domain
- JobAggregator.Application
- JobAggregator.Infrastructure
- JobAggregator.Api
- JobAggregator.Worker
- JobAggregator.Contracts

Dependency direction:
Domain -> nothing
Application -> Domain
Infrastructure -> Application + Domain
API -> Application; Infrastructure only at composition root
Worker -> Application + Infrastructure

Domain must not reference EF Core, ASP.NET Core, Angular, or provider implementations.

## EF Core Code First
- Code First is mandatory; never use Database First.
- Entities/configurations are the model source of truth.
- DbContext belongs in Infrastructure.
- Use IEntityTypeConfiguration<T>.
- Generate and review migrations.
- Do not reverse-engineer the database.
- Use explicit relationships, constraints, indexes and concurrency where needed.
- Avoid N+1 queries.

## Job Provider Architecture
All providers implement IJobSourceProvider.
Providers live under Infrastructure/JobSources/.

Pipeline:
Provider -> RawJob -> Normalize -> Validate -> Deduplicate -> Persist -> Search -> Alert -> Notification

Provider-specific models must not leak into Domain/Application.

Required providers:
LinkedIn, Indeed, Rozee, Jobi.

## External Access
Use official APIs, permitted feeds, partner integrations, or other authorized mechanisms.
Never bypass CAPTCHA, authentication, anti-bot controls, rate limits, robots/access restrictions, or private/authenticated content.

## Coding
Use SOLID, DRY, KISS, async/await, CancellationToken, DI, nullable reference types and structured logging.
Controllers are thin. Business logic belongs in Domain/Application.
No secrets in source control.

## Angular
Use standalone components, Signals for local state, RxJS for async workflows, PrimeNG, reusable components and API services.

## Agent Workflow
Before changing code:
1. Inspect the repository.
2. Identify affected projects/files.
3. Check architecture boundaries.
4. Implement the smallest maintainable change.
5. Build affected projects.
6. Run relevant tests.
7. Report changes and validation.
Do not undo unrelated work.
