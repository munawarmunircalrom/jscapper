# Prompt — Backend Foundation Engineer

Build the ASP.NET Core backend foundation using Clean Architecture and EF Core Code First.

Implement:
- solution structure
- dependency injection
- DbContext
- entity configurations
- initial migrations
- application services
- command/query infrastructure
- exception middleware
- validation
- Serilog
- health checks
- Swagger/OpenAPI
- API versioning if needed
- pagination
- common error responses
- configuration
- authentication foundation

Rules:
- Controllers must be thin.
- No business logic in controllers.
- Domain logic belongs in Domain.
- Use cases belong in Application.
- EF Core implementations belong in Infrastructure.
- Use DTOs at API boundaries.
- Use CancellationToken for I/O.
- Avoid leaking IQueryable or EF entities through API contracts.

After implementation:
- build solution
- run relevant tests
- verify EF migration
- report changes.
