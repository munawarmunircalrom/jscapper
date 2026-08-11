using JobAggregator.Application.Abstractions.Providers;

namespace JobAggregator.Infrastructure.JobSources;

public sealed class JobSourceProviderFactory(IEnumerable<IJobSourceProvider> providers) : IJobSourceProviderFactory
{
    private readonly IReadOnlyDictionary<string, IJobSourceProvider> _providers =
        providers.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

    public IJobSourceProvider GetRequiredProvider(string providerName)
    {
        if (!_providers.TryGetValue(providerName, out var provider))
        {
            throw new InvalidOperationException($"Provider '{providerName}' is not registered.");
        }

        return provider;
    }

    public IReadOnlyCollection<IJobSourceProvider> GetEnabledProviders()
    {
        return _providers.Values.Where(x => x.Configuration.Enabled).ToArray();
    }
}
