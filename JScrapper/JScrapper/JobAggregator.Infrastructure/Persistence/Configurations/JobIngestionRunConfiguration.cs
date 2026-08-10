using JobAggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobAggregator.Infrastructure.Persistence.Configurations;

public sealed class JobIngestionRunConfiguration : IEntityTypeConfiguration<JobIngestionRun>
{
    public void Configure(EntityTypeBuilder<JobIngestionRun> builder)
    {
        builder.ToTable("JobIngestionRuns");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status).IsRequired().HasMaxLength(30);

        builder.HasOne(x => x.JobSource)
            .WithMany(x => x.IngestionRuns)
            .HasForeignKey(x => x.JobSourceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.StartedAtUtc);
        builder.HasIndex(x => x.Status);

        builder.ConfigureAuditable();
    }
}
