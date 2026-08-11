namespace JobAggregator.Application.Abstractions.Providers;

public sealed record JobProviderHealth(
    string ProviderName,
    bool IsHealthy,
    DateTimeOffset CheckedAtUtc,
    DateTimeOffset? LastSuccessAtUtc,
    int ConsecutiveFailures,
    string? LastError,
    double? AverageLatencyMs);
