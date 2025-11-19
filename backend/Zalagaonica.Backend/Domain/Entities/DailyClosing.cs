using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class DailyClosing
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required, MaxLength(100)]
        public string CashierName { get; set; } = string.Empty;

        [Required]
        public decimal StartingCash { get; set; }

        [Required]
        public decimal TotalSales { get; set; }

        [Required]
        public decimal TotalExpenses { get; set; }

        [Required]
        public decimal CashInRegister { get; set; }

        public decimal ExpectedCash => StartingCash + TotalSales - TotalExpenses;

        public decimal Difference => CashInRegister - ExpectedCash;

        [Required]
        public bool IsClosed { get; set; } = false;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }
    }
}
