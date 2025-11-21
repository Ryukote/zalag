using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryCountController : ControllerBase
    {
        private readonly InventoryCountService _service;

        public InventoryCountController(InventoryCountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var counts = await _service.GetAllAsync();
                return Ok(counts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju inventura", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var count = await _service.GetByIdAsync(id);
                return count == null ? NotFound() : Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju inventure", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryCount count)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var created = await _service.CreateAsync(count);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri kreiranju inventure", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] InventoryCount count)
        {
            try
            {
                if (id != count.Id) return BadRequest("ID ne odgovara");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var success = await _service.UpdateAsync(count);
                return success ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri ažuriranju inventure", error = ex.Message });
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
                return StatusCode(500, new { message = "Greška pri brisanju inventure", error = ex.Message });
            }
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            try
            {
                var success = await _service.ApproveAsync(id);
                return success ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri odobravanju inventure", error = ex.Message });
            }
        }
    }
}
