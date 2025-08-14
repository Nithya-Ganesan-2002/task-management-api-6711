using System;
using TaskStatus = taskflow_api_backend.Models.Enums.TaskStatus;

namespace taskflow_api_backend.DTOs.Tasks
{
    /// <summary>
    /// Public task data transfer object.
    /// </summary>
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? AssignedToId { get; set; }
        public string? AssignedToUsername { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
