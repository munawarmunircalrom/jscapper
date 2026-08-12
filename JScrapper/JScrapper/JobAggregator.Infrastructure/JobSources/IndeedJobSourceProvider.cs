using JobAggregator.Application.Abstractions.Providers;
using JobAggregator.Application.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JobAggregator.Infrastructure.JobSources;

public sealed class IndeedJobSourceProvider(
    ILogger<IndeedJobSourceProvider> logger,
    IOptions<SearchPlatformOptions> searchPlatformOptions)
    : JobSourceProviderBase(logger)
{
    public override string Name => "Indeed";

    public override JobProviderConfiguration Configuration { get; } = new()
    {
        Name = "Indeed",
        Enabled = true,
        BaseUrl = searchPlatformOptions.Value.Providers.TryGetValue("Indeed", out var endpoint)
            ? endpoint.PublicEndpoint
            : "https://www.indeed.com/jobs",
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
