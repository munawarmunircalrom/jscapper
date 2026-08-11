using JobAggregator.Application.Abstractions.Ingestion;
using JobAggregator.Application.Abstractions.Notifications;
using JobAggregator.Application.Abstractions.Providers;
using JobAggregator.Application.DTOs;
using JobAggregator.Infrastructure.Notifications;
using JobAggregator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobAggregator.Infrastructure.Ingestion;

public sealed class RawJobNotificationDispatcher(
    JobAggregatorDbContext dbContext,
    IEmailNotificationSender emailNotificationSender,
    ILogger<RawJobNotificationDispatcher> logger) : IRawJobNotificationDispatcher
{
    public async Task DispatchAsync(IReadOnlyCollection<RawJob> jobs, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (jobs.Count == 0)
        {
            return;
        }

        var pendingNotifications = await dbContext.Notifications
            .Where(x => x.Status == NotificationStatuses.Pending)
            .OrderBy(x => x.CreatedAtUtc)
            .Take(500)
            .ToArrayAsync(cancellationToken);

        if (pendingNotifications.Length == 0)
        {
            logger.LogInformation("Notification stage found no pending notifications.");
            return;
        }

        foreach (var notification in pendingNotifications)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                if (string.Equals(notification.Channel, NotificationChannels.Email, StringComparison.OrdinalIgnoreCase))
                {
                    await emailNotificationSender.SendJobAlertNotificationAsync(
                        new NotificationHistoryItemDto
                        {
                            NotificationId = notification.Id,
                            UserId = notification.UserId,
                            JobId = notification.JobId,
                            AlertId = notification.AlertId,
                            SentAtUtc = notification.SentAtUtc,
                            Status = notification.Status,
                            Channel = notification.Channel,
                            Title = notification.Title,
                            Message = notification.Message,
                            IsRead = notification.IsRead,
                            CreatedAtUtc = notification.CreatedAtUtc
                        },
                        cancellationToken);
                }

                notification.Status = NotificationStatuses.Sent;
                notification.SentAtUtc = DateTimeOffset.UtcNow;
            }
            catch (Exception ex)
            {
                notification.Status = NotificationStatuses.Failed;
                logger.LogError(ex, "Failed to deliver notification {NotificationId} via {Channel}.", notification.Id, notification.Channel);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Notification stage processed and finalized {Count} pending notifications.", pendingNotifications.Length);
    }
}
