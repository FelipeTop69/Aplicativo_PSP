using Colegio.Domain.Common;

namespace Colegio.Domain.Entities;

public class AssessmentType : BaseEntity
{
    public int SubjectOfferingId { get; set; }
    public string Name { get; set; } = null!; // Parcial, Taller, Examen
    public byte Weight { get; set; }          // 0..100

    public SubjectOffering SubjectOffering { get; set; } = null!;
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
