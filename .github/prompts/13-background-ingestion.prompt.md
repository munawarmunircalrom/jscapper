# Prompt — Background Processing Engineer

Build scheduled ingestion using Hangfire or Quartz.NET.

Schedules must be configuration/database driven.

Pipeline:
Scheduler
-> Provider
-> Fetch
-> Normalize
-> Validate
-> Deduplicate
-> Persist
-> Alert Matching
-> Notification

Implement:
- retry
- exponential backoff
- timeout
- cancellation
- concurrency/distributed locking
- idempotency
- execution history
- failure tracking
- provider health
- metrics

Provider schedules must be independently configurable.

One provider failure must not stop other providers.

Use correct dependency injection scopes inside background jobs.

Add integration tests for retries, duplicate execution and provider failure.
