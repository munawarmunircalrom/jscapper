# ADR 0002: EF Core Code First in Infrastructure

## Status
Accepted

## Decision
Use EF Core Code First with DbContext and entity type configurations in Infrastructure only.

## Rationale
- Aligns with architecture constraints and source-of-truth model ownership.
- Prevents Domain/Application from coupling to EF Core.
- Enables migration-based schema evolution.

## Consequences
- Migrations must be created and reviewed as part of delivery.
- Infrastructure owns persistence details.
