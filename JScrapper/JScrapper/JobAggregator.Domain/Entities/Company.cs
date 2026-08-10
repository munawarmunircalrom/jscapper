using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class Company : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? WebsiteUrl { get; set; }
    public string? Description { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAtUtc { get; set; }

    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
