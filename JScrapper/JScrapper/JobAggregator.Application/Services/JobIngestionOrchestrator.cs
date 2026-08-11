using JobAggregator.Application.Abstractions.Background;
using JobAggregator.Application.Abstractions.Ingestion;
using JobAggregator.Application.Abstractions.Providers;
using Microsoft.Extensions.Logging;

namespace JobAggregator.Application.Services;

public sealed class JobIngestionOrchestrator(
    IJobSourceProviderFactory providerFactory,
    IRawJobNormalizer normalizer,
    IRawJobValidator validator,
    IRawJobDeduplicator deduplicator,
    IRawJobPersister persister,
    IRawJobSearchIndexer searchIndexer,
    IRawJobAlertDispatcher alertDispatcher,
    IRawJobNotificationDispatcher notificationDispatcher,
    IIngestionHistoryStore historyStore,
    ILogger<JobIngestionOrchestrator> logger) : IJobIngestionOrchestrator
{
    public async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        foreach (var provider in providerFactory.GetEnabledProviders())
        {
            try
            {
                await RunProviderAsync(provider.Name, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Ingestion failed for provider {ProviderName} in RunOnce loop.", provider.Name);
            }
        }
    }

    public async Task RunProviderAsync(string providerName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var provider = providerFactory.GetRequiredProvider(providerName);
        var runId = await historyStore.StartRunAsync(provider.Name, cancellationToken);

        var totalFetched = 0;
        var totalInserted = 0;
        var totalUpdated = 0;
        var totalDuplicates = 0;
        var totalErrors = 0;

        try
        {
            var health = await provider.CheckHealthAsync(cancellationToken);
            if (!health.IsHealthy)
            {
                logger.LogWarning(
                    "Provider {ProviderName} reported unhealthy state. ConsecutiveFailures={ConsecutiveFailures}, LastError={LastError}",
                    provider.Name,
                    health.ConsecutiveFailures,
                    health.LastError);
            }

            var request = new JobSearchRequest(
                ProviderName: provider.Name,
                Keywords: null,
                Location: null,
                PageNumber: 1,
                PageSize: provider.Configuration.DefaultPageSize,
                MaxPages: 5,
                CorrelationId: Guid.NewGuid().ToString("N"),
                IdempotencyScope: DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyyMMdd"));

            logger.LogInformation("Starting ingestion run {RunId} for provider {ProviderName}.", runId, provider.Name);

            for (var currentPage = 1; currentPage <= request.MaxPages; currentPage++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var pagedRequest = request with { PageNumber = currentPage };
                var fetched = await provider.FetchJobsAsync(pagedRequest, cancellationToken);
                totalFetched += fetched.Jobs.Count;

                var normalized = await normalizer.NormalizeAsync(fetched.Jobs, cancellationToken);
                var validated = await validator.ValidateAsync(normalized, cancellationToken);
                var deduplicated = await deduplicator.DeduplicateAsync(validated, cancellationToken);

                var duplicateCount = deduplicated.Count(x => x.DeduplicationConfidence.GetValueOrDefault() >= 0.75d);
                totalDuplicates += duplicateCount;

                await persister.PersistAsync(deduplicated, cancellationToken);
                await searchIndexer.IndexAsync(deduplicated, cancellationToken);
                await alertDispatcher.DispatchAsync(deduplicated, cancellationToken);
                await notificationDispatcher.DispatchAsync(deduplicated, cancellationToken);

                totalInserted += deduplicated.Count;

                logger.LogInformation(
                    "Run {RunId} provider {ProviderName} page {PageNumber} completed. fetched={Fetched}, normalized={Normalized}, validated={Validated}, processed={Processed}, duplicates={Duplicates}",
                    runId,
                    provider.Name,
                    currentPage,
                    fetched.Jobs.Count,
                    normalized.Count,
                    validated.Count,
                    deduplicated.Count,
                    duplicateCount);

                if (!fetched.HasMore)
                {
                    break;
                }
            }

            await historyStore.CompleteRunAsync(
                runId,
                status: "Succeeded",
                totalFetched,
                totalInserted,
                totalUpdated,
                totalDuplicates,
                totalErrors,
                cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            totalErrors++;
            await historyStore.RecordErrorAsync(runId, "CANCELED", "Run canceled.", cancellationToken);
            await historyStore.CompleteRunAsync(runId, "Canceled", totalFetched, totalInserted, totalUpdated, totalDuplicates, totalErrors, cancellationToken);
            throw;
        }
        catch (Exception exception)
        {
            totalErrors++;
            await historyStore.RecordErrorAsync(runId, "INGESTION_FAILED", exception.Message, cancellationToken);
            await historyStore.CompleteRunAsync(runId, "Failed", totalFetched, totalInserted, totalUpdated, totalDuplicates, totalErrors, cancellationToken);
            throw;
        }
    }
}
