# Prompt — Jobi Integration Engineer

Implement Jobi job ingestion through an authorized/permitted mechanism.

Keep provider-specific code under:
Infrastructure/JobSources/Jobi/

Map supported fields into RawJob.

Implement:
- pagination
- retry
- timeout
- cancellation
- rate limiting
- logging
- provider health
- idempotency
- integration tests

Do not bypass access controls, authentication, CAPTCHA, anti-bot systems, rate limits or robots restrictions.

If access is not permitted, document the limitation and do not implement a workaround.
