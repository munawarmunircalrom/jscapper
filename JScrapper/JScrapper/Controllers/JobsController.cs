using Asp.Versioning;
using JobAggregator.Application.Features.Jobs.Queries;
using JobAggregator.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobAggregator.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("jobs")]
public sealed class JobsController(ISender sender) : ControllerBase
{
    [HttpGet("search")]
    public async Task<ActionResult<JobSearchResult>> Search(
        [FromQuery] string? keyword,
        [FromQuery] string? title,
        [FromQuery] string? company,
        [FromQuery] string? location,
        [FromQuery] decimal? minSalary,
        [FromQuery] decimal? maxSalary,
        [FromQuery] string? experience,
        [FromQuery] string? employmentType,
        [FromQuery] string? skills,
        [FromQuery] bool? remote,
        [FromQuery] bool? hybrid,
        [FromQuery] string? source,
        [FromQuery] DateTimeOffset? postedFrom,
        [FromQuery] DateTimeOffset? postedTo,
        [FromQuery] string sortBy = "postedDate",
        [FromQuery] string sortDirection = "desc",
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var skillList = string.IsNullOrWhiteSpace(skills)
            ? null
            : skills.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var query = new SearchJobsQuery(
            keyword,
            title,
            company,
            location,
            minSalary,
            maxSalary,
            experience,
            employmentType,
            skillList,
            remote,
            hybrid,
            source,
            postedFrom,
            postedTo,
            sortBy,
            sortDirection,
            pageNumber,
            pageSize);

        var result = await sender.Send(query, cancellationToken);
        return Ok(result);
    }
}
