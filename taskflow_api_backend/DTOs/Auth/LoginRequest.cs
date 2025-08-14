namespace taskflow_api_backend.DTOs.Auth
{
    /// <summary>
    /// Request payload for user login.
    /// </summary>
    public class LoginRequest
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
