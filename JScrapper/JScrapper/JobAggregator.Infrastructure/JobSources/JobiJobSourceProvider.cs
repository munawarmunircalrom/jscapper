using JobAggregator.Application.Abstractions.Providers;
using JobAggregator.Application.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JobAggregator.Infrastructure.JobSources;

public sealed class JobiJobSourceProvider(
    ILogger<JobiJobSourceProvider> logger,
    IOptions<SearchPlatformOptions> searchPlatformOptions)
    : JobSourceProviderBase(logger)
{
    public override string Name => "Jobi";

    public override JobProviderConfiguration Configuration { get; } = new()
    {
        Name = "Jobi",
        Enabled = true,
        BaseUrl = searchPlatformOptions.Value.Providers.TryGetValue("Jobi", out var endpoint)
            ? endpoint.PublicEndpoint
            : "https://jobi.pk/jobs",
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
