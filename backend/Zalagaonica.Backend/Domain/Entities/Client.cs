using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string IdCardNumber { get; set; } = string.Empty; // OIB or ID card number

        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Iban { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = "individual"; // 'legal' or 'individual'

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "active"; // 'active' or 'inactive'

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Legacy field for backwards compatibility
        public string? PhoneNumber { get; set; }

        // Navigacije
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Pledge>? Pledges { get; set; }
        public ICollection<PurchaseRecord>? Purchases { get; set; }
    }
}
