using FluentValidation;
using JobAggregator.Application.DTOs;
using JobAggregator.Application.Features.Alerts;
using JobAggregator.Infrastructure.Alerts;
using JobAggregator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobAggregator.Infrastructure.Tests.Alerts;

public sealed class JobAlertServiceTests
{
    [Fact]
    public async Task CreatePauseResumeDelete_ShouldManageAlertLifecycle()
    {
        await using var dbContext = CreateDbContext();
        var validator = new UpsertJobAlertRequestValidator();
        var service = new JobAlertService(dbContext, validator);

        var userId = Guid.NewGuid();

        var created = await service.CreateAlertAsync(
            userId,
            new UpsertJobAlertRequest
            {
                Name = "My Alert",
                Keywords = "dotnet",
                Location = "Lahore",
                Skills = ["C#", "SQL"],
                MinSalary = 100000,
                Experience = "Mid",
                EmploymentType = "Full-time",
                Remote = true,
                Sources = ["LinkedIn"],
                FrequencyMinutes = 60
            },
            CancellationToken.None);

        Assert.Equal(userId, created.UserId);
        Assert.True(created.IsEnabled);

        await service.PauseAlertAsync(userId, created.AlertId, CancellationToken.None);
        var paused = await service.GetAlertsAsync(userId, CancellationToken.None);
        Assert.False(paused.Single().IsEnabled);

        await service.ResumeAlertAsync(userId, created.AlertId, CancellationToken.None);
        var resumed = await service.GetAlertsAsync(userId, CancellationToken.None);
        Assert.True(resumed.Single().IsEnabled);

        await service.DeleteAlertAsync(userId, created.AlertId, CancellationToken.None);
        var deleted = await service.GetAlertsAsync(userId, CancellationToken.None);
        Assert.Empty(deleted);
    }

    [Fact]
    public async Task CreateAlertAsync_ShouldFailValidation_WhenNameMissing()
    {
        await using var dbContext = CreateDbContext();
        var validator = new UpsertJobAlertRequestValidator();
        var service = new JobAlertService(dbContext, validator);

        await Assert.ThrowsAsync<ValidationException>(() => service.CreateAlertAsync(
            Guid.NewGuid(),
            new UpsertJobAlertRequest { Name = string.Empty },
            CancellationToken.None));
    }

    private static JobAggregatorDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<JobAggregatorDbContext>()
            .UseInMemoryDatabase($"alerts-service-tests-{Guid.NewGuid():N}")
            .Options;

        return new JobAggregatorDbContext(options);
    }
}
