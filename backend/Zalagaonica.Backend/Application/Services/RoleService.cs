using Application.DTOs.Auth;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class RoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _context.Roles
                .AsNoTracking()
                .ToListAsync();

            return roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            }).ToList();
        }

        public async Task<RoleDto?> GetRoleByIdAsync(Guid roleId)
        {
            var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
                return null;

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }

        public async Task<RoleDto> CreateRoleAsync(string name, string? description = null)
        {
            var role = new Role
            {
                Name = name,
                Description = description
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            var userExists = await _context.UserAccounts.AnyAsync(u => u.Id == userId);
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == roleId);

            if (!userExists || !roleExists)
                return false;

            var existingUserRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (existingUserRole != null)
                return true; // Already assigned

            _context.UserRoles.Add(new UserRole
            {
                UserId = userId,
                RoleId = roleId
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole == null)
                return false;

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
