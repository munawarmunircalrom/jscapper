# Job Aggregation Platform — Clean Architecture (.NET 10)

## Context
The existing API started as a single ASP.NET Core template project. It has been refactored into a modular monolith with explicit architecture boundaries while preserving existing `/weatherforecast` behavior.

## Projects and boundaries
- **JobAggregator.Domain**
  - Entities, value objects, domain services.
  - No dependencies on EF Core, ASP.NET Core, or providers.
- **JobAggregator.Application**
  - Use cases, queries/handlers, interfaces, DTOs, validators.
  - Depends on Domain + Contracts.
- **JobAggregator.Infrastructure**
  - EF Core Code First, DbContext, entity configurations, repositories, provider implementations, integrations.
  - Depends on Application + Domain + Contracts.
- **JobAggregator.Api**
  - Controllers, middleware, authentication setup, DI composition root.
  - Depends on Application + Infrastructure.
- **JobAggregator.Worker**
  - Background ingestion orchestration host.
  - Depends on Application + Infrastructure.
- **JobAggregator.Contracts**
  - Cross-layer contracts and DTOs.

## Dependency direction
Domain -> nothing  
Application -> Domain (+ Contracts)  
Infrastructure -> Application + Domain (+ Contracts)  
API -> Application + Infrastructure (composition root only)  
Worker -> Application + Infrastructure

## EF Core approach
- EF Core is isolated in Infrastructure.
- Code First is mandatory.
- `JobAggregatorDbContext` + `IEntityTypeConfiguration<T>` implemented.
- Domain entities remain persistence-ignorant.

## Provider architecture
- Provider abstraction: `IJobSourceProvider` in Application.
- Provider implementations in `Infrastructure/JobSources`.
- Current providers scaffolded: LinkedIn, Indeed, Rozee, Jobi.
- Only authorized integration mechanisms are allowed.

## Preserved functionality
- Existing endpoint behavior is retained:
  - `GET /weatherforecast` returns forecast data.
- Business logic moved from controller to Application query handler.
