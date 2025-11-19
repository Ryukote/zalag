using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryCostController : ControllerBase
    {
        private readonly DeliveryCostService _service;

        public DeliveryCostController(DeliveryCostService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var entity = await _service.GetByIdAsync(id);
            return entity == null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeliveryCostRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var deliveryCost = new DeliveryCost
            {
                Id = Guid.NewGuid(),
                Date = request.Date ?? DateTime.UtcNow,
                Courier = request.Courier,
                TrackingNumber = request.TrackingNumber,
                Description = request.Description,
                Cost = request.Cost,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _service.CreateAsync(deliveryCost);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDeliveryCostRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Date = request.Date ?? existing.Date;
            existing.Courier = request.Courier;
            existing.TrackingNumber = request.TrackingNumber;
            existing.Description = request.Description;
            existing.Cost = request.Cost;
            existing.UpdatedAt = DateTime.UtcNow;

            var success = await _service.UpdateAsync(existing);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }

    // DTOs matching frontend exactly
    public class CreateDeliveryCostRequest
    {
        public DateTime? Date { get; set; }

        [Required]
        [MaxLength(50)]
        public string Courier { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }
    }

    public class UpdateDeliveryCostRequest
    {
        [Required]
        public Guid Id { get; set; }

        public DateTime? Date { get; set; }

        [Required]
        [MaxLength(50)]
        public string Courier { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }
    }
}
