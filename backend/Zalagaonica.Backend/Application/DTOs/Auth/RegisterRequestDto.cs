using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        [MinLength(3, ErrorMessage = "Korisničko ime mora imati najmanje 3 znaka")]
        [MaxLength(100, ErrorMessage = "Korisničko ime ne može biti duže od 100 znakova")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress(ErrorMessage = "Nevažeća email adresa")]
        [MaxLength(255, ErrorMessage = "Email ne može biti duži od 255 znakova")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [MinLength(6, ErrorMessage = "Lozinka mora imati najmanje 6 znakova")]
        [MaxLength(100, ErrorMessage = "Lozinka ne može biti duža od 100 znakova")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Lozinke se ne podudaraju")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
