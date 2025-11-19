using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Vehicle
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Make { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [MaxLength(50)]
        public string? PlateNumber { get; set; }

        public Guid? ClientId { get; set; }

        [MaxLength(30)]
        public string? Status { get; set; } = "Available";
    }
}
