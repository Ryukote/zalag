using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class LoanRepayment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        // Alias for compatibility
        public DateTime RepaymentDate
        {
            get => PaymentDate;
            set => PaymentDate = value;
        }

        public string? Note { get; set; }

        // Relacija prema kreditu
        public Guid LoanId { get; set; }
        public Loan? Loan { get; set; }
    }
}
