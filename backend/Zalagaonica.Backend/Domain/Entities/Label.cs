using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Label
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string ArticleCode { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string ArticleName { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string? Barcode { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
