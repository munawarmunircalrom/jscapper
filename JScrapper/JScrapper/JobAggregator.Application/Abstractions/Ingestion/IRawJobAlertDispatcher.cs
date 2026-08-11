using JobAggregator.Application.Abstractions.Providers;

namespace JobAggregator.Application.Abstractions.Ingestion;

public interface IRawJobAlertDispatcher
{
    Task DispatchAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken);
}
