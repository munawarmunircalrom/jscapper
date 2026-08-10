# ADR 0001: Adopt Clean Architecture as a Modular Monolith

## Status
Accepted

## Decision
Refactor the single API project into a modular monolith with dedicated projects:
- JobAggregator.Domain
- JobAggregator.Application
- JobAggregator.Infrastructure
- JobAggregator.Api
- JobAggregator.Worker
- JobAggregator.Contracts

## Rationale
- Enforce dependency direction and separation of concerns.
- Keep Domain pure and independent of frameworks/providers.
- Improve maintainability and testability.
- Preserve a single deployable backend API while enabling growth.

## Consequences
- More projects and DI setup complexity.
- Clear boundaries for future features and team scaling.
