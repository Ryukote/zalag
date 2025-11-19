using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService _service;

        public ArticleController(ArticleService service)
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
        public async Task<IActionResult> Create([FromBody] CreateArticleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var article = new Article
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                PurchasePrice = request.PurchasePrice,
                RetailPrice = request.RetailPrice,
                SalePrice = request.SalePrice,
                TaxRate = request.TaxRate,
                Stock = request.Stock,
                UnitOfMeasureCode = request.UnitOfMeasureCode,
                SupplierName = request.SupplierName,
                Group = request.Group,
                Status = request.Status ?? "available",
                WarehouseType = request.WarehouseType ?? "main",
                SaleInfoPrice = request.SaleInfoPrice,
                SaleInfoDate = request.SaleInfoDate,
                SaleInfoCustomerName = request.SaleInfoCustomerName,
                SaleInfoCustomerId = request.SaleInfoCustomerId,
                WarehouseId = request.WarehouseId,
                UnitOfMeasureId = request.UnitOfMeasureId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _service.CreateAsync(article);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateArticleRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = request.Name;
            existing.Description = request.Description ?? existing.Description;
            existing.PurchasePrice = request.PurchasePrice;
            existing.RetailPrice = request.RetailPrice;
            existing.SalePrice = request.SalePrice;
            existing.TaxRate = request.TaxRate;
            existing.Stock = request.Stock;
            existing.UnitOfMeasureCode = request.UnitOfMeasureCode;
            existing.SupplierName = request.SupplierName;
            existing.Group = request.Group;
            existing.Status = request.Status ?? existing.Status;
            existing.WarehouseType = request.WarehouseType ?? existing.WarehouseType;
            existing.SaleInfoPrice = request.SaleInfoPrice;
            existing.SaleInfoDate = request.SaleInfoDate;
            existing.SaleInfoCustomerName = request.SaleInfoCustomerName;
            existing.SaleInfoCustomerId = request.SaleInfoCustomerId;
            existing.WarehouseId = request.WarehouseId;
            existing.UnitOfMeasureId = request.UnitOfMeasureId;
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
    public class CreateArticleRequest
    {
        [Required]
        [MinLength(2)]
        [MaxLength(300)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PurchasePrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal RetailPrice { get; set; }

        public decimal? SalePrice { get; set; }

        [Range(0, 100)]
        public decimal TaxRate { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        [MaxLength(20)]
        public string UnitOfMeasureCode { get; set; } = "KOM";

        [MaxLength(200)]
        public string? SupplierName { get; set; }

        [MaxLength(100)]
        public string? Group { get; set; }

        public string? Status { get; set; }

        public string? WarehouseType { get; set; }

        // Sale information fields
        public decimal? SaleInfoPrice { get; set; }
        public DateTime? SaleInfoDate { get; set; }

        [MaxLength(200)]
        public string? SaleInfoCustomerName { get; set; }
        public Guid? SaleInfoCustomerId { get; set; }

        // Legacy fields
        public Guid? WarehouseId { get; set; }
        public Guid? UnitOfMeasureId { get; set; }
    }

    public class UpdateArticleRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(300)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PurchasePrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal RetailPrice { get; set; }

        public decimal? SalePrice { get; set; }

        [Range(0, 100)]
        public decimal TaxRate { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        [MaxLength(20)]
        public string UnitOfMeasureCode { get; set; } = "KOM";

        [MaxLength(200)]
        public string? SupplierName { get; set; }

        [MaxLength(100)]
        public string? Group { get; set; }

        public string? Status { get; set; }

        public string? WarehouseType { get; set; }

        // Sale information fields
        public decimal? SaleInfoPrice { get; set; }
        public DateTime? SaleInfoDate { get; set; }

        [MaxLength(200)]
        public string? SaleInfoCustomerName { get; set; }
        public Guid? SaleInfoCustomerId { get; set; }

        // Legacy fields
        public Guid? WarehouseId { get; set; }
        public Guid? UnitOfMeasureId { get; set; }
    }
}
