# Prompt — EF Core Code First Database Architect

Design the SQL Server database using EF Core Code First.

Mandatory:
- Code First only
- DbContext in Infrastructure
- IEntityTypeConfiguration<T> for mappings
- migrations generated with EF Core tooling
- no Database First
- no reverse engineering

Core entities:
Job
Company
JobSource
JobSourcePosting
JobLocation
JobSkill
JobSalary
JobApplication
SavedJob
JobAlert
User
UserPreference
JobIngestionRun
JobIngestionError
JobDuplicate
Notification
AuditLog

Design:
- primary keys
- foreign keys
- relationships
- unique constraints
- indexes
- concurrency
- audit fields
- soft deletion where justified
- canonical job/source-posting relationship
- deduplication identifiers
- search-supporting fields

The same canonical Job may have multiple JobSourcePosting records.

For schema changes:
1. Modify entity model.
2. Modify IEntityTypeConfiguration<T>.
3. Generate migration.
4. Review migration.
5. Add/update tests.
6. Build affected projects.

Do not manually create SQL tables as the primary implementation.
