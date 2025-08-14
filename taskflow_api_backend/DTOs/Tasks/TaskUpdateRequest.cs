using System;

namespace taskflow_api_backend.DTOs.Tasks
{
    /// <summary>
    /// Request payload for updating a task.
    /// </summary>
    public class TaskUpdateRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        // Alias to our domain enum to avoid ambiguity with System.Threading.Tasks.TaskStatus
        public taskflow_api_backend.Models.Enums.TaskStatus? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }
}
