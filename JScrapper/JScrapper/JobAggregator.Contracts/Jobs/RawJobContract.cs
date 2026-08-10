namespace JobAggregator.Contracts.Jobs;

public sealed record RawJobContract(
    string ExternalId,
    string Title,
    string Company,
    string Location,
    string Source,
    Uri? SourceUrl,
    DateTimeOffset? PostedAtUtc);
