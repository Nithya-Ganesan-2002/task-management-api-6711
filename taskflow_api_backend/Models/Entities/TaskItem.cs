using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Models.Entities
{
    /// <summary>
    /// Represents a task in the system.
    /// </summary>
    public class TaskItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string? Description { get; set; }

        [Required]
        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        public DateTime? DueDate { get; set; }

        public Guid? AssignedToUserId { get; set; }

        public User? AssignedToUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Status of a task item.
    /// </summary>
    public enum TaskStatus
    {
        Todo = 0,
        InProgress = 1,
        Done = 2
    }
}
