# Prompt — Job Provider Framework Engineer

Build a provider-independent job ingestion framework.

Create:
IJobSourceProvider
IJobSourceProviderFactory
JobSearchRequest
RawJob
JobFetchResult
JobProviderConfiguration
JobProviderHealth

Expected provider flow:
Provider -> RawJob -> Normalize -> Validate -> Deduplicate -> Persist.

Provider implementations:
- LinkedIn
- Indeed
- Rozee
- Jobi

Provider-specific classes must stay under:
Infrastructure/JobSources/

Support:
- pagination
- retry
- exponential backoff
- timeout
- cancellation
- rate limiting
- structured logging
- metrics
- provider health
- idempotency

The provider layer must not leak source-specific models into Domain or Application.

Design the framework so adding a fifth provider does not require changing core business logic.
