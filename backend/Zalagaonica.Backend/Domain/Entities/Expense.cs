using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Expense
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Type { get; set; } = string.Empty; // npr. "Najam", "Održavanje", "Račun"

        [Required]
        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Alias for compatibility
        public DateTime ExpenseDate
        {
            get => Date;
            set => Date = value;
        }

        // Veze
        public Guid? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
