using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PriceChangeLog
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required]
        public DateTime ChangeDate { get; set; }

        [Required, MaxLength(50)]
        public string ArticleCode { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string ArticleName { get; set; } = string.Empty;

        [Required]
        public decimal OldPrice { get; set; }

        [Required]
        public decimal NewPrice { get; set; }

        [Required, MaxLength(500)]
        public string Reason { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string ChangedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
