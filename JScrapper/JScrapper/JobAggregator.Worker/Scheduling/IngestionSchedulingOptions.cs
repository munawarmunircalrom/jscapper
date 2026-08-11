namespace JobAggregator.Worker.Scheduling;

public sealed class IngestionSchedulingOptions
{
    public List<ProviderIngestionScheduleOptions> Providers { get; set; } = [];
}
