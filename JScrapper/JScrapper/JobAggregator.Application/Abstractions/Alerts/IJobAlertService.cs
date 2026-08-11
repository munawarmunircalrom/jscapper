using JobAggregator.Application.DTOs;

namespace JobAggregator.Application.Abstractions.Alerts;

public interface IJobAlertService
{
    Task<IReadOnlyCollection<JobAlertDto>> GetAlertsAsync(Guid userId, CancellationToken cancellationToken);
    Task<JobAlertDto> CreateAlertAsync(Guid userId, UpsertJobAlertRequest request, CancellationToken cancellationToken);
    Task<JobAlertDto> UpdateAlertAsync(Guid userId, Guid alertId, UpsertJobAlertRequest request, CancellationToken cancellationToken);
    Task PauseAlertAsync(Guid userId, Guid alertId, CancellationToken cancellationToken);
    Task ResumeAlertAsync(Guid userId, Guid alertId, CancellationToken cancellationToken);
    Task DeleteAlertAsync(Guid userId, Guid alertId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<NotificationHistoryItemDto>> GetNotificationHistoryAsync(Guid userId, CancellationToken cancellationToken);
}
