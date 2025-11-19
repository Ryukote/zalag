using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Inventory
{
    public class SaleDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ID artikla je obavezan")]
        public Guid ArticleId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Količina mora biti veća od 0")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Prodajna cijena ne može biti negativna")]
        public decimal SalePrice { get; set; }

        [Required(ErrorMessage = "Datum prodaje je obavezan")]
        public DateTime Date { get; set; }

        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
    }

    public class CreateSaleDto
    {
        [Required(ErrorMessage = "ID artikla je obavezan")]
        public Guid ArticleId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Količina mora biti veća od 0")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Prodajna cijena ne može biti negativna")]
        public decimal SalePrice { get; set; }
    }
}
