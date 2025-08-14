using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Models.Entities
{
    /// <summary>
    /// Represents an application user.
    /// </summary>
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }

    /// <summary>
    /// User roles supported by the system.
    /// </summary>
    public enum UserRole
    {
        User = 0,
        Admin = 1
    }
}
