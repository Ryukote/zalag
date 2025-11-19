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
    public class VehicleController : ControllerBase
    {
        private readonly VehicleService _service;

        public VehicleController(VehicleService service)
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
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
                PlateNumber = request.PlateNumber,
                ClientId = request.ClientId,
                Status = request.Status ?? "Available"
            };

            var created = await _service.CreateAsync(vehicle);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Make = request.Make;
            existing.Model = request.Model;
            existing.Year = request.Year;
            existing.PlateNumber = request.PlateNumber;
            existing.ClientId = request.ClientId;
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
    public class CreateVehicleRequest
    {
        [Required]
        [MaxLength(100)]
        public string Make { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [MaxLength(50)]
        public string? PlateNumber { get; set; }

        public Guid? ClientId { get; set; }

        [MaxLength(30)]
        public string? Status { get; set; }
    }

    public class UpdateVehicleRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Make { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [MaxLength(50)]
        public string? PlateNumber { get; set; }

        public Guid? ClientId { get; set; }

        [MaxLength(30)]
        public string? Status { get; set; }
    }
}
