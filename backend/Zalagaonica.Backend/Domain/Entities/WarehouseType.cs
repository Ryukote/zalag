using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class WarehouseType
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    }
}
