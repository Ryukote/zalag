using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Position { get; set; }

        [MaxLength(120)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        public DateTime HiredDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
