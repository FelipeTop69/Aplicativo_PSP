using AutoMapper;
using Colegio.API.Extensions;
using Colegio.API.Models;
using Colegio.Application.DTOs;
using Colegio.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Colegio.API.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly ColegioDbContext _db;
        private readonly IMapper _mapper;

        public StudentsController(ColegioDbContext db, IMapper mapper)
        {
            _db = db; _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<StudentResponseDTO>>> GetAll(
            [FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            var query = _db.Students.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(s =>
                    s.DocumentNumber.Contains(q) ||
                    s.FirstName.Contains(q) ||
                    s.LastName.Contains(q) ||
                    (s.Email != null && s.Email.Contains(q)));
            }

            var (items, total) = await query
                .OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                .Select(s => new StudentResponseDTO(s.Id, s.DocumentNumber, s.FirstName + " " + s.LastName, s.Email, s.Status))
                .ToPagedAsync(page, pageSize, ct);

            return Ok(new PagedResult<StudentResponseDTO>(items, page, pageSize, total));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentResponseDTO>> GetById(int id, CancellationToken ct = default)
        {
            var s = await _db.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (s is null) return NotFound();
            return Ok(new StudentResponseDTO(s.Id, s.DocumentNumber, s.FirstName + " " + s.LastName, s.Email, s.Status));
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] StudentRequestDTO dto, CancellationToken ct = default)
        {
            var exists = await _db.Students.AnyAsync(x => x.DocumentNumber == dto.DocumentNumber, ct);
            if (exists) return Conflict("Ya existe un estudiante con ese DocumentNumber.");

            var entity = _mapper.Map<Colegio.Domain.Entities.Student>(dto);
            _db.Students.Add(entity);
            await _db.SaveChangesAsync(ct);
            return Ok(entity.Id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentRequestDTO dto, CancellationToken ct = default)
        {
            var entity = await _db.Students.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return NotFound();

            // Verificar unicidad si cambian el DocumentNumber
            if (entity.DocumentNumber != dto.DocumentNumber)
            {
                var dup = await _db.Students.AnyAsync(x => x.DocumentNumber == dto.DocumentNumber && x.Id != id, ct);
                if (dup) return Conflict("DocumentNumber ya está en uso por otro estudiante.");
            }

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var e = await _db.Students.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (e is null) return NotFound();

            // Evitar borrar si tiene matrículas
            var hasEnrollments = await _db.Enrollments.AnyAsync(x => x.StudentId == id, ct);
            if (hasEnrollments) return Conflict("No se puede eliminar: el estudiante tiene matrículas asociadas.");

            _db.Students.Remove(e);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
