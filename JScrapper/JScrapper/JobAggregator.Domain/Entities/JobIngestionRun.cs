using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class JobIngestionRun : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid? JobSourceId { get; set; }
    public DateTimeOffset StartedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public string Status { get; set; } = "Running";
    public int TotalFetched { get; set; }
    public int InsertedCount { get; set; }
    public int UpdatedCount { get; set; }
    public int DuplicateCount { get; set; }
    public int ErrorCount { get; set; }

    public JobSource? JobSource { get; set; }
    public ICollection<JobIngestionError> Errors { get; set; } = new List<JobIngestionError>();
}
