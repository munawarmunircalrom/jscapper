using JobAggregator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobAggregator.Infrastructure.Tests.EFCore;

public sealed class EfCoreModelTests
{
    [Fact]
    public void NotificationModel_ShouldContainUniqueDeduplicationIndex()
    {
        var context = CreateDbContext();
        var entity = context.Model.FindEntityType("JobAggregator.Domain.Entities.Notification");

        Assert.NotNull(entity);
        var dedupeIndex = entity!.GetIndexes().FirstOrDefault(x => x.IsUnique && x.Properties.Select(p => p.Name).SequenceEqual(["UserId", "JobId", "AlertId", "Channel"]));
        Assert.NotNull(dedupeIndex);
    }

    [Fact]
    public void JobAlertSalaryFields_ShouldHavePrecision()
    {
        var context = CreateDbContext();
        var entity = context.Model.FindEntityType("JobAggregator.Domain.Entities.JobAlert");

        var min = entity!.FindProperty("MinSalary");
        var max = entity.FindProperty("MaxSalary");

        Assert.Equal(18, min!.GetPrecision());
        Assert.Equal(2, min.GetScale());
        Assert.Equal(18, max!.GetPrecision());
        Assert.Equal(2, max.GetScale());
    }

    private static JobAggregatorDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<JobAggregatorDbContext>()
            .UseInMemoryDatabase($"ef-model-tests-{Guid.NewGuid():N}")
            .Options;

        return new JobAggregatorDbContext(options);
    }
}
