using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Inventory
{
    public class ArticleDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Naziv artikla je obavezan")]
        [MinLength(2, ErrorMessage = "Naziv mora imati najmanje 2 znaka")]
        [MaxLength(300, ErrorMessage = "Naziv ne može biti dulji od 300 znakova")]
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Zaliha ne može biti negativna")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Jedinica mjere je obavezna")]
        [MaxLength(20, ErrorMessage = "Jedinica mjere ne može biti dulja od 20 znakova")]
        public string UnitOfMeasure { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Maloprodajna cijena ne može biti negativna")]
        public decimal RetailPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Porezna stopa mora biti između 0 i 100")]
        public decimal TaxRate { get; set; }

        [MaxLength(200, ErrorMessage = "Naziv dobavljača ne može biti dulji od 200 znakova")]
        public string? SupplierName { get; set; }

        [MaxLength(100, ErrorMessage = "Grupa ne može biti dulja od 100 znakova")]
        public string? Group { get; set; }

        [Required(ErrorMessage = "Status je obavezan")]
        public string Status { get; set; } = "available"; // 'available' ili 'sold'

        public SaleInfoDto? SaleInfo { get; set; }

        [Required(ErrorMessage = "Skladište je obavezno")]
        public string Warehouse { get; set; } = "main"; // 'main' ili 'pledge'
    }

    public class SaleInfoDto
    {
        [Range(0, double.MaxValue, ErrorMessage = "Prodajna cijena ne može biti negativna")]
        public decimal SalePrice { get; set; }

        [Required(ErrorMessage = "Datum prodaje je obavezan")]
        public DateTime SaleDate { get; set; }

        [Required(ErrorMessage = "Ime kupca je obavezno")]
        [MaxLength(200, ErrorMessage = "Ime kupca ne može biti dulje od 200 znakova")]
        public string CustomerName { get; set; } = string.Empty;

        public Guid? CustomerId { get; set; }
    }

    public class CreateArticleDto
    {
        [Required(ErrorMessage = "Naziv artikla je obavezan")]
        [MinLength(2, ErrorMessage = "Naziv mora imati najmanje 2 znaka")]
        [MaxLength(300, ErrorMessage = "Naziv ne može biti dulji od 300 znakova")]
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Zaliha ne može biti negativna")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Jedinica mjere je obavezna")]
        [MaxLength(20, ErrorMessage = "Jedinica mjere ne može biti dulja od 20 znakova")]
        public string UnitOfMeasure { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Maloprodajna cijena ne može biti negativna")]
        public decimal RetailPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Porezna stopa mora biti između 0 i 100")]
        public decimal TaxRate { get; set; }

        [MaxLength(200, ErrorMessage = "Naziv dobavljača ne može biti dulji od 200 znakova")]
        public string? SupplierName { get; set; }

        [MaxLength(100, ErrorMessage = "Grupa ne može biti dulja od 100 znakova")]
        public string? Group { get; set; }

        [Required(ErrorMessage = "Skladište je obavezno")]
        public string Warehouse { get; set; } = "main"; // 'main' ili 'pledge'
    }

    public class UpdateArticleDto
    {
        [Required(ErrorMessage = "ID je obavezan")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Naziv artikla je obavezan")]
        [MinLength(2, ErrorMessage = "Naziv mora imati najmanje 2 znaka")]
        [MaxLength(300, ErrorMessage = "Naziv ne može biti dulji od 300 znakova")]
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Zaliha ne može biti negativna")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Jedinica mjere je obavezna")]
        [MaxLength(20, ErrorMessage = "Jedinica mjere ne može biti dulja od 20 znakova")]
        public string UnitOfMeasure { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Maloprodajna cijena ne može biti negativna")]
        public decimal RetailPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Porezna stopa mora biti između 0 i 100")]
        public decimal TaxRate { get; set; }

        [MaxLength(200, ErrorMessage = "Naziv dobavljača ne može biti dulji od 200 znakova")]
        public string? SupplierName { get; set; }

        [MaxLength(100, ErrorMessage = "Grupa ne može biti dulja od 100 znakova")]
        public string? Group { get; set; }

        [Required(ErrorMessage = "Status je obavezan")]
        public string Status { get; set; } = "available"; // 'available' ili 'sold'

        [Required(ErrorMessage = "Skladište je obavezno")]
        public string Warehouse { get; set; } = "main"; // 'main' ili 'pledge'
    }
}
