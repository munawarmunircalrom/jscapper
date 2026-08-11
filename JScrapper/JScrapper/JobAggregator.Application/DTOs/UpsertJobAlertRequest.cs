namespace JobAggregator.Application.DTOs;

public sealed class UpsertJobAlertRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Keywords { get; init; }
    public string? Location { get; init; }
    public IReadOnlyCollection<string>? Skills { get; init; }
    public decimal? MinSalary { get; init; }
    public decimal? MaxSalary { get; init; }
    public string? Experience { get; init; }
    public string? EmploymentType { get; init; }
    public bool? Remote { get; init; }
    public IReadOnlyCollection<string>? Sources { get; init; }
    public int FrequencyMinutes { get; init; } = 60;
}
