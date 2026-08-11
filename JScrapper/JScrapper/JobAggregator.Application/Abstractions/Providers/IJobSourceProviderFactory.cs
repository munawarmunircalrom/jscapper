namespace JobAggregator.Application.Abstractions.Providers;

public interface IJobSourceProviderFactory
{
    IJobSourceProvider GetRequiredProvider(string providerName);
    IReadOnlyCollection<IJobSourceProvider> GetEnabledProviders();
}
