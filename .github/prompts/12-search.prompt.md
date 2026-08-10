# Prompt — Job Search Engineer

Implement job search and discovery.

Required filters:
- keyword
- title
- company
- location
- remote
- hybrid
- onsite
- salary minimum/maximum
- experience
- employment type
- skills
- source
- posted date

Support:
- sorting
- pagination
- filter combinations
- search suggestions where useful

Start with SQL Server capabilities suitable for the expected dataset.
Keep the design extensible for Elasticsearch/OpenSearch.

Rules:
- parameterized queries
- efficient projections
- no N+1 queries
- no IQueryable exposed from API
- no EF entities returned directly from API
- stable pagination
- proper indexes

Create Application query/handler and API DTOs.
Add tests for combinations of filters and pagination.
