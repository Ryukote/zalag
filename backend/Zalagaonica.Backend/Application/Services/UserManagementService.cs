using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserManagementService
    {
        private readonly ApplicationDbContext _context;

        public UserManagementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserAccountDto>> GetAllUsersAsync()
        {
            return await _context.UserAccounts
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => new UserAccountDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    Roles = u.UserRoles.Select(ur => ur.Role!.Name).ToList()
                })
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        public async Task<UserAccountDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.UserAccounts
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            return new UserAccountDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                Roles = user.UserRoles.Select(ur => ur.Role!.Name).ToList()
            };
        }

        public async Task<UserAccountDto> CreateUserAsync(CreateUserDto dto)
        {
            var existingUser = await _context.UserAccounts
                .FirstOrDefaultAsync(u => u.Email == dto.Email || u.Username == dto.Username);

            if (existingUser != null)
                throw new InvalidOperationException("Korisnik s tim emailom ili korisničkim imenom već postoji");

            var user = new UserAccount
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = AuthService.HashPassword(dto.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserAccounts.Add(user);
            await _context.SaveChangesAsync();

            // Assign roles
            if (dto.RoleIds != null && dto.RoleIds.Any())
            {
                foreach (var roleId in dto.RoleIds)
                {
                    _context.UserRoles.Add(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleId
                    });
                }
                await _context.SaveChangesAsync();
            }

            return await GetUserByIdAsync(user.Id) ?? throw new Exception("Failed to retrieve created user");
        }

        public async Task<bool> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _context.UserAccounts.FindAsync(id);
            if (user == null) return false;

            if (!string.IsNullOrEmpty(dto.Username))
                user.Username = dto.Username;

            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;

            if (dto.IsActive.HasValue)
                user.IsActive = dto.IsActive.Value;

            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                user.PasswordHash = AuthService.HashPassword(dto.NewPassword);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignRolesAsync(Guid userId, List<Guid> roleIds)
        {
            var user = await _context.UserAccounts.FindAsync(userId);
            if (user == null) return false;

            // Remove existing roles
            var existingRoles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();
            _context.UserRoles.RemoveRange(existingRoles);

            // Add new roles
            foreach (var roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.UserAccounts.FindAsync(id);
            if (user == null) return false;

            // Remove user roles first
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == id)
                .ToListAsync();
            _context.UserRoles.RemoveRange(userRoles);

            _context.UserAccounts.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public class UserAccountDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<Guid>? RoleIds { get; set; }
    }

    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public string? NewPassword { get; set; }
    }
}
