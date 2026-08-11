using JobAggregator.Application.Abstractions.Providers;
using JobAggregator.Infrastructure.JobSources;
using Microsoft.Extensions.Logging.Abstractions;

namespace JobAggregator.Infrastructure.Tests.Providers;

public sealed class ProviderReliabilityTests
{
    [Fact]
    public async Task FetchJobsAsync_ShouldRetryAndEventuallySucceed_WhenTransientFailuresOccur()
    {
        var provider = new TestProvider(failuresBeforeSuccess: 2, mode: FailureMode.Exception);

        var result = await provider.FetchJobsAsync(BuildRequest(timeoutMs: 200), CancellationToken.None);

        Assert.Equal(3, result.Attempts);
        Assert.Single(result.Jobs);
    }

    [Fact]
    public async Task FetchJobsAsync_ShouldThrowAfterTimeoutRetriesExhausted()
    {
        var provider = new TestProvider(failuresBeforeSuccess: int.MaxValue, mode: FailureMode.Timeout);

        await Assert.ThrowsAsync<InvalidOperationException>(() => provider.FetchJobsAsync(BuildRequest(timeoutMs: 30), CancellationToken.None));
    }

    private static JobSearchRequest BuildRequest(int timeoutMs)
        => new(
            ProviderName: "TestProvider",
            Keywords: null,
            Location: null,
            PageNumber: 1,
            PageSize: 10,
            MaxPages: 1,
            Timeout: TimeSpan.FromMilliseconds(timeoutMs),
            CorrelationId: "test",
            IdempotencyScope: "scope");

    private enum FailureMode { Exception, Timeout }

    private sealed class TestProvider(int failuresBeforeSuccess, FailureMode mode) : JobSourceProviderBase(NullLogger.Instance)
    {
        private int attempts;

        public override string Name => "TestProvider";

        public override JobProviderConfiguration Configuration { get; } = new()
        {
            Name = "TestProvider",
            MaxRetries = 2,
            RetryDelayMilliseconds = 1,
            RequestsPerMinute = 1000,
            TimeoutSeconds = 1,
            DefaultPageSize = 10,
            MaxPageSize = 10
        };

        protected override async Task<(IReadOnlyCollection<RawJob> Jobs, bool HasMore, string? NextCursor)> FetchPageCoreAsync(JobSearchRequest request, CancellationToken cancellationToken)
        {
            attempts++;

            if (attempts <= failuresBeforeSuccess)
            {
                if (mode == FailureMode.Timeout)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }

                throw new InvalidOperationException("provider failure");
            }

            var job = new RawJob(
                ProviderName: Name,
                ExternalJobId: "1",
                Title: "Backend Engineer",
                Company: "Acme",
                Description: "Build APIs",
                Location: "Remote",
                SalaryMin: null,
                SalaryMax: null,
                Currency: null,
                EmploymentType: "Full-time",
                Experience: "Mid",
                Skills: ["C#"],
                PostedAtUtc: DateTimeOffset.UtcNow,
                SourceUrl: null,
                CanonicalUrl: null,
                ContentHash: null,
                DeduplicationConfidence: null,
                IdempotencyKey: string.Empty);

            return ([job], false, null);
        }
    }
}
