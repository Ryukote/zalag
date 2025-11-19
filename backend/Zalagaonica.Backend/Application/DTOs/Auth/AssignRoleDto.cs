using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class AssignRoleDto
    {
        [Required(ErrorMessage = "ID korisnika je obavezan")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "ID uloge je obavezan")]
        public Guid RoleId { get; set; }
    }
}
