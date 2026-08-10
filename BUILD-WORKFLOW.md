# Recommended Agent Execution Order

1. @Requirements Analyst
2. @Clean Architecture Architect
3. @EF Core Code First Database Architect
4. @Backend Foundation Engineer
5. @Job Provider Framework Engineer
6. @LinkedIn Integration Engineer
7. @Indeed Integration Engineer
8. @Rozee Integration Engineer
9. @Jobi Integration Engineer
10. @Job Data Quality Engineer
11. @Job Search Engineer
12. @Background Processing Engineer
13. @Angular 20 Frontend Engineer
14. @Security and Identity Engineer
15. @Job Alerts and Notifications Engineer
16. @QA and Test Automation Engineer
17. @Performance and Observability Engineer
18. @DevOps and Deployment Engineer
19. @Production Validation Engineer

Use @Job Aggregator Orchestrator when coordinating multi-step work.

## Key architectural rule
EF Core Code First is mandatory. Create/update C# entities and IEntityTypeConfiguration<T> first, then generate/review migrations. Do not use Database First.
