using JobAggregator.Application.Abstractions.Background;
using JobAggregator.Domain.Entities;
using JobAggregator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobAggregator.Infrastructure.Background;

public sealed class IngestionHistoryStore(JobAggregatorDbContext dbContext) : IIngestionHistoryStore
{
    public async Task<Guid> StartRunAsync(string providerName, CancellationToken cancellationToken)
    {
        var source = await dbContext.JobSources
            .FirstOrDefaultAsync(x => x.Name == providerName, cancellationToken);

        if (source is null)
        {
            source = new JobSource
            {
                Id = Guid.NewGuid(),
                Name = providerName,
                IsActive = true
            };

            await dbContext.JobSources.AddAsync(source, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var run = new JobIngestionRun
        {
            Id = Guid.NewGuid(),
            JobSourceId = source.Id,
            StartedAtUtc = DateTimeOffset.UtcNow,
            Status = "Running",
            TotalFetched = 0,
            InsertedCount = 0,
            UpdatedCount = 0,
            DuplicateCount = 0,
            ErrorCount = 0
        };

        await dbContext.JobIngestionRuns.AddAsync(run, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return run.Id;
    }

    public async Task CompleteRunAsync(
        Guid runId,
        string status,
        int totalFetched,
        int insertedCount,
        int updatedCount,
        int duplicateCount,
        int errorCount,
        CancellationToken cancellationToken)
    {
        var run = await dbContext.JobIngestionRuns.FirstOrDefaultAsync(x => x.Id == runId, cancellationToken);
        if (run is null)
        {
            return;
        }

        run.Status = status;
        run.CompletedAtUtc = DateTimeOffset.UtcNow;
        run.TotalFetched = totalFetched;
        run.InsertedCount = insertedCount;
        run.UpdatedCount = updatedCount;
        run.DuplicateCount = duplicateCount;
        run.ErrorCount = errorCount;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RecordErrorAsync(Guid runId, string errorCode, string errorMessage, CancellationToken cancellationToken)
    {
        var error = new JobIngestionError
        {
            Id = Guid.NewGuid(),
            JobIngestionRunId = runId,
            ErrorCode = errorCode,
            ErrorMessage = errorMessage,
            OccurredAtUtc = DateTimeOffset.UtcNow
        };

        await dbContext.JobIngestionErrors.AddAsync(error, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
