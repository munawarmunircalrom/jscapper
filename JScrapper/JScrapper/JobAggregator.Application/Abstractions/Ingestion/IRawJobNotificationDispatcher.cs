using JobAggregator.Application.Abstractions.Providers;

namespace JobAggregator.Application.Abstractions.Ingestion;

public interface IRawJobNotificationDispatcher
{
    Task DispatchAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken);
}
