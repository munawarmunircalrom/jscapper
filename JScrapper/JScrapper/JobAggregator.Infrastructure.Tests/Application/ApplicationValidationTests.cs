using JobAggregator.Application.Features.Alerts;
using JobAggregator.Application.Features.Jobs.Queries;
using JobAggregator.Application.DTOs;

namespace JobAggregator.Infrastructure.Tests.Application;

public sealed class ApplicationValidationTests
{
    [Fact]
    public void SearchJobsQueryValidator_ShouldRejectInvalidSalaryRange()
    {
        var validator = new SearchJobsQueryValidator();

        var result = validator.Validate(new SearchJobsQuery(
            Keyword: null,
            Title: null,
            Company: null,
            Location: null,
            MinSalary: 1000,
            MaxSalary: 100,
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
            PageSize: 20));

        Assert.False(result.IsValid);
    }

    [Fact]
    public void UpsertJobAlertRequestValidator_ShouldAcceptMissingSalaryAndLocation()
    {
        var validator = new UpsertJobAlertRequestValidator();

        var result = validator.Validate(new UpsertJobAlertRequest
        {
            Name = "No salary/location",
            Keywords = "dotnet",
            Location = null,
            MinSalary = null,
            MaxSalary = null,
            Skills = ["C#"],
            Sources = ["LinkedIn"],
            FrequencyMinutes = 30
        });

        Assert.True(result.IsValid);
    }
}
