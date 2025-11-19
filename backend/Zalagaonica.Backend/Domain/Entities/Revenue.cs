using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Revenue
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Source { get; set; } = string.Empty; // npr. prodaja, kamata, najam itd.

        public string? Description { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Opcionalno povezivanje s klijentom ili zaposlenikom
        public Guid? ClientId { get; set; }
        public Client? Client { get; set; }

        public Guid? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
