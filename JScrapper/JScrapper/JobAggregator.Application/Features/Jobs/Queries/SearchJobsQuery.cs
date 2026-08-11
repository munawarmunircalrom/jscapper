using JobAggregator.Application.DTOs;
using MediatR;

namespace JobAggregator.Application.Features.Jobs.Queries;

public sealed record SearchJobsQuery(
    string? Keyword,
    string? Title,
    string? Company,
    string? Location,
    decimal? MinSalary,
    decimal? MaxSalary,
    string? Experience,
    string? EmploymentType,
    IReadOnlyCollection<string>? Skills,
    bool? Remote,
    bool? Hybrid,
    string? Source,
    DateTimeOffset? PostedFrom,
    DateTimeOffset? PostedTo,
    string SortBy = "postedDate",
    string SortDirection = "desc",
    int PageNumber = 1,
    int PageSize = 20) : IRequest<JobSearchResult>;
