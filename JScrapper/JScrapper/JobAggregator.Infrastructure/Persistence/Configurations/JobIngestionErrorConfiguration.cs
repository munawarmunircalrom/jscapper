using JobAggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobAggregator.Infrastructure.Persistence.Configurations;

public sealed class JobIngestionErrorConfiguration : IEntityTypeConfiguration<JobIngestionError>
{
    public void Configure(EntityTypeBuilder<JobIngestionError> builder)
    {
        builder.ToTable("JobIngestionErrors");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ErrorCode).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ErrorMessage).IsRequired().HasMaxLength(2000);

        builder.HasOne(x => x.JobIngestionRun)
            .WithMany(x => x.Errors)
            .HasForeignKey(x => x.JobIngestionRunId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.JobSourcePosting)
            .WithMany()
            .HasForeignKey(x => x.JobSourcePostingId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => new { x.JobIngestionRunId, x.OccurredAtUtc });

        builder.ConfigureAuditable();
    }
}
