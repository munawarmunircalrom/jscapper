using JobAggregator.Domain.Common;

namespace JobAggregator.Domain.Entities;

public sealed class Job : AuditableEntity
{
    public Guid Id { get; set; }
    public string CanonicalHash { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? EmploymentType { get; set; }
    public string? WorkMode { get; set; }
    public string? Seniority { get; set; }
    public DateTimeOffset? PostedAtUtc { get; set; }
    public DateTimeOffset? ExpiresAtUtc { get; set; }
    public string SearchText { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAtUtc { get; set; }

    public Guid CompanyId { get; set; }
    public Guid JobLocationId { get; set; }
    public Guid? JobSalaryId { get; set; }

    public Company Company { get; set; } = null!;
    public JobLocation JobLocation { get; set; } = null!;
    public JobSalary? JobSalary { get; set; }
    public ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
    public ICollection<JobSourcePosting> JobSourcePostings { get; set; } = new List<JobSourcePosting>();
    public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    public ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
