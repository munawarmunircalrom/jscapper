using JobAggregator.Domain.Entities;

namespace JobAggregator.Infrastructure.Tests.Domain;

public sealed class DomainEntityDefaultsTests
{
    [Fact]
    public void JobAlert_ShouldInitializeAsEnabled()
    {
        var alert = new JobAlert();

        Assert.True(alert.IsEnabled);
        Assert.Equal(60, alert.FrequencyMinutes);
        Assert.NotNull(alert.Notifications);
    }

    [Fact]
    public void Notification_ShouldDefaultToInAppPending()
    {
        var notification = new Notification();

        Assert.Equal("Pending", notification.Status);
        Assert.Equal("InApp", notification.Channel);
        Assert.False(notification.IsRead);
    }
}
