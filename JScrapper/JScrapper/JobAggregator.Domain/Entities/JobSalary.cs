using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class JobSalary : AuditableEntity
{
    public Guid Id { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public string Currency { get; set; } = "USD";
    public string? Period { get; set; }

    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
