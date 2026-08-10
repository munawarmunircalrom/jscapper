# Prompt — LinkedIn Integration Engineer

Implement the LinkedIn job provider only through an authorized and permitted access mechanism.

Before implementation:
1. Determine the supported access mechanism.
2. Document authorization requirements.
3. Document available fields.
4. Document rate limits.
5. Document technical/terms restrictions.
6. Define failure behavior.

Create provider-specific code under:
Infrastructure/JobSources/LinkedIn/

Map data into the common RawJob model.

Support:
- search
- pagination
- mapping
- retries
- timeout
- cancellation
- rate limiting
- logging
- health checks

Do not:
- bypass login
- bypass CAPTCHA
- defeat bot detection
- evade rate limits
- access private/authenticated content without authorization
- bypass robots or access controls

If the required capability cannot be implemented through a permitted mechanism, document the limitation rather than creating a bypass.
