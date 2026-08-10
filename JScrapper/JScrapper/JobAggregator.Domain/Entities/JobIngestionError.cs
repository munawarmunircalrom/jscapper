using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class JobIngestionError : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid JobIngestionRunId { get; set; }
    public Guid? JobSourcePostingId { get; set; }
    public string ErrorCode { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public DateTimeOffset OccurredAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public JobIngestionRun JobIngestionRun { get; set; } = null!;
    public JobSourcePosting? JobSourcePosting { get; set; }
}
