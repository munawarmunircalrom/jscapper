using JobAggregator.Application.Abstractions.Providers;
using JobAggregator.Domain.Entities;
using JobAggregator.Infrastructure.Ingestion;
using JobAggregator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace JobAggregator.Infrastructure.Tests;

public sealed class RawJobPersisterTests
{
    [Fact]
    public async Task PersistAsync_ShouldKeepSingleCanonicalJob_AndPreserveMultipleSourcePostings_ForNearDuplicate()
    {
        await using var dbContext = CreateDbContext();

        var source = new JobSource { Id = Guid.NewGuid(), Name = "LinkedIn", IsActive = true };
        var company = new Company { Id = Guid.NewGuid(), Name = "Acme Inc", IsDeleted = false };
        var location = new JobLocation { Id = Guid.NewGuid(), City = "Lahore", Country = "Pakistan", RawText = "Lahore, Pakistan" };
        var salary = new JobSalary { Id = Guid.NewGuid(), MinAmount = 150000, MaxAmount = 250000, Currency = "PKR", Period = "year" };

        var canonicalJob = new Job
        {
            Id = Guid.NewGuid(),
            CanonicalHash = "hash-existing",
            Title = "Senior .NET Developer",
            CompanyId = company.Id,
            JobLocationId = location.Id,
            JobSalaryId = salary.Id,
            Description = "Design and build ASP.NET Core APIs with SQL Server and Azure.",
            EmploymentType = "Full-time",
            Seniority = "Senior",
            PostedAtUtc = DateTimeOffset.UtcNow.AddDays(-1),
            SearchText = "Senior .NET Developer Acme Inc Lahore",
            IsDeleted = false
        };

        var existingPosting = new JobSourcePosting
        {
            Id = Guid.NewGuid(),
            JobId = canonicalJob.Id,
            JobSourceId = source.Id,
            ExternalJobId = "li-100",
            SourceUrl = "https://linkedin.com/jobs/view/100",
            RawPayloadHash = "payload-100",
            FirstSeenAtUtc = DateTimeOffset.UtcNow.AddDays(-1),
            LastSeenAtUtc = DateTimeOffset.UtcNow.AddDays(-1),
            IsActive = true
        };

        dbContext.JobSources.Add(source);
        dbContext.Companies.Add(company);
        dbContext.JobLocations.Add(location);
        dbContext.JobSalaries.Add(salary);
        dbContext.Jobs.Add(canonicalJob);
        dbContext.JobSourcePostings.Add(existingPosting);
        await dbContext.SaveChangesAsync();

        var persister = new RawJobPersister(dbContext, NullLogger<RawJobPersister>.Instance);

        var incoming = new RawJob(
            ProviderName: "Indeed",
            ExternalJobId: "ind-200",
            Title: "Senior .NET Developer",
            Company: "Acme Inc",
            Description: "Build ASP.NET Core APIs and SQL Server services on Azure platform.",
            Location: "Lahore, Pakistan",
            SalaryMin: 155000,
            SalaryMax: 245000,
            Currency: "PKR",
            EmploymentType: "Full-time",
            Experience: "Senior",
            Skills: [".NET", "SQL Server", "Azure"],
            PostedAtUtc: DateTimeOffset.UtcNow,
            SourceUrl: new Uri("https://indeed.com/viewjob?jk=200"),
            CanonicalUrl: null,
            ContentHash: null,
            DeduplicationConfidence: null,
            IdempotencyKey: "indeed:ind-200");

        await persister.PersistAsync([incoming], CancellationToken.None);

        Assert.Equal(1, await dbContext.Jobs.CountAsync());
        Assert.Equal(2, await dbContext.JobSourcePostings.CountAsync());

        var duplicate = await dbContext.JobDuplicates.SingleAsync();
        Assert.Equal(canonicalJob.Id, duplicate.CanonicalJobId);
        Assert.True(duplicate.MatchConfidence >= 0.80m);
        Assert.Contains("company_title_location", duplicate.DuplicateReason, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PersistAsync_ShouldUpdateExistingPosting_WhenProviderExternalIdAlreadyExists()
    {
        await using var dbContext = CreateDbContext();

        var source = new JobSource { Id = Guid.NewGuid(), Name = "LinkedIn", IsActive = true };
        var company = new Company { Id = Guid.NewGuid(), Name = "Acme", IsDeleted = false };
        var location = new JobLocation { Id = Guid.NewGuid(), Country = "Pakistan", RawText = "Pakistan" };

        var canonicalJob = new Job
        {
            Id = Guid.NewGuid(),
            CanonicalHash = "hash-1",
            Title = "Backend Engineer",
            CompanyId = company.Id,
            JobLocationId = location.Id,
            SearchText = "Backend Engineer Acme",
            IsDeleted = false
        };

        var posting = new JobSourcePosting
        {
            Id = Guid.NewGuid(),
            JobId = canonicalJob.Id,
            JobSourceId = source.Id,
            ExternalJobId = "li-500",
            SourceUrl = "https://linkedin.com/jobs/view/500",
            RawPayloadHash = "oldhash",
            FirstSeenAtUtc = DateTimeOffset.UtcNow.AddDays(-2),
            LastSeenAtUtc = DateTimeOffset.UtcNow.AddDays(-2),
            IsActive = true
        };

        dbContext.JobSources.Add(source);
        dbContext.Companies.Add(company);
        dbContext.JobLocations.Add(location);
        dbContext.Jobs.Add(canonicalJob);
        dbContext.JobSourcePostings.Add(posting);
        await dbContext.SaveChangesAsync();

        var persister = new RawJobPersister(dbContext, NullLogger<RawJobPersister>.Instance);

        var samePosting = new RawJob(
            ProviderName: "LinkedIn",
            ExternalJobId: "li-500",
            Title: "Backend Engineer",
            Company: "Acme",
            Description: "Updated payload",
            Location: "Pakistan",
            SalaryMin: null,
            SalaryMax: null,
            Currency: null,
            EmploymentType: "Full-time",
            Experience: "Mid",
            Skills: ["C#"],
            PostedAtUtc: DateTimeOffset.UtcNow,
            SourceUrl: new Uri("https://linkedin.com/jobs/view/500"),
            CanonicalUrl: "https://linkedin.com/jobs/view/500",
            ContentHash: "newhash",
            DeduplicationConfidence: 1,
            IdempotencyKey: "linkedin:li-500");

        var previousLastSeen = posting.LastSeenAtUtc;

        await persister.PersistAsync([samePosting], CancellationToken.None);

        Assert.Equal(1, await dbContext.Jobs.CountAsync());
        Assert.Equal(1, await dbContext.JobSourcePostings.CountAsync());
        Assert.Equal(0, await dbContext.JobDuplicates.CountAsync());

        var refreshed = await dbContext.JobSourcePostings.SingleAsync();
        Assert.True(refreshed.LastSeenAtUtc >= previousLastSeen);
        Assert.Equal("newhash", refreshed.RawPayloadHash);
    }

    private static JobAggregatorDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<JobAggregatorDbContext>()
            .UseInMemoryDatabase($"job-agg-tests-{Guid.NewGuid():N}")
            .Options;

        return new JobAggregatorDbContext(options);
    }
}
