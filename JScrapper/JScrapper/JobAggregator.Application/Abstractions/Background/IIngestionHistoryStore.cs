namespace JobAggregator.Application.Abstractions.Background;

public interface IIngestionHistoryStore
{
    Task<Guid> StartRunAsync(string providerName, CancellationToken cancellationToken);

    Task CompleteRunAsync(
        Guid runId,
        string status,
        int totalFetched,
        int insertedCount,
        int updatedCount,
        int duplicateCount,
        int errorCount,
        CancellationToken cancellationToken);

    Task RecordErrorAsync(Guid runId, string errorCode, string errorMessage, CancellationToken cancellationToken);
}
