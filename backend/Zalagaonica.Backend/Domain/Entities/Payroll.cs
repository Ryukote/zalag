using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Payroll
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? Status { get; set; } = "Processed";
    }
}
