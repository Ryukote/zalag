using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Sale
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        // Alias for compatibility
        public DateTime Date
        {
            get => SaleDate;
            set => SaleDate = value;
        }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [MaxLength(50)]
        public string? InvoiceNumber { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(200)]
        public string? CustomerName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Veze
        [Required]
        public Guid ArticleId { get; set; }
        public Article? Article { get; set; }

        public Guid? ClientId { get; set; }
        public Client? Client { get; set; }

        // Alias for compatibility
        public Guid? CustomerId
        {
            get => ClientId;
            set => ClientId = value;
        }

        public Guid? UserId { get; set; }
    }
}
