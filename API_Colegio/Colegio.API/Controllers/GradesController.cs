using Colegio.Application.DTOs;
using Colegio.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.API.Controllers
{
    [ApiController]
    [Route("api/grades")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _svc;
        public GradesController(IGradeService svc) => _svc = svc;

        [HttpPost("upsert")]
        public async Task<IActionResult> Upsert([FromBody] GradeRequestDTO dto)
            => Ok(await _svc.UpsertAsync(dto));
    }
}
