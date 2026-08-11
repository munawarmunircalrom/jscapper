namespace JobAggregator.Application.DTOs;

public sealed class NotificationHistoryItemDto
{
    public Guid NotificationId { get; init; }
    public Guid UserId { get; init; }
    public Guid? JobId { get; init; }
    public Guid? AlertId { get; init; }
    public DateTimeOffset? SentAtUtc { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Channel { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public bool IsRead { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
}
