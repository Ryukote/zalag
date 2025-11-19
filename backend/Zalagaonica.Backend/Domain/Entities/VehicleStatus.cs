using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class VehicleStatus
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Description { get; set; }
    }
}
