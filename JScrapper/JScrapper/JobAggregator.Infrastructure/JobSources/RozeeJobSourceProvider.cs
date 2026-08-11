using JobAggregator.Application.Abstractions.Providers;
using Microsoft.Extensions.Logging;

namespace JobAggregator.Infrastructure.JobSources;

public sealed class RozeeJobSourceProvider(ILogger<RozeeJobSourceProvider> logger)
    : JobSourceProviderBase(logger)
{
    public override string Name => "Rozee";

    public override JobProviderConfiguration Configuration { get; } = new()
    {
        Name = "Rozee",
        Enabled = true,
        TimeoutSeconds = 30,
        MaxRetries = 2,
        RetryDelayMilliseconds = 500,
        RequestsPerMinute = 20,
        DefaultPageSize = 50,
        MaxPageSize = 100
    };

    protected override Task<(IReadOnlyCollection<RawJob> Jobs, bool HasMore, string? NextCursor)> FetchPageCoreAsync(
        JobSearchRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<(IReadOnlyCollection<RawJob>, bool, string?)>(([], false, null));
    }
}
