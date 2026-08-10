using JobAggregator.Domain.Entities;

namespace JobAggregator.Application.Abstractions.Persistence;

public interface IJobSourcePostingRepository
{
    Task<JobSourcePosting?> GetBySourceExternalIdAsync(Guid jobSourceId, string externalJobId, CancellationToken cancellationToken);
    Task AddAsync(JobSourcePosting posting, CancellationToken cancellationToken);
}
