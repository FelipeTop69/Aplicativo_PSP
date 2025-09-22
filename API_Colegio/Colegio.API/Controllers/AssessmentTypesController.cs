using Colegio.Application.DTOs;
using Colegio.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.API.Controllers
{
    [ApiController]
    [Route("api/assessment-types")]
    public class AssessmentTypesController : ControllerBase
    {
        private readonly IAssessmentService _svc;
        public AssessmentTypesController(IAssessmentService svc) => _svc = svc;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssessmentTypeRequestDTO dto)
            => Ok(await _svc.CreateAsync(dto));
    }
}
