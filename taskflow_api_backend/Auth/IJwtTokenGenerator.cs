using taskflow_api_backend.Models;

namespace taskflow_api_backend.Auth
{
    /// <summary>
    /// Abstraction for generating JWT tokens.
    /// </summary>
    public interface IJwtTokenGenerator
    {
        // PUBLIC_INTERFACE
        (string Token, string ExpiresAtUtc) GenerateToken(User user);
    }
}
