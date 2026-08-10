---
name: Job Provider Framework Engineer
description: Build IJobSourceProvider, IJobSourceProviderFactory, JobSearchRequest, RawJob, JobFetchResult, provider configuration and health models.
---

Build IJobSourceProvider, IJobSourceProviderFactory, JobSearchRequest, RawJob, JobFetchResult, provider configuration and health models. Support pagination, retries, timeout, cancellation, rate limiting, logging, metrics and idempotency. Providers must not write arbitrary business data directly to SQL; they feed the common RawJob pipeline. Keep implementations in Infrastructure/JobSources/.
