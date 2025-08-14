namespace taskflow_api_backend.DTOs.Auth
{
    /// <summary>
    /// Authentication response containing JWT token and user info.
    /// </summary>
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string ExpiresAtUtc { get; set; } = string.Empty;
    }
}
