using JobAggregator.Application.Abstractions.Search;
using JobAggregator.Application.DTOs;
using MediatR;

namespace JobAggregator.Application.Features.Jobs.Queries;

public sealed class SearchJobsQueryHandler(IJobSearchService jobSearchService)
    : IRequestHandler<SearchJobsQuery, JobSearchResult>
{
    public Task<JobSearchResult> Handle(SearchJobsQuery request, CancellationToken cancellationToken)
    {
        return jobSearchService.SearchAsync(request, cancellationToken);
    }
}
