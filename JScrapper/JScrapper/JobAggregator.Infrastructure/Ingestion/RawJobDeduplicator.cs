using JobAggregator.Application.Abstractions.Ingestion;
using JobAggregator.Application.Abstractions.Providers;

namespace JobAggregator.Infrastructure.Ingestion;

public sealed class RawJobDeduplicator : IRawJobDeduplicator
{
    public Task<IReadOnlyCollection<RawJob>> DeduplicateAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = new List<RawJob>(jobs.Count);

        foreach (var job in jobs)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var existing = result.FirstOrDefault(candidate =>
                string.Equals(candidate.ProviderName, job.ProviderName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(candidate.ExternalJobId, job.ExternalJobId, StringComparison.OrdinalIgnoreCase));

            if (existing is null)
            {
                existing = result.FirstOrDefault(candidate =>
                    !string.IsNullOrWhiteSpace(candidate.CanonicalUrl) &&
                    string.Equals(candidate.CanonicalUrl, job.CanonicalUrl, StringComparison.OrdinalIgnoreCase));
            }

            if (existing is null)
            {
                existing = result.FirstOrDefault(candidate =>
                    !string.IsNullOrWhiteSpace(candidate.ContentHash) &&
                    string.Equals(candidate.ContentHash, job.ContentHash, StringComparison.OrdinalIgnoreCase));
            }

            if (existing is null)
            {
                existing = result.FirstOrDefault(candidate =>
                    string.Equals(candidate.Company, job.Company, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(candidate.Title, job.Title, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(candidate.Location, job.Location, StringComparison.OrdinalIgnoreCase));
            }

            if (existing is null)
            {
                result.Add(job with { DeduplicationConfidence = 0.0 });
                continue;
            }

            var dedup = JobDeduplication.Evaluate(job, existing);
            var confidence = dedup.IsDuplicate ? dedup.Confidence : 0.0;

            // Keep all source jobs for provenance; confidence only informs downstream persistence/canonicalization.
            result.Add(job with { DeduplicationConfidence = confidence });
        }

        return Task.FromResult<IReadOnlyCollection<RawJob>>(result);
    }
}
