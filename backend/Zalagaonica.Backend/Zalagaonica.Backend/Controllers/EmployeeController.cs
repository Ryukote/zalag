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
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeeController(EmployeeService service)
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
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Position = request.Position,
                Email = request.Email,
                Phone = request.Phone,
                HiredDate = request.HiredDate ?? DateTime.UtcNow,
                IsActive = request.IsActive ?? true
            };

            var created = await _service.CreateAsync(employee);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.FullName = request.FullName;
            existing.Position = request.Position;
            existing.Email = request.Email;
            existing.Phone = request.Phone;
            existing.HiredDate = request.HiredDate ?? existing.HiredDate;
            existing.IsActive = request.IsActive ?? existing.IsActive;

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
    public class CreateEmployeeRequest
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Position { get; set; }

        [MaxLength(120)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        public DateTime? HiredDate { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UpdateEmployeeRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Position { get; set; }

        [MaxLength(120)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        public DateTime? HiredDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
