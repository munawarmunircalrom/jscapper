namespace JobAggregator.Domain.Common;

public abstract class AuditableEntity
{
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public byte[] RowVersion { get; set; } = [];
}
