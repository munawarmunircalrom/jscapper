using JobAggregator.Application.Abstractions.Notifications;
using JobAggregator.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace JobAggregator.Infrastructure.Notifications;

public sealed class NoOpEmailNotificationSender(ILogger<NoOpEmailNotificationSender> logger) : IEmailNotificationSender
{
    public Task SendJobAlertNotificationAsync(NotificationHistoryItemDto notification, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        logger.LogInformation("Email notification abstraction invoked for NotificationId={NotificationId}.", notification.NotificationId);
        return Task.CompletedTask;
    }
}
