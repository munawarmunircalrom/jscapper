# Prompt — Production Validation Engineer

Validate the deployed Job Aggregation Platform without making uncontrolled production changes.

Check:
- Angular frontend
- API
- SQL Server/Azure SQL
- Redis
- Worker
- scheduler
- provider health
- authentication
- search
- filtering
- saved jobs
- alerts
- notifications
- logging
- health endpoints

Validate:
- /health
- readiness/liveness endpoints if configured
- Swagger/OpenAPI where enabled
- API latency
- failed ingestion
- failed background jobs
- authentication failures

Produce:
1. deployment status
2. tests performed
3. failures
4. likely root causes
5. safe remediation recommendations
6. rollback recommendation if required

Do not modify production configuration unless explicitly authorized.
