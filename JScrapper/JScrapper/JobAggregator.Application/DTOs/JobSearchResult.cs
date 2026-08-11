namespace JobAggregator.Application.DTOs;

public sealed class JobSearchResult
{
    public required IReadOnlyCollection<JobSearchItemDto> Items { get; init; }
    public required int TotalCount { get; init; }
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required string SortBy { get; init; }
    public required string SortDirection { get; init; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / Math.Max(1, PageSize));
}
