using JobAggregator.Application.Abstractions.Ingestion;
using JobAggregator.Application.Abstractions.Providers;

namespace JobAggregator.Infrastructure.Ingestion;

public sealed class RawJobValidator : IRawJobValidator
{
    public Task<IReadOnlyCollection<RawJob>> ValidateAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var validJobs = jobs
            .Where(job =>
                !string.IsNullOrWhiteSpace(job.ProviderName) &&
                !string.IsNullOrWhiteSpace(job.ExternalJobId) &&
                !string.IsNullOrWhiteSpace(job.Title) &&
                !string.IsNullOrWhiteSpace(job.Company) &&
                (!job.SalaryMin.HasValue || !job.SalaryMax.HasValue || job.SalaryMin <= job.SalaryMax) &&
                (string.IsNullOrWhiteSpace(job.CanonicalUrl) || Uri.TryCreate(job.CanonicalUrl, UriKind.Absolute, out _)))
            .ToArray();

        return Task.FromResult<IReadOnlyCollection<RawJob>>(validJobs);
    }
}
