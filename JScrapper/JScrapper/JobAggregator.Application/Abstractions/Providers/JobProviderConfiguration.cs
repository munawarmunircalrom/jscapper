namespace JobAggregator.Application.Abstractions.Providers;

public sealed class JobProviderConfiguration
{
    public required string Name { get; init; }
    public bool Enabled { get; init; } = true;
    public string? BaseUrl { get; init; }
    public int TimeoutSeconds { get; init; } = 30;
    public int MaxRetries { get; init; } = 2;
    public int RetryDelayMilliseconds { get; init; } = 500;
    public int RequestsPerMinute { get; init; } = 30;
    public int DefaultPageSize { get; init; } = 50;
    public int MaxPageSize { get; init; } = 100;
}
