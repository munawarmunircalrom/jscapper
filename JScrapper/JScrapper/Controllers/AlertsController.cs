using Asp.Versioning;
using JobAggregator.Application.Abstractions.Alerts;
using JobAggregator.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobAggregator.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("alerts")]
[Authorize]
public sealed class AlertsController(IJobAlertService jobAlertService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<JobAlertDto>>> GetAlerts(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await jobAlertService.GetAlertsAsync(userId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<JobAlertDto>> CreateAlert([FromBody] UpsertJobAlertRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await jobAlertService.CreateAlertAsync(userId, request, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{alertId:guid}")]
    public async Task<ActionResult<JobAlertDto>> EditAlert(Guid alertId, [FromBody] UpsertJobAlertRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await jobAlertService.UpdateAlertAsync(userId, alertId, request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{alertId:guid}/pause")]
    public async Task<ActionResult> PauseAlert(Guid alertId, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        await jobAlertService.PauseAlertAsync(userId, alertId, cancellationToken);
        return NoContent();
    }

    [HttpPost("{alertId:guid}/resume")]
    public async Task<ActionResult> ResumeAlert(Guid alertId, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        await jobAlertService.ResumeAlertAsync(userId, alertId, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{alertId:guid}")]
    public async Task<ActionResult> DeleteAlert(Guid alertId, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        await jobAlertService.DeleteAlertAsync(userId, alertId, cancellationToken);
        return NoContent();
    }

    [HttpGet("notifications")]
    public async Task<ActionResult<IReadOnlyCollection<NotificationHistoryItemDto>>> NotificationHistory(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await jobAlertService.GetNotificationHistoryAsync(userId, cancellationToken);
        return Ok(result);
    }

    private bool TryGetUserId(out Guid userId)
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? User.FindFirstValue("user_id");

        return Guid.TryParse(id, out userId);
    }
}
