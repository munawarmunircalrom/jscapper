using JobAggregator.Contracts.Jobs;

namespace JobAggregator.Application.Abstractions.Providers;

public interface IJobSourceProvider
{
    string Name { get; }
    Task<IReadOnlyCollection<RawJobContract>> FetchJobsAsync(CancellationToken cancellationToken);
}
