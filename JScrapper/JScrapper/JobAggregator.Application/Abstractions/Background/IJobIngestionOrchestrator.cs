namespace JobAggregator.Application.Abstractions.Background;

public interface IJobIngestionOrchestrator
{
    Task RunOnceAsync(CancellationToken cancellationToken);
    Task RunProviderAsync(string providerName, CancellationToken cancellationToken);
}
