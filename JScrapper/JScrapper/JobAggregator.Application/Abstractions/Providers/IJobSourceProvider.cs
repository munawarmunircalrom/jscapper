namespace JobAggregator.Application.Abstractions.Providers;

public interface IJobSourceProvider
{
    string Name { get; }
    JobProviderConfiguration Configuration { get; }
    Task<JobProviderHealth> CheckHealthAsync(CancellationToken cancellationToken);
    Task<JobFetchResult> FetchJobsAsync(JobSearchRequest request, CancellationToken cancellationToken);
}
