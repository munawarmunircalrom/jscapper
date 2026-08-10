using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class JobLocation : AuditableEntity
{
    public Guid Id { get; set; }
    public string Country { get; set; } = string.Empty;
    public string? State { get; set; }
    public string? City { get; set; }
    public string? RawText { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
