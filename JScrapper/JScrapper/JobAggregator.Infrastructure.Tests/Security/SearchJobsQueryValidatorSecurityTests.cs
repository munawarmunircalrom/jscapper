using JobAggregator.Application.Features.Jobs.Queries;

namespace JobAggregator.Infrastructure.Tests.Security;

public sealed class SearchJobsQueryValidatorSecurityTests
{
    private readonly SearchJobsQueryValidator validator = new();

    [Fact]
    public void Validate_ShouldFail_WhenKeywordTooLong()
    {
        var query = new SearchJobsQuery(
            Keyword: new string('a', 201),
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

        var result = validator.Validate(query);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Keyword");
    }

    [Fact]
    public void Validate_ShouldFail_WhenSalaryRangeInvalid()
    {
        var query = new SearchJobsQuery(
            Keyword: null,
            Title: null,
            Company: null,
            Location: null,
            MinSalary: 5000,
            MaxSalary: 1000,
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

        var result = validator.Validate(query);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("MinSalary", StringComparison.OrdinalIgnoreCase));
    }
}
