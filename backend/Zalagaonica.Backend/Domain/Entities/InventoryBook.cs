using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class InventoryBook
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required, MaxLength(50)]
        public string EntryNumber { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string ArticleCode { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string ArticleName { get; set; } = string.Empty;

        [Required]
        public int QuantitySold { get; set; }

        [Required]
        public decimal SalePrice { get; set; }

        [Required]
        public decimal TotalSale { get; set; }

        [Required, MaxLength(100)]
        public string Cashier { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
