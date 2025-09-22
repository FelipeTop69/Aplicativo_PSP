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
    [Route("api/subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly ColegioDbContext _db;
        private readonly IMapper _mapper;

        public SubjectsController(ColegioDbContext db, IMapper mapper)
        {
            _db = db; _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<SubjectResponseDTO>>> GetAll(
            [FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            var query = _db.Subjects.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(s => s.Code.Contains(q) || s.Name.Contains(q));
            }

            var (items, total) = await query
                .OrderBy(s => s.Name)
                .Select(s => new SubjectResponseDTO(s.Id, s.Code, s.Name, s.WeeklyHours))
                .ToPagedAsync(page, pageSize, ct);

            return Ok(new PagedResult<SubjectResponseDTO>(items, page, pageSize, total));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubjectResponseDTO>> GetById(int id, CancellationToken ct = default)
        {
            var s = await _db.Subjects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (s is null) return NotFound();
            return Ok(new SubjectResponseDTO(s.Id, s.Code, s.Name, s.WeeklyHours));
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] SubjectRequestDTO dto, CancellationToken ct = default)
        {
            if (await _db.Subjects.AnyAsync(x => x.Code == dto.Code, ct))
                return Conflict("Code ya existe.");

            var entity = _mapper.Map<Colegio.Domain.Entities.Subject>(dto);
            _db.Subjects.Add(entity);
            await _db.SaveChangesAsync(ct);
            return Ok(entity.Id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SubjectRequestDTO dto, CancellationToken ct = default)
        {
            var entity = await _db.Subjects.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return NotFound();

            if (entity.Code != dto.Code && await _db.Subjects.AnyAsync(x => x.Code == dto.Code && x.Id != id, ct))
                return Conflict("Code ya está en uso.");

            _mapper.Map(dto, entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var e = await _db.Subjects.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (e is null) return NotFound();

            var used = await _db.SubjectOfferings.AnyAsync(x => x.SubjectId == id, ct);
            if (used) return Conflict("No se puede eliminar: la materia está asociada a periodos (SubjectOffering).");

            _db.Subjects.Remove(e);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
