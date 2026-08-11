using JobAggregator.Application.Abstractions.Providers;

namespace JobAggregator.Application.Abstractions.Ingestion;

public interface IRawJobValidator
{
    Task<IReadOnlyCollection<RawJob>> ValidateAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken);
}
