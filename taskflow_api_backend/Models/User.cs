using System.ComponentModel.DataAnnotations;

namespace taskflow_api_backend.Models
{
    /// <summary>
    /// Represents an application user.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Role { get; set; } = "User";

        public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    }
}
