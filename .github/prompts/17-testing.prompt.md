# Prompt — QA and Test Automation Engineer

Create a complete automated test strategy.

Backend:
- Domain unit tests
- Application unit tests
- Infrastructure integration tests
- API tests
- EF Core Code First tests
- provider tests
- normalization tests
- deduplication tests
- search tests
- auth/authorization tests
- background job tests

Frontend:
- component tests
- service tests
- search/filter tests
- authentication tests

Critical scenarios:
1. Same job from multiple providers.
2. Same job with different URLs.
3. Slightly different company names.
4. Missing salary.
5. Missing location.
6. HTML descriptions.
7. Provider timeout.
8. Provider outage.
9. Duplicate ingestion.
10. Scheduler retry.
11. Unauthorized admin access.

Test business behavior, not private implementation details.

Build the solution and run relevant test suites before declaring work complete.
