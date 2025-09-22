using Colegio.Domain.Common;

namespace Colegio.Domain.Entities;

public class Enrollment : BaseEntity
{
    public int SubjectOfferingId { get; set; }
    public int StudentId { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public decimal? FinalAverage { get; set; }

    public SubjectOffering SubjectOffering { get; set; } = null!;
    public Student Student { get; set; } = null!;
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
