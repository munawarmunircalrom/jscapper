using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class JobSkill : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsRequired { get; set; }

    public Job Job { get; set; } = null!;
}
