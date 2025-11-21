using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryBookController : ControllerBase
    {
        private readonly InventoryBookService _service;

        public InventoryBookController(InventoryBookService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var entries = await _service.GetAllAsync();
                return Ok(entries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju KPO unosa", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var entry = await _service.GetByIdAsync(id);
                return entry == null ? NotFound() : Ok(entry);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju KPO unosa", error = ex.Message });
            }
        }

        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> GetByArticle(Guid articleId)
        {
            try
            {
                var entries = await _service.GetByArticleIdAsync(articleId);
                return Ok(entries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju KPO unosa za artikl", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryBook entry)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var created = await _service.CreateAsync(entry);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri kreiranju KPO unosa", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] InventoryBook entry)
        {
            try
            {
                if (id != entry.Id) return BadRequest("ID ne odgovara");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var success = await _service.UpdateAsync(entry);
                return success ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri ažuriranju KPO unosa", error = ex.Message });
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
                return StatusCode(500, new { message = "Greška pri brisanju KPO unosa", error = ex.Message });
            }
        }
    }
}
