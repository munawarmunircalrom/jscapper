using JobAggregator.Application.Abstractions.Background;
using JobAggregator.Worker.Jobs;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Quartz;

namespace JobAggregator.Infrastructure.Tests.Background;

public sealed class ProviderIngestionJobTests
{
    [Fact]
    public async Task Execute_ShouldRetryAndSucceedBeforeMaxAttempts()
    {
        var orchestrator = new Mock<IJobIngestionOrchestrator>();
        orchestrator
            .SetupSequence(x => x.RunProviderAsync("LinkedIn", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("fail"))
            .Returns(Task.CompletedTask);

        var context = BuildContext("LinkedIn", timeoutSeconds: 10, maxAttempts: 2, retryBaseDelaySeconds: 1);
        var job = new ProviderIngestionJob(orchestrator.Object, NullLogger<ProviderIngestionJob>.Instance);

        await job.Execute(context.Object);

        orchestrator.Verify(x => x.RunProviderAsync("LinkedIn", It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Execute_ShouldThrow_WhenProviderNameMissing()
    {
        var orchestrator = new Mock<IJobIngestionOrchestrator>();
        var context = BuildContext(providerName: "", timeoutSeconds: 10, maxAttempts: 1, retryBaseDelaySeconds: 1);
        var job = new ProviderIngestionJob(orchestrator.Object, NullLogger<ProviderIngestionJob>.Instance);

        await Assert.ThrowsAsync<JobExecutionException>(() => job.Execute(context.Object));
    }

    private static Mock<IJobExecutionContext> BuildContext(string providerName, int timeoutSeconds, int maxAttempts, int retryBaseDelaySeconds)
    {
        var map = new JobDataMap
        {
            [ProviderIngestionJob.ProviderNameKey] = providerName,
            [ProviderIngestionJob.TimeoutSecondsKey] = timeoutSeconds,
            [ProviderIngestionJob.MaxAttemptsKey] = maxAttempts,
            [ProviderIngestionJob.RetryBaseDelaySecondsKey] = retryBaseDelaySeconds
        };

        var context = new Mock<IJobExecutionContext>();
        context.SetupGet(x => x.MergedJobDataMap).Returns(map);
        context.SetupGet(x => x.CancellationToken).Returns(CancellationToken.None);
        return context;
    }
}
