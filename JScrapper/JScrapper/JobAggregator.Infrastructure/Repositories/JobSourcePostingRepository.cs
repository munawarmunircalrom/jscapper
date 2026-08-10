using JobAggregator.Application.Abstractions.Persistence;
using JobAggregator.Domain.Entities;
using JobAggregator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobAggregator.Infrastructure.Repositories;

public sealed class JobSourcePostingRepository(JobAggregatorDbContext dbContext) : IJobSourcePostingRepository
{
    public Task<JobSourcePosting?> GetBySourceExternalIdAsync(Guid jobSourceId, string externalJobId, CancellationToken cancellationToken)
    {
        return dbContext.JobSourcePostings
            .FirstOrDefaultAsync(x => x.JobSourceId == jobSourceId && x.ExternalJobId == externalJobId, cancellationToken);
    }

    public async Task AddAsync(JobSourcePosting posting, CancellationToken cancellationToken)
    {
        await dbContext.JobSourcePostings.AddAsync(posting, cancellationToken);
    }
}
