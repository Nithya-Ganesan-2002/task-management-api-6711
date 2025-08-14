using System.ComponentModel.DataAnnotations;
using TaskStatus = taskflow_api_backend.Models.Enums.TaskStatus;

namespace taskflow_api_backend.Models
{
    /// <summary>
    /// Task entity for tracking work items.
    /// </summary>
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string? Description { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        public DateTime? DueDate { get; set; }

        public Guid CreatedById { get; set; }
        public User? CreatedBy { get; set; }

        public Guid? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
