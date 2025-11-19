using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Inventory
{
    public class ClientDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Ime klijenta je obavezno")]
        [MinLength(2, ErrorMessage = "Ime mora imati najmanje 2 znaka")]
        [MaxLength(200, ErrorMessage = "Ime ne može biti duže od 200 znakova")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Grad je obavezan")]
        [MaxLength(100, ErrorMessage = "Grad ne može biti dulji od 100 znakova")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adresa je obavezna")]
        [MaxLength(200, ErrorMessage = "Adresa ne može biti dulja od 200 znakova")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Broj osobne iskaznice / OIB je obavezan")]
        [MaxLength(50, ErrorMessage = "Broj osobne iskaznice / OIB ne može biti dulji od 50 znakova")]
        public string IdCardNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Nevažeća email adresa")]
        [MaxLength(255, ErrorMessage = "Email ne može biti dulji od 255 znakova")]
        public string? Email { get; set; }

        [MaxLength(50, ErrorMessage = "IBAN ne može biti dulji od 50 znakova")]
        public string? Iban { get; set; }

        [Required(ErrorMessage = "Tip klijenta je obavezan")]
        public string Type { get; set; } = "individual"; // 'legal' ili 'individual'

        [Required(ErrorMessage = "Status je obavezan")]
        public string Status { get; set; } = "active"; // 'active' ili 'inactive'

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateClientDto
    {
        [Required(ErrorMessage = "Ime klijenta je obavezno")]
        [MinLength(2, ErrorMessage = "Ime mora imati najmanje 2 znaka")]
        [MaxLength(200, ErrorMessage = "Ime ne može biti duže od 200 znakova")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Grad je obavezan")]
        [MaxLength(100, ErrorMessage = "Grad ne može biti dulji od 100 znakova")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adresa je obavezna")]
        [MaxLength(200, ErrorMessage = "Adresa ne može biti dulja od 200 znakova")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Broj osobne iskaznice / OIB je obavezan")]
        [MaxLength(50, ErrorMessage = "Broj osobne iskaznice / OIB ne može biti dulji od 50 znakova")]
        public string IdCardNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Nevažeća email adresa")]
        [MaxLength(255, ErrorMessage = "Email ne može biti dulji od 255 znakova")]
        public string? Email { get; set; }

        [MaxLength(50, ErrorMessage = "IBAN ne može biti dulji od 50 znakova")]
        public string? Iban { get; set; }

        [Required(ErrorMessage = "Tip klijenta je obavezan")]
        public string Type { get; set; } = "individual"; // 'legal' ili 'individual'
    }

    public class UpdateClientDto
    {
        [Required(ErrorMessage = "ID je obavezan")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Ime klijenta je obavezno")]
        [MinLength(2, ErrorMessage = "Ime mora imati najmanje 2 znaka")]
        [MaxLength(200, ErrorMessage = "Ime ne može biti duže od 200 znakova")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Grad je obavezan")]
        [MaxLength(100, ErrorMessage = "Grad ne može biti dulji od 100 znakova")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adresa je obavezna")]
        [MaxLength(200, ErrorMessage = "Adresa ne može biti dulja od 200 znakova")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Broj osobne iskaznice / OIB je obavezan")]
        [MaxLength(50, ErrorMessage = "Broj osobne iskaznice / OIB ne može biti dulji od 50 znakova")]
        public string IdCardNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Nevažeća email adresa")]
        [MaxLength(255, ErrorMessage = "Email ne može biti dulji od 255 znakova")]
        public string? Email { get; set; }

        [MaxLength(50, ErrorMessage = "IBAN ne može biti dulji od 50 znakova")]
        public string? Iban { get; set; }

        [Required(ErrorMessage = "Status je obavezan")]
        public string Status { get; set; } = "active"; // 'active' ili 'inactive'
    }
}
