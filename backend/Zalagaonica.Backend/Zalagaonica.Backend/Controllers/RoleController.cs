using Application.DTOs.Auth;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = await _roleService.CreateRoleAsync(createRoleDto.Name, createRoleDto.Description);
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _roleService.AssignRoleToUserAsync(assignRoleDto.UserId, assignRoleDto.RoleId);

            if (!success)
                return BadRequest(new { message = "Korisnik ili uloga ne postoji" });

            return Ok(new { message = "Uloga uspješno dodijeljena korisniku" });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleDto assignRoleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _roleService.RemoveRoleFromUserAsync(assignRoleDto.UserId, assignRoleDto.RoleId);

            if (!success)
                return BadRequest(new { message = "Korisnička uloga nije pronađena" });

            return Ok(new { message = "Uloga uspješno uklonjena s korisnika" });
        }
    }
}
