using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(120)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(120)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; } = "Active";
    }
}
