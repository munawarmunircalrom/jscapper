using JobAggregator.Application.Abstractions.Persistence;
using JobAggregator.Domain.Entities;
using JobAggregator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobAggregator.Infrastructure.Repositories;

public sealed class JobRepository(JobAggregatorDbContext dbContext) : IJobRepository
{
    public Task<Job?> GetByCanonicalHashAsync(string canonicalHash, CancellationToken cancellationToken)
    {
        return dbContext.Jobs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CanonicalHash == canonicalHash, cancellationToken);
    }

    public async Task AddAsync(Job job, CancellationToken cancellationToken)
    {
        await dbContext.Jobs.AddAsync(job, cancellationToken);
    }
}
