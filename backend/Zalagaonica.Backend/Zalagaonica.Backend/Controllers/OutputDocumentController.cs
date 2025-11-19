using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutputDocumentController : ControllerBase
    {
        private readonly OutputDocumentService _service;

        public OutputDocumentController(OutputDocumentService service)
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
        public async Task<IActionResult> Create([FromBody] CreateOutputDocumentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var document = new OutputDocument
            {
                ClientName = request.ClientName,
                DocumentNumber = request.DocumentNumber,
                DocumentDate = request.DocumentDate ?? DateTime.UtcNow,
                TotalValue = request.TotalValue,
                Status = request.Status ?? "otvoren",
                DocumentType = request.DocumentType ?? "RAČUN",
                Operator = request.Operator,
                Note = request.Note,
                IsPosted = request.IsPosted ?? false,
                TotalWithTax = request.TotalWithTax,
                PretaxAmount = request.PretaxAmount
            };

            var created = await _service.CreateAsync(document);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOutputDocumentRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.ClientName = request.ClientName;
            existing.DocumentNumber = request.DocumentNumber;
            existing.DocumentDate = request.DocumentDate ?? existing.DocumentDate;
            existing.TotalValue = request.TotalValue;
            existing.Status = request.Status ?? existing.Status;
            existing.DocumentType = request.DocumentType ?? existing.DocumentType;
            existing.Operator = request.Operator;
            existing.Note = request.Note;
            existing.IsPosted = request.IsPosted ?? existing.IsPosted;
            existing.TotalWithTax = request.TotalWithTax;
            existing.PretaxAmount = request.PretaxAmount;

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

    public class CreateOutputDocumentRequest
    {
        [Required]
        [MaxLength(200)]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string DocumentNumber { get; set; } = string.Empty;

        public DateTime? DocumentDate { get; set; }

        [Range(0, 999999999)]
        public decimal TotalValue { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        [MaxLength(50)]
        public string? DocumentType { get; set; }

        [MaxLength(100)]
        public string? Operator { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        public bool? IsPosted { get; set; }

        public decimal TotalWithTax { get; set; }

        public decimal PretaxAmount { get; set; }
    }

    public class UpdateOutputDocumentRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string DocumentNumber { get; set; } = string.Empty;

        public DateTime? DocumentDate { get; set; }

        [Range(0, 999999999)]
        public decimal TotalValue { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        [MaxLength(50)]
        public string? DocumentType { get; set; }

        [MaxLength(100)]
        public string? Operator { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        public bool? IsPosted { get; set; }

        public decimal TotalWithTax { get; set; }

        public decimal PretaxAmount { get; set; }
    }
}
