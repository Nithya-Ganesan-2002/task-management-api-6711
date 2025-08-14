using TaskFlow.Api.Models.DTOs;
using TaskFlow.Api.Models.Entities;

namespace TaskFlow.Api.Mapping
{
    /// <summary>
    /// Extension methods to map entities to DTOs.
    /// </summary>
    public static class ModelMappings
    {
        // PUBLIC_INTERFACE
        public static UserDto ToDto(this User user) => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };

        // PUBLIC_INTERFACE
        public static TaskDto ToDto(this TaskItem task) => new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate,
            AssignedToUserId = task.AssignedToUserId,
            AssignedToUsername = task.AssignedToUser?.Username,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
