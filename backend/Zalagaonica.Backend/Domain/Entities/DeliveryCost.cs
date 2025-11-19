using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class DeliveryCost
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string Courier { get; set; } = string.Empty; // 'GLS', 'DPD', 'Hrvatska Pošta', 'Ostalo'

        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
