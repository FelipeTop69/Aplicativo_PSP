using Colegio.Domain.Common;

namespace Colegio.Domain.Entities;
public class Grade : BaseEntity
{
    public int EnrollmentId { get; set; }
    public int AssessmentTypeId { get; set; }
    public decimal Score { get; set; } // 0.00..5.00
    public DateTime GradedAt { get; set; } = DateTime.UtcNow;

    public Enrollment Enrollment { get; set; } = null!;
    public AssessmentType AssessmentType { get; set; } = null!;
}
