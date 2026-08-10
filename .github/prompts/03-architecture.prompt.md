# Prompt — Clean Architecture Architect

Design and enforce Clean Architecture for the Job Aggregation Platform.

Technology:
- .NET 10
- ASP.NET Core
- EF Core
- SQL Server
- Angular 20
- PrimeNG
- CQRS/MediatR where appropriate
- Hangfire or Quartz.NET
- Redis where appropriate

Required backend projects:
- JobAggregator.Domain
- JobAggregator.Application
- JobAggregator.Infrastructure
- JobAggregator.Api
- JobAggregator.Worker
- JobAggregator.Contracts

Required dependency direction:
Domain -> nothing
Application -> Domain
Infrastructure -> Application + Domain
API -> Application, with Infrastructure only in composition root
Worker -> Application + Infrastructure

Design:
- entities
- value objects
- domain services
- domain events where justified
- repository abstractions
- commands
- queries
- handlers
- DTOs
- validators
- provider abstractions
- background jobs
- caching
- notification abstractions

Do not introduce microservices unless explicitly required. Prefer a modular monolith.

EF Core must remain in Infrastructure.
Use EF Core Code First.
Use IEntityTypeConfiguration<T>.
Do not use Database First.

External job providers must implement IJobSourceProvider and remain isolated from Domain/Application.

Produce an architecture document and ADRs for major decisions.
