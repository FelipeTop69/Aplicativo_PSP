using Colegio.Application.DTOs;
using Colegio.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.API.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _svc;
        public EnrollmentsController(IEnrollmentService svc) => _svc = svc;

        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentRequestDTO dto)
            => Ok(await _svc.EnrollAsync(dto));

        [HttpPost("{id:int}/recalculate")]
        public async Task<IActionResult> Recalculate([FromRoute] int id)
        {
            await _svc.RecalculateFinalAverageAsync(id);
            return NoContent();
        }
    }
}
