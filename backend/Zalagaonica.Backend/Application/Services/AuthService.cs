using Application.DTOs.Auth;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            var hash = HashPassword(loginRequest.Password);
            var user = await _context.UserAccounts
                .AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.PasswordHash == hash && u.IsActive);

            if (user == null)
                return null;

            var token = GenerateJwtToken(user);
            var userDto = MapToUserDto(user);

            return new LoginResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        public async Task<UserDto?> RegisterAsync(RegisterRequestDto registerRequest, string roleName = "User")
        {
            // Check if user already exists
            var existingUser = await _context.UserAccounts
                .FirstOrDefaultAsync(u => u.Email == registerRequest.Email || u.Username == registerRequest.Username);

            if (existingUser != null)
                return null;

            var hash = HashPassword(registerRequest.Password);
            var user = new UserAccount
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                PasswordHash = hash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                role = new Role { Name = roleName };
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
            }

            _context.UserAccounts.Add(user);
            await _context.SaveChangesAsync();

            _context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
            await _context.SaveChangesAsync();

            // Reload user with roles
            user = await _context.UserAccounts
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstAsync(u => u.Id == user.Id);

            return MapToUserDto(user);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.UserAccounts
                .AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user == null ? null : MapToUserDto(user);
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.UserAccounts
                .AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();

            return users.Select(MapToUserDto).ToList();
        }

        private string GenerateJwtToken(UserAccount user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "SuperTajniJWTKljuc123!";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Add roles to claims
            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    if (userRole.Role != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
                    }
                }
            }

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserDto MapToUserDto(UserAccount user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                Roles = user.UserRoles?.Select(ur => new RoleDto
                {
                    Id = ur.Role?.Id ?? Guid.Empty,
                    Name = ur.Role?.Name ?? string.Empty,
                    Description = ur.Role?.Description
                }).ToList() ?? new List<RoleDto>()
            };
        }
    }
}
