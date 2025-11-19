using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress(ErrorMessage = "Nevažeća email adresa")]
        [MaxLength(255, ErrorMessage = "Email ne može biti duži od 255 znakova")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [MinLength(6, ErrorMessage = "Lozinka mora imati najmanje 6 znakova")]
        public string Password { get; set; } = string.Empty;
    }
}
