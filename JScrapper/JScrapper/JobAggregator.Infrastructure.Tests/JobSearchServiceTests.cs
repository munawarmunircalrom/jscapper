using JobAggregator.Application.Features.Jobs.Queries;
using JobAggregator.Domain.Entities;
using JobAggregator.Infrastructure.Persistence;
using JobAggregator.Infrastructure.Search;
using Microsoft.EntityFrameworkCore;

namespace JobAggregator.Infrastructure.Tests;

public sealed class JobSearchServiceTests
{
    [Fact]
    public async Task SearchAsync_ShouldFilterByKeywordSourceWorkModeAndPaginate()
    {
        await using var dbContext = CreateDbContext();
        await SeedAsync(dbContext);

        var service = new JobSearchService(dbContext);

        var query = new SearchJobsQuery(
            Keyword: "ASP.NET",
            Title: null,
            Company: "Acme",
            Location: "Lahore",
            MinSalary: 150000,
            MaxSalary: null,
            Experience: "Senior",
            EmploymentType: "Full-time",
            Skills: ["SQL Server"],
            Remote: true,
            Hybrid: null,
            Source: "LinkedIn",
            PostedFrom: DateTimeOffset.UtcNow.AddDays(-10),
            PostedTo: DateTimeOffset.UtcNow,
            SortBy: "postedDate",
            SortDirection: "desc",
            PageNumber: 1,
            PageSize: 10);

        var result = await service.SearchAsync(query, CancellationToken.None);

        Assert.Single(result.Items);
        Assert.Equal("Senior .NET Developer", result.Items.Single().Title);
        Assert.Contains("LinkedIn", result.Items.Single().Sources);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task SearchAsync_ShouldSortBySalaryAsc_AndApplyPaging()
    {
        await using var dbContext = CreateDbContext();
        await SeedAsync(dbContext);

        var service = new JobSearchService(dbContext);

        var query = new SearchJobsQuery(
            Keyword: null,
            Title: null,
            Company: null,
            Location: null,
            MinSalary: null,
            MaxSalary: null,
            Experience: null,
            EmploymentType: null,
            Skills: null,
            Remote: null,
            Hybrid: null,
            Source: null,
            PostedFrom: null,
            PostedTo: null,
            SortBy: "salary",
            SortDirection: "asc",
            PageNumber: 1,
            PageSize: 1);

        var result = await service.SearchAsync(query, CancellationToken.None);

        Assert.Equal(2, result.TotalCount);
        Assert.Single(result.Items);
        Assert.Equal("Mid .NET Developer", result.Items.Single().Title);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(1, result.PageSize);
    }

    [Fact]
    public async Task SearchAsync_ShouldIncludeJobsWithMissingSalaryAndLocation_WhenNoSuchFiltersProvided()
    {
        await using var dbContext = CreateDbContext();
        await SeedAsync(dbContext);

        var company = await dbContext.Companies.FirstAsync();
        var fallbackLocation = await dbContext.JobLocations.FirstAsync();

        var job = new Job
        {
            Id = Guid.NewGuid(),
            CanonicalHash = "hash-job-3",
            Title = "Junior Developer",
            Description = "Entry role",
            EmploymentType = "Full-time",
            WorkMode = null,
            Seniority = "Junior",
            PostedAtUtc = DateTimeOffset.UtcNow,
            SearchText = "Junior Developer Acme",
            IsDeleted = false,
            CompanyId = company.Id,
            JobLocationId = fallbackLocation.Id,
            JobSalaryId = null
        };

        dbContext.Jobs.Add(job);
        await dbContext.SaveChangesAsync();

        var service = new JobSearchService(dbContext);

        var query = new SearchJobsQuery(
            Keyword: null,
            Title: null,
            Company: null,
            Location: null,
            MinSalary: null,
            MaxSalary: null,
            Experience: null,
            EmploymentType: null,
            Skills: null,
            Remote: null,
            Hybrid: null,
            Source: null,
            PostedFrom: null,
            PostedTo: null,
            SortBy: "postedDate",
            SortDirection: "desc",
            PageNumber: 1,
            PageSize: 20);

        var result = await service.SearchAsync(query, CancellationToken.None);

        Assert.Contains(result.Items, x => x.Title == "Junior Developer" && x.SalaryMin is null && x.SalaryMax is null);
    }

    private static JobAggregatorDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<JobAggregatorDbContext>()
            .UseInMemoryDatabase($"job-search-tests-{Guid.NewGuid():N}")
            .Options;

        return new JobAggregatorDbContext(options);
    }

