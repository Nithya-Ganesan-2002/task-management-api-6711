
using taskflow_api_backend.DTOs.Auth;
using taskflow_api_backend.Models;
using taskflow_api_backend.Repositories;
using taskflow_api_backend.Auth;

namespace taskflow_api_backend.Services
{
    /// <summary>
    /// Authentication service implementing login and registration workflows.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtTokenGenerator _jwt;

        public AuthService(IUserRepository userRepo, IJwtTokenGenerator jwt)
        {
            _userRepo = userRepo;
            _jwt = jwt;
        }

        // PUBLIC_INTERFACE
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
        {
            // Simple validations
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ArgumentException("Username is required.");
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required.");
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.");

            var exists = await _userRepo.ExistsByEmailAsync(request.Email, ct);
            if (exists)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            var existingUserByUsername = await _userRepo.GetByUsernameAsync(request.Username, ct);
            if (existingUserByUsername != null)
            {
                throw new InvalidOperationException("A user with this username already exists.");
            }

            var user = new User
            {
                Username = request.Username.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "User"
            };

            await _userRepo.AddAsync(user, ct);

            var token = _jwt.GenerateToken(user);
            return new AuthResponse
            {
                Token = token.Token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ExpiresAtUtc = token.ExpiresAtUtc
            };
        }

        // PUBLIC_INTERFACE
        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.UsernameOrEmail) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Username/Email and Password are required.");
            }

            var lookup = request.UsernameOrEmail.Trim();
            User? user = lookup.Contains("@")
                ? await _userRepo.GetByEmailAsync(lookup, ct)
                : await _userRepo.GetByUsernameAsync(lookup, ct);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!valid)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = _jwt.GenerateToken(user);
            return new AuthResponse
            {
                Token = token.Token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ExpiresAtUtc = token.ExpiresAtUtc
            };
        }
    }
}
