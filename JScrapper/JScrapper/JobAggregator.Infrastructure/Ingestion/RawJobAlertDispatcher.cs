using JobAggregator.Application.Abstractions.Ingestion;
using JobAggregator.Application.Abstractions.Providers;
using JobAggregator.Domain.Entities;
using JobAggregator.Infrastructure.Alerts;
using JobAggregator.Infrastructure.Notifications;
using JobAggregator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobAggregator.Infrastructure.Ingestion;

public sealed class RawJobAlertDispatcher(
    JobAggregatorDbContext dbContext,
    ILogger<RawJobAlertDispatcher> logger) : IRawJobAlertDispatcher
{
    public async Task DispatchAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (jobs.Count == 0)
        {
            return;
        }

        var lookbackUtc = DateTimeOffset.UtcNow.AddMinutes(-30);
        var externalIds = jobs
            .Where(x => !string.IsNullOrWhiteSpace(x.ExternalJobId))
            .Select(x => x.ExternalJobId)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (externalIds.Length == 0)
        {
            return;
        }

        var providerNames = jobs
            .Select(x => x.ProviderName)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var newCanonicalJobs = await dbContext.JobSourcePostings
            .AsNoTracking()
            .Where(x => externalIds.Contains(x.ExternalJobId)
                        && providerNames.Contains(x.JobSource.Name)
                        && x.Job.CreatedAtUtc >= lookbackUtc)
            .Include(x => x.Job)
                .ThenInclude(x => x.Company)
            .Include(x => x.Job)
                .ThenInclude(x => x.JobLocation)
            .Include(x => x.Job)
                .ThenInclude(x => x.JobSalary)
            .Include(x => x.Job)
                .ThenInclude(x => x.JobSkills)
            .Select(x => x.Job)
            .Distinct()
            .ToArrayAsync(cancellationToken);

        if (newCanonicalJobs.Length == 0)
        {
            logger.LogInformation("Alert stage found no newly inserted canonical jobs to evaluate.");
            return;
        }

        var enabledAlerts = await dbContext.JobAlerts
            .Where(x => x.IsEnabled)
            .ToArrayAsync(cancellationToken);

        if (enabledAlerts.Length == 0)
        {
            logger.LogInformation("Alert stage skipped because no enabled alerts exist.");
            return;
        }

        var jobIds = newCanonicalJobs.Select(x => x.Id).ToArray();
        var sourcesByJob = await dbContext.JobSourcePostings
            .AsNoTracking()
            .Where(x => jobIds.Contains(x.JobId) && x.IsActive)
            .Select(x => new { x.JobId, SourceName = x.JobSource.Name })
            .ToArrayAsync(cancellationToken);

        var groupedSources = sourcesByJob
            .GroupBy(x => x.JobId)
            .ToDictionary(g => g.Key, g => (IReadOnlyCollection<string>)g.Select(x => x.SourceName).Distinct(StringComparer.OrdinalIgnoreCase).ToArray());

        var candidateNotifications = new List<Notification>();

        foreach (var job in newCanonicalJobs)
        {
            var sources = groupedSources.TryGetValue(job.Id, out var resolvedSources)
                ? resolvedSources
                : Array.Empty<string>();

            foreach (var alert in enabledAlerts)
            {
                var match = JobAlertMatcher.Match(job, sources, alert);
                if (!match.IsMatch)
                {
                    continue;
                }

                candidateNotifications.Add(new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = alert.UserId,
                    JobId = job.Id,
                    AlertId = alert.Id,
                    Type = "JobAlertMatch",
                    Channel = NotificationChannels.InApp,
                    Status = NotificationStatuses.Pending,
                    Title = $"New job match: {job.Title}",
                    Message = $"{job.Title} at {job.Company.Name} matched alert '{alert.Name}'.",
                    IsRead = false,
                    RelatedEntityType = "Job",
                    RelatedEntityId = job.Id.ToString("D")
                });

                alert.LastRunAtUtc = DateTimeOffset.UtcNow;
            }
        }

        if (candidateNotifications.Count == 0)
        {
            logger.LogInformation("Alert stage evaluated {JobCount} jobs and found no matches.", newCanonicalJobs.Length);
            return;
        }

        var dedupeKeys = candidateNotifications
            .Select(x => new { x.UserId, x.JobId, x.AlertId, x.Channel })
            .Distinct()
            .ToArray();

        var existing = await dbContext.Notifications
            .Where(x => dedupeKeys.Select(k => k.UserId).Contains(x.UserId)
                        && x.JobId.HasValue
                        && x.AlertId.HasValue
                        && x.Channel == NotificationChannels.InApp)
            .Select(x => new { x.UserId, x.JobId, x.AlertId, x.Channel })
            .ToArrayAsync(cancellationToken);

        var existingHash = existing
            .Select(x => BuildKey(x.UserId, x.JobId!.Value, x.AlertId!.Value, x.Channel))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var toInsert = candidateNotifications
            .Where(x => x.JobId.HasValue && x.AlertId.HasValue)
            .Where(x => !existingHash.Contains(BuildKey(x.UserId, x.JobId!.Value, x.AlertId!.Value, x.Channel)))
            .ToArray();

        if (toInsert.Length == 0)
        {
            logger.LogInformation("Alert stage found only duplicate notifications; no new records created.");
            return;
        }

        await dbContext.Notifications.AddRangeAsync(toInsert, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Alert stage matched {MatchCount} alerts against {JobCount} newly inserted canonical jobs and created {NotificationCount} pending notifications.",
            candidateNotifications.Count,
            newCanonicalJobs.Length,
            toInsert.Length);
    }

    private static string BuildKey(Guid userId, Guid jobId, Guid alertId, string channel)
        => $"{userId:D}|{jobId:D}|{alertId:D}|{channel}";
}
