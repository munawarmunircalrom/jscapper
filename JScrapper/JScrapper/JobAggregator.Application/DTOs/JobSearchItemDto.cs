namespace JobAggregator.Application.DTOs;

public sealed class JobSearchItemDto
{
    public Guid JobId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Company { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Location { get; init; }
    public decimal? SalaryMin { get; init; }
    public decimal? SalaryMax { get; init; }
    public string? Currency { get; init; }
    public string? EmploymentType { get; init; }
    public string? Experience { get; init; }
    public string? WorkMode { get; init; }
    public DateTimeOffset? PostedAtUtc { get; init; }
    public IReadOnlyCollection<string> Skills { get; init; } = [];
    public IReadOnlyCollection<string> Sources { get; init; } = [];
}
