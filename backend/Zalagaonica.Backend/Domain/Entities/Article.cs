using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Article
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(300)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal PurchasePrice { get; set; }

        public decimal RetailPrice { get; set; }

        public decimal? SalePrice { get; set; }

        [Range(0, 100)]
        public decimal TaxRate { get; set; }

        public int Stock { get; set; } = 0;

        [MaxLength(20)]
        public string UnitOfMeasureCode { get; set; } = "KOM"; // 'KOM', 'KG', 'L', etc.

        [MaxLength(200)]
        public string? SupplierName { get; set; }

        [MaxLength(100)]
        public string? Group { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "available"; // 'available' or 'sold'

        [Required]
        [MaxLength(20)]
        public string WarehouseType { get; set; } = "main"; // 'main' or 'pledge'

        // Sale information (stored as JSON if needed, or separate fields)
        public decimal? SaleInfoPrice { get; set; }
        public DateTime? SaleInfoDate { get; set; }
        [MaxLength(200)]
        public string? SaleInfoCustomerName { get; set; }
        public Guid? SaleInfoCustomerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Legacy navigacija (for backwards compatibility)
        public Guid? WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }

        public Guid? UnitOfMeasureId { get; set; }
        public UnitOfMeasure? UnitOfMeasure { get; set; }
    }
}
