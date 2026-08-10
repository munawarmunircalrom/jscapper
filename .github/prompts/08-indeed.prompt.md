# Prompt — Indeed Integration Engineer

Implement Indeed integration only through an authorized/permitted mechanism.

Keep all provider-specific code under:
Infrastructure/JobSources/Indeed/

Implement as appropriate:
- IndeedJobSourceProvider
- provider configuration
- mapper/parser
- health check
- integration tests

Map all supported data into RawJob.

Support:
- pagination
- retry
- timeout
- cancellation
- rate limiting
- structured logging
- provider health
- idempotency

Do not bypass:
- authentication
- CAPTCHA
- anti-bot systems
- rate limits
- access restrictions

If a required capability is unavailable through an authorized mechanism, stop and document the constraint.
