using JobAggregator.Application.DTOs;
using JobAggregator.Application.Features.Jobs.Queries;

namespace JobAggregator.Application.Abstractions.Search;

public interface IJobSearchService
{
    Task<JobSearchResult> SearchAsync(SearchJobsQuery query, CancellationToken cancellationToken);
}
