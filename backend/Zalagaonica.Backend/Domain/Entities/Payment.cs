using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Method { get; set; } = string.Empty; // gotovina, kartica, transakcija...

        public string? Description { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Veze
        public Guid? ClientId { get; set; }
        public Client? Client { get; set; }

        public Guid? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
