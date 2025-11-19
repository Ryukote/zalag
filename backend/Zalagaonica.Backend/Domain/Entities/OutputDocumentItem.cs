using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class OutputDocumentItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid DocumentId { get; set; }

        [Required]
        public Guid ArticleId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public decimal Total => Quantity * Price;
    }
}
