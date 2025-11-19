using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CustomerDebt
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string CustomerCode { get; set; } = string.Empty;

        [Required]
        public decimal TotalDebt { get; set; }

        [Required]
        public decimal Paid { get; set; }

        public decimal Remaining => TotalDebt - Paid;

        [Required]
        public DateTime DueDate { get; set; }

        public int DaysPastDue
        {
            get
            {
                if (DateTime.UtcNow <= DueDate) return 0;
                return (DateTime.UtcNow - DueDate).Days;
            }
        }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "current"; // current, overdue, critical

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
