namespace JobAggregator.Application.Configuration;

public sealed class SearchPlatformOptions
{
    public string DefaultProvider { get; set; } = "LinkedIn";

    public Dictionary<string, SearchPlatformEndpointOptions> Providers { get; set; } = new(StringComparer.OrdinalIgnoreCase)
    {
        ["LinkedIn"] = new SearchPlatformEndpointOptions { PublicEndpoint = "https://www.linkedin.com/jobs/search" },
        ["Indeed"] = new SearchPlatformEndpointOptions { PublicEndpoint = "https://www.indeed.com/jobs" },
        ["Rozee"] = new SearchPlatformEndpointOptions { PublicEndpoint = "https://www.rozee.pk/job/jsearch" },
        ["Jobi"] = new SearchPlatformEndpointOptions { PublicEndpoint = "https://jobi.pk/jobs" }
    };
}

public sealed class SearchPlatformEndpointOptions
{
    public string PublicEndpoint { get; set; } = string.Empty;
}
