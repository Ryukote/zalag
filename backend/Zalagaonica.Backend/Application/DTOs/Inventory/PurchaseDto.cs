using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Inventory
{
    public class PurchaseRecordDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ID klijenta je obavezan")]
        public Guid ClientId { get; set; }

        [Required(ErrorMessage = "Ime klijenta je obavezno")]
        [MaxLength(200, ErrorMessage = "Ime klijenta ne može biti dulje od 200 znakova")]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Naziv predmeta je obavezan")]
        [MaxLength(300, ErrorMessage = "Naziv predmeta ne može biti dulji od 300 znakova")]
        public string ItemName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis predmeta je obavezan")]
        [MaxLength(1000, ErrorMessage = "Opis predmeta ne može biti dulji od 1000 znakova")]
        public string ItemDescription { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Procijenjena vrijednost ne može biti negativna")]
        public decimal EstimatedValue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Iznos kupnje ne može biti negativan")]
        public decimal PurchaseAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Ukupan iznos ne može biti negativan")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Datum kupnje je obavezan")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Datum plaćanja je obavezan")]
        public string PaymentDate { get; set; } = string.Empty;

        // Base64 encoded images
        public List<string> ItemImages { get; set; } = new();

        // Base64 encoded files (images or PDFs)
        public List<string> WarrantyFiles { get; set; } = new();
    }

    public class CreatePurchaseRecordDto
    {
        [Required(ErrorMessage = "ID klijenta je obavezan")]
        public Guid ClientId { get; set; }

        [Required(ErrorMessage = "Ime klijenta je obavezno")]
        [MaxLength(200, ErrorMessage = "Ime klijenta ne može biti dulje od 200 znakova")]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Naziv predmeta je obavezan")]
        [MaxLength(300, ErrorMessage = "Naziv predmeta ne može biti dulji od 300 znakova")]
        public string ItemName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis predmeta je obavezan")]
        [MaxLength(1000, ErrorMessage = "Opis predmeta ne može biti dulji od 1000 znakova")]
        public string ItemDescription { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Procijenjena vrijednost ne može biti negativna")]
        public decimal EstimatedValue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Iznos kupnje ne može biti negativan")]
        public decimal PurchaseAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Ukupan iznos ne može biti negativan")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Datum plaćanja je obavezan")]
        public string PaymentDate { get; set; } = string.Empty;

        // Base64 encoded images
        public List<string> ItemImages { get; set; } = new();

        // Base64 encoded files (images or PDFs)
        public List<string> WarrantyFiles { get; set; } = new();
    }
}
