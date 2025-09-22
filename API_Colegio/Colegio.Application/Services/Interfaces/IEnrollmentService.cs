using Colegio.Application.DTOs;

namespace Colegio.Application.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<int> EnrollAsync(EnrollmentRequestDTO dto);
        Task RecalculateFinalAverageAsync(int enrollmentId);
    }
}
