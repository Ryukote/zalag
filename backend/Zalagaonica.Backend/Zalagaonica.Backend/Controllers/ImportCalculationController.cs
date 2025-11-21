using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ImportCalculationController : ControllerBase
    {
        private readonly ImportCalculationService _service;

        public ImportCalculationController(ImportCalculationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var calculations = await _service.GetAllAsync();
                return Ok(calculations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju kalkulacija", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var calculation = await _service.GetByIdAsync(id);
                return calculation == null ? NotFound() : Ok(calculation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju kalkulacije", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ImportCalculation calculation)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var created = await _service.CreateAsync(calculation);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri kreiranju kalkulacije", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ImportCalculation calculation)
        {
            try
            {
                if (id != calculation.Id) return BadRequest("ID ne odgovara");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var success = await _service.UpdateAsync(calculation);
                return success ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri ažuriranju kalkulacije", error = ex.Message });
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
                return StatusCode(500, new { message = "Greška pri brisanju kalkulacije", error = ex.Message });
            }
        }
    }
}
