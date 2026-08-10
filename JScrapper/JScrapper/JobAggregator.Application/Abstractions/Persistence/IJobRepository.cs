using JobAggregator.Domain.Entities;

namespace JobAggregator.Application.Abstractions.Persistence;

public interface IJobRepository
{
    Task<Job?> GetByCanonicalHashAsync(string canonicalHash, CancellationToken cancellationToken);
    Task AddAsync(Job job, CancellationToken cancellationToken);
}
