using TaskFlow.Api.Models.DTOs;

namespace TaskFlow.Api.Services.Interfaces
{
    public interface IAuthService
    {
        // PUBLIC_INTERFACE
        Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    }
}
