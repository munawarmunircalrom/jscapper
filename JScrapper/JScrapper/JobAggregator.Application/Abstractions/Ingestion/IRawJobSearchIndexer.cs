using JobAggregator.Application.Abstractions.Providers;

namespace JobAggregator.Application.Abstractions.Ingestion;

public interface IRawJobSearchIndexer
{
    Task IndexAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken);
}
