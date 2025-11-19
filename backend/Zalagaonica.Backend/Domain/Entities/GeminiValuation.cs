using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class GeminiValuation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ArticleId { get; set; }
        public Article? Article { get; set; }

        public decimal EstimatedValue { get; set; }

        public string? ModelName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
