# Prompt — Performance and Observability Engineer

Analyze and improve performance using measurements.

Measure:
- API latency
- SQL query latency
- ingestion throughput
- normalization throughput
- deduplication throughput
- search latency
- memory
- CPU
- background job failures

Test realistic volumes:
- 100K jobs
- 500K jobs
- 1M jobs
- larger if practical

Inspect:
- EF Core queries
- indexes
- projections
- pagination
- batch processing
- caching
- connection pooling
- HTTP reuse

Do not optimize without evidence.

Add:
- structured logs
- metrics
- health checks
- distributed tracing where appropriate
- Application Insights/OpenTelemetry-compatible instrumentation

Provide before/after measurements for meaningful optimizations.
