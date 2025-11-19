using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _service;

        public CustomerController(CustomerService service)
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
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Status = request.Status ?? "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _service.CreateAsync(customer);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.FullName = request.FullName;
            existing.Email = request.Email;
            existing.PhoneNumber = request.PhoneNumber;
            existing.Address = request.Address;
            existing.Status = request.Status ?? existing.Status;
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
    public class CreateCustomerRequest
    {
        [Required]
        [MaxLength(120)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(120)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }
    }

    public class UpdateCustomerRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(120)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }
    }
}
