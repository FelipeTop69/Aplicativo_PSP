using Colegio.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Colegio.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly ColegioDbContext _db;

        public ReportsController(ColegioDbContext db)
        {
            _db = db;
        }

        [HttpGet("transcript")]
        public async Task<IActionResult> Transcript([FromQuery] int studentId, [FromQuery] int periodId)
        {
            var mats = await _db.Enrollments.AsNoTracking()
                .Include(e => e.SubjectOffering).ThenInclude(o => o.Subject)
                .Where(e => e.StudentId == studentId && e.SubjectOffering.PeriodId == periodId)
                .Select(e => new {
                    subject = e.SubjectOffering.Subject.Name,
                    final = e.FinalAverage
                })
                .ToListAsync();

            var alumno = await _db.Students.AsNoTracking().Where(s => s.Id == studentId)
                .Select(s => s.FirstName + " " + s.LastName).FirstOrDefaultAsync();

            var period = await _db.Periods.AsNoTracking().Where(p => p.Id == periodId)
                .Select(p => p.Name).FirstOrDefaultAsync();

            return Ok(new { studentId, studentName = alumno, periodId, periodName = period, subjects = mats });
        }
    }
}
