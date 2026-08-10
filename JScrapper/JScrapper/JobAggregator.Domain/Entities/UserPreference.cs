using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class UserPreference : AuditableEntity
{
    public Guid UserId { get; set; }
    public string PreferredKeywords { get; set; } = string.Empty;
    public string PreferredLocations { get; set; } = string.Empty;
    public decimal? MinSalary { get; set; }
    public string PreferredCurrency { get; set; } = "USD";
    public bool RemoteOnly { get; set; }

    public User User { get; set; } = null!;
}
