using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class OutputDocument
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string ClientName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string DocumentNumber { get; set; } = string.Empty;

        public DateTime DocumentDate { get; set; } = DateTime.UtcNow;

        [Range(0, 999999999)]
        public decimal TotalValue { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "otvoren"; // 'otvoren' or 'proknjižen'

        [MaxLength(50)]
        public string DocumentType { get; set; } = "RAČUN"; // 'RAČUN', 'OTPREMNICA'

        public int Year { get; set; }

        [MaxLength(100)]
        public string? Operator { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        public bool IsPosted { get; set; } = false;

        public decimal TotalWithTax { get; set; }

        public decimal PretaxAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
