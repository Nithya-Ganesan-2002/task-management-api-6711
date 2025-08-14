using System.ComponentModel.DataAnnotations;
using TaskFlow.Api.Models.Entities;
using DomainTaskStatus = TaskFlow.Api.Models.Entities.TaskStatus;

namespace TaskFlow.Api.Models.DTOs
{
    public class TaskCreateDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid? AssignedToUserId { get; set; }
    }

    public class TaskUpdateDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string? Description { get; set; }

        [Required]
        public DomainTaskStatus Status { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid? AssignedToUserId { get; set; }
    }

    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DomainTaskStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public string? AssignedToUsername { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
