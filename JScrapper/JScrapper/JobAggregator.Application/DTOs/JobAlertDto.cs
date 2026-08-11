namespace JobAggregator.Application.DTOs;

public sealed class JobAlertDto
{
    public Guid AlertId { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Keywords { get; init; }
    public string? Location { get; init; }
    public IReadOnlyCollection<string> Skills { get; init; } = [];
    public decimal? MinSalary { get; init; }
    public decimal? MaxSalary { get; init; }
    public string? Experience { get; init; }
    public string? EmploymentType { get; init; }
    public bool? Remote { get; init; }
    public IReadOnlyCollection<string> Sources { get; init; } = [];
    public bool IsEnabled { get; init; }
    public DateTimeOffset? LastRunAtUtc { get; init; }
}
