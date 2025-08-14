using System;

namespace taskflow_api_backend.DTOs.Users
{
    /// <summary>
    /// Public user data transfer object.
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}
