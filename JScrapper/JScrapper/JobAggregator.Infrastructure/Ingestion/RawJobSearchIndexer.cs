using JobAggregator.Application.Abstractions.Ingestion;
using JobAggregator.Application.Abstractions.Providers;
using Microsoft.Extensions.Logging;

namespace JobAggregator.Infrastructure.Ingestion;

public sealed class RawJobSearchIndexer(ILogger<RawJobSearchIndexer> logger) : IRawJobSearchIndexer
{
    public Task IndexAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        logger.LogInformation("Search stage indexed {Count} raw jobs.", jobs.Count);
        return Task.CompletedTask;
    }
}
