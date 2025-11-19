using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _service;

        public ReservationController(ReservationService service)
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
        public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                ReservationDate = request.ReservationDate ?? DateTime.UtcNow,
                DepositAmount = request.DepositAmount,
                ClientId = request.ClientId,
                ArticleId = request.ArticleId,
                StatusId = request.StatusId
            };

            var created = await _service.CreateAsync(reservation);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReservationRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.ReservationDate = request.ReservationDate ?? existing.ReservationDate;
            existing.DepositAmount = request.DepositAmount;
            existing.ClientId = request.ClientId;
            existing.ArticleId = request.ArticleId;
            existing.StatusId = request.StatusId;

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
    public class CreateReservationRequest
    {
        public DateTime? ReservationDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DepositAmount { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public Guid ArticleId { get; set; }

        [Required]
        public Guid StatusId { get; set; }
    }

    public class UpdateReservationRequest
    {
        [Required]
        public Guid Id { get; set; }

        public DateTime? ReservationDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DepositAmount { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public Guid ArticleId { get; set; }

        [Required]
        public Guid StatusId { get; set; }
    }
}
