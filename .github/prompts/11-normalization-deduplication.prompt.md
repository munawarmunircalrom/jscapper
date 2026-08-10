# Prompt — Job Data Quality Engineer

Build the normalization and deduplication pipeline.

Normalize:
- title
- company
- description
- location
- salary
- currency
- employment type
- experience
- skills
- posted/updated dates
- URLs

Normalization should handle:
- whitespace
- casing
- HTML
- company aliases
- location variations
- salary formats
- employment type variations
- experience wording
- canonical URLs

Deduplication signals:
1. provider + external job ID
2. canonical URL
3. content hash
4. normalized company + title + location
5. description similarity
6. salary similarity
7. posting-date proximity

Create deterministic rules plus a confidence score.

Use a canonical Job and preserve source-specific JobSourcePosting records.

Do not destroy provenance when deduplicating.
Add tests for cross-provider duplicates and near-duplicates.
