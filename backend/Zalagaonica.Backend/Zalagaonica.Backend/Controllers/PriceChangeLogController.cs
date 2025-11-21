using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PriceChangeLogController : ControllerBase
    {
        private readonly PriceChangeLogService _service;

        public PriceChangeLogController(PriceChangeLogService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var logs = await _service.GetAllAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju log zapisa promjena cijena", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var log = await _service.GetByIdAsync(id);
                return log == null ? NotFound() : Ok(log);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju log zapisa", error = ex.Message });
            }
        }

        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> GetByArticle(Guid articleId)
        {
            try
            {
                var logs = await _service.GetByArticleIdAsync(articleId);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju log zapisa za artikl", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PriceChangeLog log)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var created = await _service.CreateAsync(log);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri kreiranju log zapisa", error = ex.Message });
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
                return StatusCode(500, new { message = "Greška pri brisanju log zapisa", error = ex.Message });
            }
        }
    }
}
