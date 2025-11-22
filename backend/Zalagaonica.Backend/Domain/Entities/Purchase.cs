using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Purchase
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public decimal Amount { get; set; }

        [MaxLength(200)]
        public string? ClientName { get; set; }

        [MaxLength(300)]
        public string? ItemName { get; set; }

        // Veze
        public Guid ArticleId { get; set; }
        public Article? Article { get; set; }

        public Guid? ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
