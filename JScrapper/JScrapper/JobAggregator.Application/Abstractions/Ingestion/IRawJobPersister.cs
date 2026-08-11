using JobAggregator.Application.Abstractions.Providers;

namespace JobAggregator.Application.Abstractions.Ingestion;

public interface IRawJobPersister
{
    Task PersistAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken);
}
