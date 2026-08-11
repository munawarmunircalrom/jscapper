using JobAggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobAggregator.Infrastructure.Persistence.Configurations;

public sealed class JobSkillConfiguration : IEntityTypeConfiguration<JobSkill>
{
    public void Configure(EntityTypeBuilder<JobSkill> builder)
    {
        builder.ToTable("JobSkills");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.IsRequired).IsRequired();

        builder.HasOne(x => x.Job)
            .WithMany(x => x.JobSkills)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.JobId, x.Name }).IsUnique();
        builder.HasIndex(x => x.Name);

        builder.ConfigureAuditable();
    }
}
