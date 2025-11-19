using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Warehouse
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Location { get; set; }

        public Guid? WarehouseTypeId { get; set; }

        public WarehouseType? Type { get; set; }

        public ICollection<Article>? Articles { get; set; }
    }
}
