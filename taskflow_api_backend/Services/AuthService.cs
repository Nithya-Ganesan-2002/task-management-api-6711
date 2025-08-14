using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskFlow.Api.Mapping;
using TaskFlow.Api.Models.DTOs;
using TaskFlow.Api.Models.Entities;
using TaskFlow.Api.Repositories.Interfaces;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Services
{
    /// <summary>
    /// Handles user registration and login, including JWT issuance.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthService(IUserRepository users, IConfiguration config)
        {
            _users = users;
            _config = config;
        }

        // PUBLIC_INTERFACE
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
        {
            // Check if username or email already exists
            var existingByUsername = await _users.GetByUsernameAsync(request.Username, ct);
            if (existingByUsername != null)
                throw new InvalidOperationException("Username already exists.");

            var existingByEmail = await _users.GetByEmailAsync(request.Email, ct);
            if (existingByEmail != null)
                throw new InvalidOperationException("Email already exists.");

            var user = new User
            {
                Username = request.Username.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _hasher.HashPassword(user, request.Password);
            await _users.AddAsync(user, ct);

            var (token, expiresAt) = GenerateJwtToken(user);
            return new AuthResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = user.ToDto()
            };
        }

        // PUBLIC_INTERFACE
        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            User? user = null;

            if (request.UsernameOrEmail.Contains('@'))
                user = await _users.GetByEmailAsync(request.UsernameOrEmail.Trim().ToLowerInvariant(), ct);
            else
                user = await _users.GetByUsernameAsync(request.UsernameOrEmail.Trim(), ct);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (verify == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var (token, expiresAt) = GenerateJwtToken(user);
            return new AuthResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = user.ToDto()
            };
        }

        private (string token, DateTime expiresAt) GenerateJwtToken(User user)
        {
            var issuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("Missing Jwt:Issuer");
            var audience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("Missing Jwt:Audience");
            var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Missing Jwt:Key");
            var expirationMinutesStr = _config["Jwt:ExpirationMinutes"] ?? "60";
            var expirationMinutes = int.TryParse(expirationMinutesStr, out var m) ? m : 60;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expiresAt);
        }
    }
}
