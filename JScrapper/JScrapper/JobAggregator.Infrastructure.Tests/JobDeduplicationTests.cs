using JobAggregator.Application.Abstractions.Providers;
using JobAggregator.Infrastructure.Ingestion;

namespace JobAggregator.Infrastructure.Tests;

public sealed class JobDeduplicationTests
{
    [Fact]
    public void Evaluate_ShouldReturnExactDuplicate_WhenProviderAndExternalIdMatch()
    {
        var existing = BuildRawJob(provider: "LinkedIn", externalId: "abc-1");
        var candidate = BuildRawJob(provider: "LinkedIn", externalId: "abc-1");

        var result = JobDeduplication.Evaluate(candidate, existing);

        Assert.True(result.IsDuplicate);
        Assert.Equal(1.0, result.Confidence);
        Assert.Equal("provider_external_id", result.Reason);
    }

    [Fact]
    public void Evaluate_ShouldReturnNearDuplicate_WithHighConfidence()
    {
        var existing = BuildRawJob(
            provider: "LinkedIn",
            externalId: "li-100",
            title: "Senior .NET Developer",
            company: "Acme Inc",
            location: "Lahore, Pakistan",
            description: "Design and build ASP.NET Core APIs with SQL Server and Azure.",
            salaryMin: 150000,
            salaryMax: 250000,
            currency: "PKR",
            postedAtUtc: DateTimeOffset.UtcNow.AddDays(-1));

        var candidate = BuildRawJob(
            provider: "Indeed",
            externalId: "ind-200",
            title: "Senior .NET Developer",
            company: "Acme Inc",
            location: "Lahore, Pakistan",
            description: "Build ASP.NET Core APIs and SQL Server services on Azure platform.",
            salaryMin: 155000,
            salaryMax: 245000,
            currency: "PKR",
            postedAtUtc: DateTimeOffset.UtcNow);

        var result = JobDeduplication.Evaluate(candidate, existing);

        Assert.True(result.IsDuplicate);
        Assert.True(result.Confidence >= 0.80);
        Assert.Contains("company_title_location", result.Reason, StringComparison.OrdinalIgnoreCase);
    }

    private static RawJob BuildRawJob(
        string provider,
        string externalId,
        string title = "Software Engineer",
        string company = "Acme",
        string? description = "desc",
        string? location = "Karachi, Pakistan",
        decimal? salaryMin = null,
        decimal? salaryMax = null,
        string? currency = "USD",
        DateTimeOffset? postedAtUtc = null)
    {
        return new RawJob(
            ProviderName: provider,
            ExternalJobId: externalId,
            Title: title,
            Company: company,
            Description: description,
            Location: location,
            SalaryMin: salaryMin,
            SalaryMax: salaryMax,
            Currency: currency,
            EmploymentType: "Full-time",
            Experience: "Senior",
            Skills: [".NET", "SQL"],
            PostedAtUtc: postedAtUtc,
            SourceUrl: null,
            CanonicalUrl: null,
            ContentHash: null,
            DeduplicationConfidence: null,
            IdempotencyKey: $"{provider}:{externalId}");
    }
}
