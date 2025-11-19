using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CashRegisterTransaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [MaxLength(30)]
        public string? Type { get; set; } = "Deposit";

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
