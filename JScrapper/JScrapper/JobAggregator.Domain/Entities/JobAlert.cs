using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class JobAlert : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Query { get; set; } = string.Empty;
    public string? Keywords { get; set; }
    public string? Location { get; set; }
    public string? SkillsCsv { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string? Experience { get; set; }
    public string? EmploymentType { get; set; }
    public bool? Remote { get; set; }
    public string? SourcesCsv { get; set; }
    public int FrequencyMinutes { get; set; } = 60;
    public bool IsEnabled { get; set; } = true;
    public DateTimeOffset? LastRunAtUtc { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
