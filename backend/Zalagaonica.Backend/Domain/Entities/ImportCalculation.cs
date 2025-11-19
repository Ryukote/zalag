using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ImportCalculation
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required]
        public DateTime DocumentDate { get; set; }

        [Required, MaxLength(200)]
        public string SupplierName { get; set; } = string.Empty;

        [Required]
        public decimal TotalAmount { get; set; }

        [Required, MaxLength(10)]
        public string Currency { get; set; } = "EUR";

        [Required]
        public decimal ExchangeRate { get; set; }

        [Required]
        public decimal TotalInLocalCurrency { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "draft"; // draft, confirmed, closed

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
