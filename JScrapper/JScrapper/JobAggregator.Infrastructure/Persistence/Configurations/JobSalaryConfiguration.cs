using JobAggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobAggregator.Infrastructure.Persistence.Configurations;

public sealed class JobSalaryConfiguration : IEntityTypeConfiguration<JobSalary>
{
    public void Configure(EntityTypeBuilder<JobSalary> builder)
    {
        builder.ToTable("JobSalaries", table =>
        {
            table.HasCheckConstraint("CK_JobSalaries_MinLessOrEqualMax", "[MinAmount] IS NULL OR [MaxAmount] IS NULL OR [MinAmount] <= [MaxAmount]");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MinAmount).HasPrecision(18, 2);
        builder.Property(x => x.MaxAmount).HasPrecision(18, 2);
        builder.Property(x => x.Currency).IsRequired().HasMaxLength(3).IsUnicode(false);
        builder.Property(x => x.Period).HasMaxLength(30);

        builder.ConfigureAuditable();
    }
}
