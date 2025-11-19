using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Inventory
{
    public class PledgeDto
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

        [Range(0, double.MaxValue, ErrorMessage = "Iznos pozajmice ne može biti negativan")]
        public decimal LoanAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Iznos povrata ne može biti negativan")]
        public decimal ReturnAmount { get; set; }

        [Range(1, 365, ErrorMessage = "Razdoblje mora biti između 1 i 365 dana")]
        public int Period { get; set; }

        [Required(ErrorMessage = "Datum zaloga je obavezan")]
        public DateTime PledgeDate { get; set; }

        [Required(ErrorMessage = "Rok za otkup je obavezan")]
        public DateTime RedeemDeadline { get; set; }

        public bool Redeemed { get; set; }

        public bool Forfeited { get; set; }

        // Base64 encoded images
        public List<string> ItemImages { get; set; } = new();

        // Base64 encoded files (images or PDFs)
        public List<string> WarrantyFiles { get; set; } = new();
    }

    public class CreatePledgeDto
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

        [Range(0, double.MaxValue, ErrorMessage = "Iznos pozajmice ne može biti negativan")]
        public decimal LoanAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Iznos povrata ne može biti negativan")]
        public decimal ReturnAmount { get; set; }

        [Range(1, 365, ErrorMessage = "Razdoblje mora biti između 1 i 365 dana")]
        public int Period { get; set; }

        // Base64 encoded images
        public List<string> ItemImages { get; set; } = new();

        // Base64 encoded files (images or PDFs)
        public List<string> WarrantyFiles { get; set; } = new();
    }

    public class UpdatePledgeDto
    {
        [Required(ErrorMessage = "ID je obavezan")]
        public Guid Id { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Iznos pozajmice ne može biti negativan")]
        public decimal? LoanAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Iznos povrata ne može biti negativan")]
        public decimal? ReturnAmount { get; set; }

        [Range(1, 365, ErrorMessage = "Razdoblje mora biti između 1 i 365 dana")]
        public int? Period { get; set; }

        public DateTime? RedeemDeadline { get; set; }
    }
}
