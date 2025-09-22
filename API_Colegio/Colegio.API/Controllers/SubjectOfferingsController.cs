using Colegio.Application.DTOs;
using Colegio.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.API.Controllers
{
    [ApiController]
    [Route("api/subject-offerings")]
    public class SubjectOfferingsController : ControllerBase
    {
        private readonly ISubjectOfferingService _svc;
        public SubjectOfferingsController(ISubjectOfferingService svc) => _svc = svc;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubjectOfferingRequestDTO dto)
        => Ok(await _svc.CreateAsync(dto));

        [HttpPost("{id:int}/close")]
        public async Task<IActionResult> Close([FromRoute] int id)
        {
            await _svc.CloseAsync(id);
            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var r = await _svc.GetAsync(id);
            return r is null ? NotFound() : Ok(r);
        }
    }
}
