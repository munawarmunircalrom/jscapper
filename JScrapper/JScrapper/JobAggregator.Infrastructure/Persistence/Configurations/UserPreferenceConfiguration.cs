using JobAggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobAggregator.Infrastructure.Persistence.Configurations;

public sealed class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
{
    public void Configure(EntityTypeBuilder<UserPreference> builder)
    {
        builder.ToTable("UserPreferences");
        builder.HasKey(x => x.UserId);

        builder.Property(x => x.PreferredKeywords).HasMaxLength(1000);
        builder.Property(x => x.PreferredLocations).HasMaxLength(1000);
        builder.Property(x => x.MinSalary).HasPrecision(18, 2);
        builder.Property(x => x.PreferredCurrency).IsRequired().HasMaxLength(3).IsUnicode(false);

        builder.HasOne(x => x.User)
            .WithOne(x => x.Preference)
            .HasForeignKey<UserPreference>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ConfigureAuditable();
    }
}
