using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class InventoryCount
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string InventoryNumber { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [Required, MaxLength(50)]
        public string ArticleCode { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string ArticleName { get; set; } = string.Empty;

        [Required]
        public int BookQuantity { get; set; }

        [Required]
        public int PhysicalQuantity { get; set; }

        public int Difference => PhysicalQuantity - BookQuantity;

        [Required, MaxLength(100)]
        public string Warehouse { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, verified, approved

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
