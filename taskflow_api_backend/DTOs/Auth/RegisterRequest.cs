namespace taskflow_api_backend.DTOs.Auth
{
    /// <summary>
    /// Request payload for user registration.
    /// </summary>
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
