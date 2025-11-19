using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PurchaseRecord
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string ItemDescription { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal EstimatedValue { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PurchaseAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string PaymentDate { get; set; } = string.Empty;

        // Store images as JSON array of base64 strings
        public string ItemImagesJson { get; set; } = "[]";

        // Store warranty files as JSON array of base64 strings
        public string WarrantyFilesJson { get; set; } = "[]";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Client? Client { get; set; }
    }
}
