namespace taskflow_api_backend.Options
{
    /// <summary>
    /// JWT settings loaded from environment variables.
    /// </summary>
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpirationMinutes { get; set; } = 60;
    }
}
