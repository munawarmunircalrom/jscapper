using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class SavedJob : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid JobId { get; set; }

    public User User { get; set; } = null!;
    public Job Job { get; set; } = null!;
}
