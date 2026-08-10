using JobAggregator.Application.Abstractions.Providers;
using JobAggregator.Contracts.Jobs;

namespace JobAggregator.Infrastructure.JobSources;

public sealed class RozeeJobSourceProvider : IJobSourceProvider
{
    public string Name => "Rozee";

    public Task<IReadOnlyCollection<RawJobContract>> FetchJobsAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyCollection<RawJobContract>>([]);
    }
}
