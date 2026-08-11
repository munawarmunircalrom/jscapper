using JobAggregator.Application.Abstractions.Providers;

namespace JobAggregator.Application.Abstractions.Ingestion;

public interface IRawJobNormalizer
{
    Task<IReadOnlyCollection<RawJob>> NormalizeAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken);
}
