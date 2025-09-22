using Colegio.Application.DTOs;

namespace Colegio.Application.Services.Interfaces;

public interface ISubjectOfferingService
{
    Task<int> CreateAsync(SubjectOfferingRequestDTO dto);
    Task CloseAsync(int offeringId);
    Task<SubjectOfferingResponseDTO?> GetAsync(int id);
}

