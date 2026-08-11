using JobAggregator.Application.DTOs;

namespace JobAggregator.Application.Abstractions.Notifications;

public interface IEmailNotificationSender
{
    Task SendJobAlertNotificationAsync(NotificationHistoryItemDto notification, CancellationToken cancellationToken);
}
