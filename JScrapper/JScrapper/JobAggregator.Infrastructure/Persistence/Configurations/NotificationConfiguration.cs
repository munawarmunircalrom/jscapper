using JobAggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobAggregator.Infrastructure.Persistence.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status).IsRequired().HasMaxLength(40);
        builder.Property(x => x.Channel).IsRequired().HasMaxLength(40);
        builder.Property(x => x.Type).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Message).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.RelatedEntityType).HasMaxLength(100);
        builder.Property(x => x.RelatedEntityId).HasMaxLength(100);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Job)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Alert)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.AlertId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => new { x.UserId, x.IsRead, x.CreatedAtUtc });
        builder.HasIndex(x => new { x.UserId, x.JobId, x.AlertId, x.Channel }).IsUnique();
        builder.HasIndex(x => new { x.Status, x.Channel, x.CreatedAtUtc });

        builder.ConfigureAuditable();
    }
}
