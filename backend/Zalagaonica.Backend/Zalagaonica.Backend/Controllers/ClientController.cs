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
    public class ClientController : ControllerBase
    {
        private readonly ClientService _service;
        public ClientController(ClientService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var entity = await _service.GetByIdAsync(id);
            return entity == null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var client = new Client
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                City = request.City,
                Address = request.Address,
                IdCardNumber = request.IdCardNumber,
                Email = request.Email,
                Iban = request.Iban,
                Type = request.Type ?? "individual",
                Status = request.Status ?? "active",
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _service.CreateAsync(client);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = request.Name;
            existing.City = request.City;
            existing.Address = request.Address;
            existing.IdCardNumber = request.IdCardNumber;
            existing.Email = request.Email;
            existing.Iban = request.Iban;
            existing.Type = request.Type ?? existing.Type;
            existing.Status = request.Status ?? existing.Status;
            existing.PhoneNumber = request.PhoneNumber;
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
    public class CreateClientRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string IdCardNumber { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Iban { get; set; }

        [MaxLength(20)]
        public string? Type { get; set; }

        [MaxLength(20)]
        public string? Status { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class UpdateClientRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string IdCardNumber { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Iban { get; set; }

        [MaxLength(20)]
        public string? Type { get; set; }

        [MaxLength(20)]
        public string? Status { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
