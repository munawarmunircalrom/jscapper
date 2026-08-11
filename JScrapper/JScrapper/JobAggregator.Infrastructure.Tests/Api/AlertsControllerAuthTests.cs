using JobAggregator.Api.Controllers;
using JobAggregator.Application.Abstractions.Alerts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace JobAggregator.Infrastructure.Tests.Api;

public sealed class AlertsControllerAuthTests
{
    [Fact]
    public async Task GetAlerts_ShouldReturnUnauthorized_WhenUserClaimMissing()
    {
        var service = new Mock<IJobAlertService>();
        var controller = new AlertsController(service.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            }
        };

        var result = await controller.GetAlerts(CancellationToken.None);

        Assert.IsType<UnauthorizedResult>(result.Result);
    }
}
