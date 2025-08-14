using taskflow_api_backend.DTOs.Auth;

namespace taskflow_api_backend.Services
{
    /// <summary>
    /// Service abstraction for user authentication operations.
    /// </summary>
    public interface IAuthService
    {
        // PUBLIC_INTERFACE
        Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    }
}
