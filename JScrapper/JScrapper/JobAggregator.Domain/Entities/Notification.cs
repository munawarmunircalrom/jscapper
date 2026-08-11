using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class Notification : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? JobId { get; set; }
    public Guid? AlertId { get; set; }
    public DateTimeOffset? SentAtUtc { get; set; }
    public string Status { get; set; } = "Pending";
    public string Channel { get; set; } = "InApp";
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTimeOffset? ReadAtUtc { get; set; }
    public string? RelatedEntityType { get; set; }
    public string? RelatedEntityId { get; set; }

    public User User { get; set; } = null!;
    public Job? Job { get; set; }
    public JobAlert? Alert { get; set; }
}
