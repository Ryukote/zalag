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

        public string? Note { get; set; }

        // Relacija prema kreditu
        public Guid LoanId { get; set; }
        public Loan? Loan { get; set; }
    }
}
