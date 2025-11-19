using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;

        public decimal DepositAmount { get; set; }

        // Veze
        public Guid ClientId { get; set; }
        public Client? Client { get; set; }

        public Guid ArticleId { get; set; }
        public Article? Article { get; set; }

        public Guid? ReservationStatusId { get; set; }
        public ReservationStatus? ReservationStatus { get; set; }

        public ReservationStatus? Status { get; set; }
        public Guid StatusId { get; set; }
    }
}
