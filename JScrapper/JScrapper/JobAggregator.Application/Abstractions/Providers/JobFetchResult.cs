namespace JobAggregator.Application.Abstractions.Providers;

public sealed record JobFetchResult(
    string ProviderName,
    int PageNumber,
    int PageSize,
    IReadOnlyCollection<RawJob> Jobs,
    bool HasMore,
    string? NextCursor,
    int Attempts,
    TimeSpan Duration,
    JobProviderHealth Health);
