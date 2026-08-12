using JobAggregator.Api.Controllers;
using JobAggregator.Application.Configuration;
using JobAggregator.Application.Features.Jobs.Queries;
using JobAggregator.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace JobAggregator.Infrastructure.Tests.Api;

public sealed class JobsControllerDefaultsTests
{
    [Fact]
    public async Task Search_ShouldPassKeywordSoftwareEngieer_WithLinkedInSource()
    {
        SearchJobsQuery? captured = null;

        var sender = new Mock<ISender>();
        sender
            .Setup(x => x.Send(It.IsAny<SearchJobsQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<JobSearchResult>, CancellationToken>((request, _) => captured = (SearchJobsQuery)request)
            .ReturnsAsync(new JobSearchResult
            {
                Items = [],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 20,
                SortBy = "postedDate",
                SortDirection = "desc"
            });

        var options = Options.Create(new SearchPlatformOptions { DefaultProvider = "LinkedIn" });
        var controller = new JobsController(sender.Object, options);

        _ = await controller.Search(
            keyword: "software engieer",
            title: null,
            company: null,
            location: null,
            minSalary: null,
            maxSalary: null,
            experience: null,
            employmentType: null,
            skills: null,
            remote: null,
            hybrid: null,
            source: "LinkedIn",
            postedFrom: null,
            postedTo: null,
            cancellationToken: CancellationToken.None);

        Assert.NotNull(captured);
        Assert.Equal("software engieer", captured!.Keyword);
        Assert.Equal("LinkedIn", captured.Source);
    }

    [Fact]
    public async Task Search_ShouldDefaultSourceToLinkedIn_WhenSourceNotProvided()
    {
        SearchJobsQuery? captured = null;

        var sender = new Mock<ISender>();
        sender
            .Setup(x => x.Send(It.IsAny<SearchJobsQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<JobSearchResult>, CancellationToken>((request, _) => captured = (SearchJobsQuery)request)
            .ReturnsAsync(new JobSearchResult
            {
                Items = [],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 20,
                SortBy = "postedDate",
                SortDirection = "desc"
            });

        var options = Options.Create(new SearchPlatformOptions { DefaultProvider = "LinkedIn" });
        var controller = new JobsController(sender.Object, options);

        var action = await controller.Search(
            keyword: null,
            title: null,
            company: null,
            location: null,
            minSalary: null,
            maxSalary: null,
            experience: null,
            employmentType: null,
            skills: null,
            remote: null,
            hybrid: null,
            source: null,
            postedFrom: null,
            postedTo: null,
            cancellationToken: CancellationToken.None);

        Assert.IsType<OkObjectResult>(action.Result);
        Assert.NotNull(captured);
        Assert.Equal("LinkedIn", captured!.Source);
    }

    [Fact]
    public async Task Search_ShouldUseRequestedSource_WhenProvided()
    {
        SearchJobsQuery? captured = null;

        var sender = new Mock<ISender>();
        sender
            .Setup(x => x.Send(It.IsAny<SearchJobsQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<JobSearchResult>, CancellationToken>((request, _) => captured = (SearchJobsQuery)request)
            .ReturnsAsync(new JobSearchResult
            {
                Items = [],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 20,
                SortBy = "postedDate",
                SortDirection = "desc"
            });

        var options = Options.Create(new SearchPlatformOptions { DefaultProvider = "LinkedIn" });
        var controller = new JobsController(sender.Object, options);

        _ = await controller.Search(
            keyword: null,
            title: null,
            company: null,
            location: null,
            minSalary: null,
            maxSalary: null,
            experience: null,
            employmentType: null,
            skills: null,
            remote: null,
            hybrid: null,
            source: "Indeed",
            postedFrom: null,
            postedTo: null,
            cancellationToken: CancellationToken.None);

        Assert.NotNull(captured);
        Assert.Equal("Indeed", captured!.Source);
    }
}
