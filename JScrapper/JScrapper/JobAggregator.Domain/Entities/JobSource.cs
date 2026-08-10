using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class JobSource : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? BaseUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<JobSourcePosting> JobSourcePostings { get; set; } = new List<JobSourcePosting>();
    public ICollection<JobIngestionRun> IngestionRuns { get; set; } = new List<JobIngestionRun>();
}
