namespace JobAggregator.Application.Abstractions.Providers;

public sealed record JobSearchRequest(
    string ProviderName,
    string? Keywords,
    string? Location,
    int PageNumber = 1,
    int PageSize = 50,
    int MaxPages = 1,
    string? Cursor = null,
    TimeSpan? Timeout = null,
    string? CorrelationId = null,
    string? IdempotencyScope = null);
