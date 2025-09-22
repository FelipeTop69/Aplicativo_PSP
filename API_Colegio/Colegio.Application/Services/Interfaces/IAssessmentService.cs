using Colegio.Application.DTOs;

namespace Colegio.Application.Services.Interfaces
{
    public interface IAssessmentService
    {
        Task<int> CreateAsync(AssessmentTypeRequestDTO dto);
        Task ValidateWeightsAsync(int subjectOfferingId); // suma = 100
    }
}
