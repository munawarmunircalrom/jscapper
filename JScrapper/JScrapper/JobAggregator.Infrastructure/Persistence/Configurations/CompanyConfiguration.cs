using JobAggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobAggregator.Infrastructure.Persistence.Configurations;

public sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        builder.Property(x => x.WebsiteUrl).HasMaxLength(500);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.IsDeleted).IsRequired();

        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => new { x.Name, x.IsDeleted })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.ConfigureAuditable();
    }
}
