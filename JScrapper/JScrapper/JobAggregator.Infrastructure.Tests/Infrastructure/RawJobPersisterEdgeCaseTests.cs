using JobAggregator.Application.Abstractions.Providers;
using JobAggregator.Domain.Entities;
using JobAggregator.Infrastructure.Ingestion;
using JobAggregator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace JobAggregator.Infrastructure.Tests.Infrastructure;

public sealed class RawJobPersisterEdgeCaseTests
{
    [Fact]
    public async Task PersistAsync_ShouldTreatDifferentCompanyNameAsDuplicate_WhenCanonicalUrlMatches()
    {
        await using var dbContext = CreateDbContext();

        var sourceLinkedIn = new JobSource { Id = Guid.NewGuid(), Name = "LinkedIn", IsActive = true };
        var sourceIndeed = new JobSource { Id = Guid.NewGuid(), Name = "Indeed", IsActive = true };
        var company = new Company { Id = Guid.NewGuid(), Name = "Acme", IsDeleted = false };
        var location = new JobLocation { Id = Guid.NewGuid(), Country = "Pakistan", City = "Lahore", RawText = "Lahore" };

        var canonicalJob = new Job
        {
            Id = Guid.NewGuid(),
            CanonicalHash = "hash-x",
            Title = "Platform Engineer",
            CompanyId = company.Id,
            JobLocationId = location.Id,
            SearchText = "Platform Engineer Acme Lahore",
            IsDeleted = false
        };

        dbContext.JobSources.AddRange(sourceLinkedIn, sourceIndeed);
        dbContext.Companies.Add(company);
        dbContext.JobLocations.Add(location);
        dbContext.Jobs.Add(canonicalJob);
        dbContext.JobSourcePostings.Add(new JobSourcePosting
        {
            Id = Guid.NewGuid(),
            JobId = canonicalJob.Id,
            JobSourceId = sourceLinkedIn.Id,
            ExternalJobId = "li-1",
            SourceUrl = "https://example.com/jobs/1",
            IsActive = true,
            RawPayloadHash = "a"
        });
        await dbContext.SaveChangesAsync();

        var persister = new RawJobPersister(dbContext, NullLogger<RawJobPersister>.Instance);

        var incoming = new RawJob(
            ProviderName: "Indeed",
            ExternalJobId: "in-2",
            Title: "Platform Engineer",
            Company: "Acme Technologies",
            Description: "Same role",
            Location: "Lahore",
            SalaryMin: null,
            SalaryMax: null,
            Currency: null,
            EmploymentType: "Full-time",
            Experience: "Mid",
            Skills: ["C#"],
            PostedAtUtc: DateTimeOffset.UtcNow,
            SourceUrl: new Uri("https://example.com/jobs/1?utm=indeed"),
            CanonicalUrl: "https://example.com/jobs/1",
            ContentHash: "b",
            DeduplicationConfidence: null,
            IdempotencyKey: "Indeed:in-2");

        await persister.PersistAsync([incoming], CancellationToken.None);

        Assert.Equal(1, await dbContext.Jobs.CountAsync());
        Assert.Equal(2, await dbContext.JobSourcePostings.CountAsync());
        Assert.Equal(1, await dbContext.JobDuplicates.CountAsync());
    }

    private static JobAggregatorDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<JobAggregatorDbContext>()
            .UseInMemoryDatabase($"persister-edge-tests-{Guid.NewGuid():N}")
            .Options;

        return new JobAggregatorDbContext(options);
    }
}
