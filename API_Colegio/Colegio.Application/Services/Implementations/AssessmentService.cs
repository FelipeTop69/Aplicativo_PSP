using Colegio.Application.DTOs;
using Colegio.Application.Services.Interfaces;
using Colegio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Colegio.Application.Services.Implementations;
public class AssessmentService : IAssessmentService
{
    private readonly ColegioDbContext _db;
    public AssessmentService(ColegioDbContext db) => _db = db;

    public async Task<int> CreateAsync(AssessmentTypeRequestDTO dto)
    {
        var off = await _db.SubjectOfferings.Include(o => o.Period).FirstOrDefaultAsync(o => o.Id == dto.SubjectOfferingId)
                  ?? throw new KeyNotFoundException("Offering no encontrado.");
        if (off.IsClosed || off.Period.Status == "Closed") throw new InvalidOperationException("La oferta/periodo está cerrado.");

        var exists = await _db.AssessmentTypes.AnyAsync(a => a.SubjectOfferingId == dto.SubjectOfferingId && a.Name == dto.Name);
        if (exists) throw new InvalidOperationException("Ya existe un rubro con ese nombre.");

        var entity = new Domain.Entities.AssessmentType { SubjectOfferingId = dto.SubjectOfferingId, Name = dto.Name, Weight = dto.Weight };
        _db.AssessmentTypes.Add(entity);
        await _db.SaveChangesAsync();

        await ValidateWeightsAsync(dto.SubjectOfferingId);
        return entity.Id;
    }

    public async Task ValidateWeightsAsync(int subjectOfferingId)
    {
        var sum = await _db.AssessmentTypes
            .Where(a => a.SubjectOfferingId == subjectOfferingId)
            .SumAsync(a => (int)a.Weight);

        if (sum != 100)
            throw new InvalidOperationException($"La suma de pesos es {sum}. Debe ser exactamente 100.");
    }
}
