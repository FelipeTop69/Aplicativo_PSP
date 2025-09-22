using Microsoft.EntityFrameworkCore;
using Colegio.Domain.Entities;
using Colegio.Infrastructure.Persistence.Seed;

namespace Colegio.Infrastructure.Persistence;
public class ColegioDbContext : DbContext
{
    public ColegioDbContext(DbContextOptions<ColegioDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Period> Periods => Set<Period>();
    public DbSet<SubjectOffering> SubjectOfferings => Set<SubjectOffering>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<AssessmentType> AssessmentTypes => Set<AssessmentType>();
    public DbSet<Grade> Grades => Set<Grade>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ColegioDbContext).Assembly);
        modelBuilder.SeedInitialData();
        base.OnModelCreating(modelBuilder);
    }
}
