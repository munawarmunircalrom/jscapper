using JobAggregator.Application.Abstractions.Background;
using Quartz;

namespace JobAggregator.Worker.Jobs;

[DisallowConcurrentExecution]
public sealed class ProviderIngestionJob(
    IJobIngestionOrchestrator orchestrator,
    ILogger<ProviderIngestionJob> logger) : IJob
{
    public const string ProviderNameKey = "ProviderName";
    public const string TimeoutSecondsKey = "TimeoutSeconds";
    public const string MaxAttemptsKey = "MaxAttempts";
    public const string RetryBaseDelaySecondsKey = "RetryBaseDelaySeconds";

    public async Task Execute(IJobExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;

        var providerName = context.MergedJobDataMap.GetString(ProviderNameKey);
        if (string.IsNullOrWhiteSpace(providerName))
        {
            throw new JobExecutionException("ProviderName job data is required.");
        }

        var timeoutSeconds = Math.Max(10, context.MergedJobDataMap.GetInt(TimeoutSecondsKey));
        var maxAttempts = Math.Max(1, context.MergedJobDataMap.GetInt(MaxAttemptsKey));
        var retryBaseDelaySeconds = Math.Max(1, context.MergedJobDataMap.GetInt(RetryBaseDelaySecondsKey));

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            linkedCts.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

            try
            {
                await orchestrator.RunProviderAsync(providerName, linkedCts.Token);
                return;
            }
            catch (OperationCanceledException) when (linkedCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                if (attempt >= maxAttempts)
                {
                    logger.LogError("Provider job {ProviderName} timed out after {TimeoutSeconds}s on final attempt {Attempt}.", providerName, timeoutSeconds, attempt);
                    return;
                }

                var delay = TimeSpan.FromSeconds(Math.Min(retryBaseDelaySeconds * Math.Pow(2, attempt - 1), 60));
                logger.LogWarning("Provider job {ProviderName} timed out on attempt {Attempt}. Retrying in {DelaySeconds}s.", providerName, attempt, delay.TotalSeconds);
                await Task.Delay(delay, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                if (attempt >= maxAttempts)
                {
                    logger.LogError(exception, "Provider job {ProviderName} failed after {Attempt} attempts.", providerName, attempt);
                    return;
                }

                var delay = TimeSpan.FromSeconds(Math.Min(retryBaseDelaySeconds * Math.Pow(2, attempt - 1), 60));
                logger.LogWarning(exception, "Provider job {ProviderName} failed on attempt {Attempt}. Retrying in {DelaySeconds}s.", providerName, attempt, delay.TotalSeconds);
                await Task.Delay(delay, cancellationToken);
            }
        }
    }
}