    private static async Task SeedAsync(JobAggregatorDbContext dbContext)
    {
        var company = new Company { Id = Guid.NewGuid(), Name = "Acme", IsDeleted = false };
        var locationLhr = new JobLocation { Id = Guid.NewGuid(), City = "Lahore", Country = "Pakistan", RawText = "Lahore, Pakistan" };
        var locationKar = new JobLocation { Id = Guid.NewGuid(), City = "Karachi", Country = "Pakistan", RawText = "Karachi, Pakistan" };
        var salaryHigh = new JobSalary { Id = Guid.NewGuid(), MinAmount = 180000, MaxAmount = 260000, Currency = "PKR", Period = "year" };
        var salaryLow = new JobSalary { Id = Guid.NewGuid(), MinAmount = 90000, MaxAmount = 130000, Currency = "PKR", Period = "year" };

        var sourceLinkedIn = new JobSource { Id = Guid.NewGuid(), Name = "LinkedIn", IsActive = true };
        var sourceIndeed = new JobSource { Id = Guid.NewGuid(), Name = "Indeed", IsActive = true };

        var job1 = new Job
        {
            Id = Guid.NewGuid(),
            CanonicalHash = "hash-job-1",
            Title = "Senior .NET Developer",
            Description = "Build ASP.NET APIs and SQL Server services",
            EmploymentType = "Full-time",
            WorkMode = "Remote",
            Seniority = "Senior",
            PostedAtUtc = DateTimeOffset.UtcNow.AddDays(-2),
            SearchText = "Senior .NET Developer Acme ASP.NET SQL Server Lahore",
            IsDeleted = false,
            CompanyId = company.Id,
            JobLocationId = locationLhr.Id,
            JobSalaryId = salaryHigh.Id
        };

        job1.JobSkills.Add(new JobSkill { Id = Guid.NewGuid(), Name = "SQL Server", IsRequired = true });
        job1.JobSkills.Add(new JobSkill { Id = Guid.NewGuid(), Name = "ASP.NET", IsRequired = true });

        var job2 = new Job
        {
            Id = Guid.NewGuid(),
            CanonicalHash = "hash-job-2",
            Title = "Mid .NET Developer",
            Description = "Maintain enterprise applications",
            EmploymentType = "Full-time",
            WorkMode = "Hybrid",
            Seniority = "Mid",
            PostedAtUtc = DateTimeOffset.UtcNow.AddDays(-1),
            SearchText = "Mid .NET Developer Acme Karachi",
            IsDeleted = false,
            CompanyId = company.Id,
            JobLocationId = locationKar.Id,
            JobSalaryId = salaryLow.Id
        };

        job2.JobSkills.Add(new JobSkill { Id = Guid.NewGuid(), Name = "C#", IsRequired = true });

        dbContext.Companies.Add(company);
        dbContext.JobLocations.AddRange(locationLhr, locationKar);
        dbContext.JobSalaries.AddRange(salaryHigh, salaryLow);
        dbContext.JobSources.AddRange(sourceLinkedIn, sourceIndeed);
        dbContext.Jobs.AddRange(job1, job2);

        dbContext.JobSourcePostings.AddRange(
            new JobSourcePosting
            {
                Id = Guid.NewGuid(),
                JobId = job1.Id,
                JobSourceId = sourceLinkedIn.Id,
                ExternalJobId = "li-1",
                SourceUrl = "https://linkedin.com/jobs/1",
                RawPayloadHash = "hp-1",
                IsActive = true,
                FirstSeenAtUtc = DateTimeOffset.UtcNow.AddDays(-2),
                LastSeenAtUtc = DateTimeOffset.UtcNow
            },
            new JobSourcePosting
            {
                Id = Guid.NewGuid(),
                JobId = job2.Id,
                JobSourceId = sourceIndeed.Id,
                ExternalJobId = "in-2",
                SourceUrl = "https://indeed.com/jobs/2",
                RawPayloadHash = "hp-2",
                IsActive = true,
                FirstSeenAtUtc = DateTimeOffset.UtcNow.AddDays(-1),
                LastSeenAtUtc = DateTimeOffset.UtcNow
            });

        await dbContext.SaveChangesAsync();
    }
}
