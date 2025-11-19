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
    public class PayrollController : ControllerBase
    {
        private readonly PayrollService _service;

        public PayrollController(PayrollService service)
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
        public async Task<IActionResult> Create([FromBody] CreatePayrollRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var payroll = new Payroll
            {
                Id = Guid.NewGuid(),
                EmployeeId = request.EmployeeId,
                Amount = request.Amount,
                PaymentDate = request.PaymentDate ?? DateTime.UtcNow,
                Status = request.Status ?? "Processed"
            };

            var created = await _service.CreateAsync(payroll);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePayrollRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.EmployeeId = request.EmployeeId;
            existing.Amount = request.Amount;
            existing.PaymentDate = request.PaymentDate ?? existing.PaymentDate;
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
    public class CreatePayrollRequest
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime? PaymentDate { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }
    }

    public class UpdatePayrollRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime? PaymentDate { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }
    }
}
