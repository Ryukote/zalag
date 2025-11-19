using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Loan
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }
        [Required]
        public Guid ArticleId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Range(0, 100)]
        public decimal InterestRate { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; } = "Active";

        public Client? Client { get; set; }
        public Article? Article { get; set; }

        public virtual ICollection<LoanRepayment>? Repayments { get; set; }
    }
}
