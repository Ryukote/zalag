using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ItemData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ArticleId { get; set; }
        public Article? Article { get; set; }

        [Required]
        public string Key { get; set; } = string.Empty;

        public string? Value { get; set; }
    }
}
