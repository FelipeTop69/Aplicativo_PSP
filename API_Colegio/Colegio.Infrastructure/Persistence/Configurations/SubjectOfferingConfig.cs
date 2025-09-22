using Colegio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colegio.Infrastructure.Persistence.Configurations;
public class SubjectOfferingConfig : IEntityTypeConfiguration<SubjectOffering>
{
    public void Configure(EntityTypeBuilder<SubjectOffering> b)
    {
        b.HasIndex(x => new { x.SubjectId, x.PeriodId }).IsUnique();

        b.HasOne(x => x.Subject)
            .WithMany()
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Period)
            .WithMany()
            .HasForeignKey(x => x.PeriodId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
