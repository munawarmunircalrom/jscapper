using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class JobSourcePosting : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public Guid JobSourceId { get; set; }
    public string ExternalJobId { get; set; } = string.Empty;
    public string? SourceUrl { get; set; }
    public string? RawPayloadHash { get; set; }
    public DateTimeOffset FirstSeenAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastSeenAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public bool IsActive { get; set; } = true;

    public Job Job { get; set; } = null!;
    public JobSource JobSource { get; set; } = null!;
}
