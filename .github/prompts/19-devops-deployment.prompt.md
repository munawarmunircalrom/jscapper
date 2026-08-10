# Prompt — DevOps and Deployment Engineer

Prepare the platform for production.

Create/configure:
- Dockerfiles
- local Docker Compose where useful
- environment configuration
- CI/CD
- Azure deployment
- health checks
- monitoring
- logging

Recommended production architecture:
Angular hosting/CDN
-> ASP.NET Core API
-> SQL Server/Azure SQL
-> Redis
-> Worker
-> Job providers

Use:
- Azure SQL
- Azure Cache for Redis
- Azure Key Vault
- Application Insights
- Azure App Service or Azure Container Apps as appropriate

CI/CD:
Restore
-> Build
-> Unit Tests
-> Integration Tests
-> Security Scan
-> Container Build
-> Push
-> Staging Deploy
-> Smoke Tests
-> Approval
-> Production Deploy

Never commit secrets.
