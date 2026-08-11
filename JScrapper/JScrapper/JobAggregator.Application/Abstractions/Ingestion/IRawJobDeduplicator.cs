using JobAggregator.Application.Abstractions.Providers;

namespace JobAggregator.Application.Abstractions.Ingestion;

public interface IRawJobDeduplicator
{
    Task<IReadOnlyCollection<RawJob>> DeduplicateAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken);
}
