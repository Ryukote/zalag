using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ReservationStatus
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
