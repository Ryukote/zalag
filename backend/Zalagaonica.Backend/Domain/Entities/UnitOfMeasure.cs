using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UnitOfMeasure
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
