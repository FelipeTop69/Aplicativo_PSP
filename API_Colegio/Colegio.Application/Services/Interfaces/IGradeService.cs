using Colegio.Application.DTOs;

namespace Colegio.Application.Services.Interfaces
{
    public interface IGradeService
    {
        Task<int> UpsertAsync(GradeRequestDTO dto); // crea o actualiza
    }
}
