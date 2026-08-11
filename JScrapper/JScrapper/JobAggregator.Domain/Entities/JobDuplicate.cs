using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class JobDuplicate : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid CanonicalJobId { get; set; }
    public Guid? DuplicateJobSourcePostingId { get; set; }
    public string DuplicateExternalJobId { get; set; } = string.Empty;
    public string DuplicateReason { get; set; } = string.Empty;
    public decimal MatchConfidence { get; set; }

    public Job CanonicalJob { get; set; } = null!;
    public JobSourcePosting? DuplicateJobSourcePosting { get; set; }
}
