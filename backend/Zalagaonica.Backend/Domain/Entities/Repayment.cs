using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Repayment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public string? Note { get; set; }

        // Veze
        public Guid? LoanId { get; set; }
        public Loan? Loan { get; set; }

        public Guid? ClientId { get; set; }
        public Client? Client { get; set; }

        public Guid? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
