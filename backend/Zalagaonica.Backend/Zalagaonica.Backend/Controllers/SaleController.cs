using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly SaleService _service;

        public SaleController(SaleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var sales = await _service.GetAllAsync();
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju prodaja", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var sale = await _service.GetByIdAsync(id);
                return sale == null ? NotFound() : Ok(sale);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju prodaje", error = ex.Message });
            }
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var sales = await _service.GetByDateRangeAsync(startDate, endDate);
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju prodaja po datumu", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Sale sale)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var created = await _service.CreateAsync(sale);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri kreiranju prodaje", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Sale sale)
        {
            try
            {
                if (id != sale.Id) return BadRequest("ID ne odgovara");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var success = await _service.UpdateAsync(sale);
                return success ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri ažuriranju prodaje", error = ex.Message });
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
                return StatusCode(500, new { message = "Greška pri brisanju prodaje", error = ex.Message });
            }
        }
    }
}
