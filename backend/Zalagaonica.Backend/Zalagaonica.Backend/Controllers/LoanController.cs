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
    public class LoanController : ControllerBase
    {
        private readonly LoanService _service;

        public LoanController(LoanService service)
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
        public async Task<IActionResult> Create([FromBody] CreateLoanRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                ClientId = request.ClientId,
                ArticleId = request.ArticleId,
                Amount = request.Amount,
                InterestRate = request.InterestRate,
                StartDate = request.StartDate ?? DateTime.UtcNow,
                EndDate = request.EndDate,
                Status = request.Status ?? "Active"
            };

            var created = await _service.CreateAsync(loan);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLoanRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.ClientId = request.ClientId;
            existing.ArticleId = request.ArticleId;
            existing.Amount = request.Amount;
            existing.InterestRate = request.InterestRate;
            existing.StartDate = request.StartDate ?? existing.StartDate;
            existing.EndDate = request.EndDate;
            existing.Status = request.Status ?? existing.Status;

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
    public class CreateLoanRequest
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public Guid ArticleId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Range(0, 100)]
        public decimal InterestRate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }
    }

    public class UpdateLoanRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public Guid ArticleId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Range(0, 100)]
        public decimal InterestRate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }
    }
}
