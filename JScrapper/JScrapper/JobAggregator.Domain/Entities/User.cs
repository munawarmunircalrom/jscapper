using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class User : AuditableEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public UserPreference? Preference { get; set; }
    public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    public ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
    public ICollection<JobAlert> JobAlerts { get; set; } = new List<JobAlert>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
