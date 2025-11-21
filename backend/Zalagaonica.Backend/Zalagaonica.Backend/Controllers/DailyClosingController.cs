using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DailyClosingController : ControllerBase
    {
        private readonly DailyClosingService _service;

        public DailyClosingController(DailyClosingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var closings = await _service.GetAllAsync();
                return Ok(closings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju dnevnih zatvaranja", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var closing = await _service.GetByIdAsync(id);
                return closing == null ? NotFound() : Ok(closing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju dnevnog zatvaranja", error = ex.Message });
            }
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            try
            {
                var closing = await _service.GetByDateAsync(date);
                return closing == null ? NotFound() : Ok(closing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju dnevnog zatvaranja", error = ex.Message });
            }
        }

        [HttpGet("check/{date}")]
        public async Task<IActionResult> IsDateClosed(DateTime date)
        {
            try
            {
                var isClosed = await _service.IsDateClosedAsync(date);
                return Ok(new { date, isClosed });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri provjeri statusa zatvaranja", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DailyClosing closing)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var created = await _service.CreateAsync(closing);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri kreiranju dnevnog zatvaranja", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DailyClosing closing)
        {
            try
            {
                if (id != closing.Id) return BadRequest("ID ne odgovara");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var success = await _service.UpdateAsync(closing);
                return success ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri ažuriranju dnevnog zatvaranja", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _service.DeleteAsync(id);
                return success ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri brisanju dnevnog zatvaranja", error = ex.Message });
            }
        }
    }
}
