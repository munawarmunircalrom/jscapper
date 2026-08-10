# Prompt — Rozee Integration Engineer

Implement Rozee job ingestion through an authorized/permitted mechanism.

Keep provider-specific implementation under:
Infrastructure/JobSources/Rozee/

Map available fields into RawJob:
- external job ID
- title
- company
- description
- location
- salary
- currency
- employment type
- experience
- skills
- posted date
- updated date
- original URL

Implement:
- pagination
- retry
- timeout
- cancellation
- rate limiting
- logging
- provider health
- idempotency
- tests

Do not bypass authentication, CAPTCHA, anti-bot controls, robots restrictions or access controls.
