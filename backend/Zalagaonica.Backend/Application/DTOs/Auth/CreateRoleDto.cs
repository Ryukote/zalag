using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Naziv uloge je obavezan")]
        [MinLength(2, ErrorMessage = "Naziv uloge mora imati najmanje 2 znaka")]
        [MaxLength(100, ErrorMessage = "Naziv uloge ne može biti dulji od 100 znakova")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Opis ne može biti dulji od 500 znakova")]
        public string? Description { get; set; }
    }
}
