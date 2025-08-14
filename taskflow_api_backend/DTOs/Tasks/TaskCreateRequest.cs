using System;

namespace taskflow_api_backend.DTOs.Tasks
{
    /// <summary>
    /// Request payload for creating a task.
    /// </summary>
    public class TaskCreateRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }
}
