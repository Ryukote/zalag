using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class WarehouseCard
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? ArticleId { get; set; }

        [Required, MaxLength(50)]
        public string ArticleCode { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string ArticleName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Warehouse { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? DocumentType { get; set; }

        [MaxLength(50)]
        public string? DocumentNumber { get; set; }

        public int InQuantity { get; set; }
        public int OutQuantity { get; set; }
        public int Balance { get; set; }

        [Required]
        public int CurrentStock { get; set; }

        [Required]
        public int ReservedStock { get; set; }

        public int AvailableStock => CurrentStock - ReservedStock;

        [Required, MaxLength(20)]
        public string UnitOfMeasure { get; set; } = "kom";

        [Required]
        public DateTime LastMovement { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
