using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class JobApplication : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid JobId { get; set; }
    public DateTimeOffset AppliedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public string Status { get; set; } = "Applied";
    public string? ExternalApplicationId { get; set; }
    public string? Notes { get; set; }

    public User User { get; set; } = null!;
    public Job Job { get; set; } = null!;
}
