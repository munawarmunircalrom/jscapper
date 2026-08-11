using JobAggregator.Application.Abstractions.Providers;
using JobAggregator.Domain.Entities;
using JobAggregator.Infrastructure.Ingestion;
using JobAggregator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace JobAggregator.Infrastructure.Tests.Alerts;

public sealed class RawJobAlertDispatcherTests
{
    [Fact]
    public async Task DispatchAsync_ShouldCreateSingleNotificationAndPreventDuplicates()
    {
        await using var dbContext = CreateDbContext();

        var userId = Guid.NewGuid();
        var company = new Company { Id = Guid.NewGuid(), Name = "Acme", IsDeleted = false };
        var location = new JobLocation { Id = Guid.NewGuid(), Country = "Pakistan", City = "Lahore", RawText = "Lahore, Pakistan" };
        var salary = new JobSalary { Id = Guid.NewGuid(), MinAmount = 100000, MaxAmount = 200000, Currency = "PKR" };
        var source = new JobSource { Id = Guid.NewGuid(), Name = "LinkedIn", IsActive = true };

        var job = new Job
        {
            Id = Guid.NewGuid(),
            CanonicalHash = "hash-1",
            Title = "Senior .NET Engineer",
            Description = "Build APIs",
            CompanyId = company.Id,
            JobLocationId = location.Id,
            JobSalaryId = salary.Id,
            EmploymentType = "Full-time",
            Seniority = "Senior",
            WorkMode = "Remote",
            SearchText = "Senior .NET Engineer Acme Lahore",
            PostedAtUtc = DateTimeOffset.UtcNow,
            IsDeleted = false
        };

        job.JobSkills.Add(new JobSkill { Id = Guid.NewGuid(), Name = "C#", IsRequired = true });

        var posting = new JobSourcePosting
        {
            Id = Guid.NewGuid(),
            JobId = job.Id,
            JobSourceId = source.Id,
            ExternalJobId = "li-1",
            SourceUrl = "https://linkedin.com/jobs/view/1",
            IsActive = true,
            FirstSeenAtUtc = DateTimeOffset.UtcNow,
            LastSeenAtUtc = DateTimeOffset.UtcNow
        };

        var alert = new JobAlert
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = "Remote Senior",
            Query = "",
            Keywords = ".NET",
            Location = "Lahore",
            SkillsCsv = "C#",
            MinSalary = 90000,
            Experience = "Senior",
            EmploymentType = "Full",
            Remote = true,
            SourcesCsv = "LinkedIn",
            IsEnabled = true
        };

        dbContext.Users.Add(new User { Id = userId, Email = "a@b.com", DisplayName = "A", IsActive = true });
        dbContext.Companies.Add(company);
        dbContext.JobLocations.Add(location);
        dbContext.JobSalaries.Add(salary);
        dbContext.JobSources.Add(source);
        dbContext.Jobs.Add(job);
        dbContext.JobSourcePostings.Add(posting);
        dbContext.JobAlerts.Add(alert);
        await dbContext.SaveChangesAsync();

        var dispatcher = new RawJobAlertDispatcher(dbContext, NullLogger<RawJobAlertDispatcher>.Instance);

        var raw = new RawJob(
            ProviderName: "LinkedIn",
            ExternalJobId: "li-1",
            Title: job.Title,
            Company: company.Name,
            Description: job.Description,
            Location: location.RawText,
            SalaryMin: 100000,
            SalaryMax: 200000,
            Currency: "PKR",
            EmploymentType: "Full-time",
            Experience: "Senior",
            Skills: ["C#"],
            PostedAtUtc: DateTimeOffset.UtcNow,
            SourceUrl: new Uri("https://linkedin.com/jobs/view/1"),
            CanonicalUrl: null,
            ContentHash: null,
            DeduplicationConfidence: null,
            IdempotencyKey: "linkedin:li-1");

        await dispatcher.DispatchAsync([raw], CancellationToken.None);
        await dispatcher.DispatchAsync([raw], CancellationToken.None);

        var notifications = await dbContext.Notifications.ToArrayAsync();
        Assert.Single(notifications);
        Assert.Equal(userId, notifications[0].UserId);
        Assert.Equal(job.Id, notifications[0].JobId);
        Assert.Equal(alert.Id, notifications[0].AlertId);
    }

    private static JobAggregatorDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<JobAggregatorDbContext>()
            .UseInMemoryDatabase($"alerts-dispatcher-tests-{Guid.NewGuid():N}")
            .Options;

        return new JobAggregatorDbContext(options);
    }
}
