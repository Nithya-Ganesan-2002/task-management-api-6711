using Microsoft.EntityFrameworkCore;
using taskflow_api_backend.DTOs.Tasks;
using taskflow_api_backend.Models;
using taskflow_api_backend.Repositories;

namespace taskflow_api_backend.Services
{
    /// <summary>
    /// Task service implementation.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IUserRepository _userRepo;

        public TaskService(ITaskRepository taskRepo, IUserRepository userRepo)
        {
            _taskRepo = taskRepo;
            _userRepo = userRepo;
        }

        // PUBLIC_INTERFACE
        public async Task<TaskDto> CreateAsync(Guid creatorUserId, TaskCreateRequest request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required.");

            // Validate assigned user if provided
            if (request.AssignedToUserId.HasValue)
            {
                var assigned = await _userRepo.GetByIdAsync(request.AssignedToUserId.Value, ct);
                if (assigned == null) throw new ArgumentException("Assigned user does not exist.");
            }

            var now = DateTime.UtcNow;
            var task = new TaskItem
            {
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                DueDate = request.DueDate,
                CreatedById = creatorUserId,
                AssignedToId = request.AssignedToUserId,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _taskRepo.AddAsync(task, ct);
            var created = await _taskRepo.GetByIdAsync(task.Id, ct);
            return ToDto(created ?? task);
        }

        // PUBLIC_INTERFACE
        public async Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var task = await _taskRepo.GetByIdAsync(id, ct);
            return task == null ? null : ToDto(task);
        }

        // PUBLIC_INTERFACE
        public async Task<IReadOnlyList<TaskDto>> GetForUserAsync(Guid userId, CancellationToken ct = default)
        {
            var tasks = await _taskRepo.GetAllForUserAsync(userId, ct);
            return tasks.Select(ToDto).ToList();
        }

        // PUBLIC_INTERFACE
        public async Task<IReadOnlyList<TaskDto>> GetAllAsync(CancellationToken ct = default)
        {
            var tasks = await _taskRepo.GetAllAsync(ct);
            return tasks.Select(ToDto).ToList();
        }

        // PUBLIC_INTERFACE
        public async Task<TaskDto?> UpdateAsync(Guid taskId, TaskUpdateRequest request, CancellationToken ct = default)
        {
            var task = await _taskRepo.GetByIdAsync(taskId, ct);
            if (task == null) return null;

            if (!string.IsNullOrWhiteSpace(request.Title))
                task.Title = request.Title.Trim();

            if (request.Description != null)
                task.Description = request.Description?.Trim();

            if (request.Status.HasValue)
                task.Status = request.Status.Value;

            if (request.DueDate.HasValue)
                task.DueDate = request.DueDate;

            if (request.AssignedToUserId.HasValue)
            {
                var assigned = await _userRepo.GetByIdAsync(request.AssignedToUserId.Value, ct);
                if (assigned == null) throw new ArgumentException("Assigned user does not exist.");
                task.AssignedToId = request.AssignedToUserId;
            }

            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepo.UpdateAsync(task, ct);
            var updated = await _taskRepo.GetByIdAsync(task.Id, ct);
            return updated == null ? null : ToDto(updated);
        }

        // PUBLIC_INTERFACE
        public async Task<bool> DeleteAsync(Guid taskId, Guid requesterUserId, CancellationToken ct = default)
        {
            var task = await _taskRepo.GetByIdAsync(taskId, ct);
            if (task == null) return false;

            // Only creator can delete
            if (task.CreatedById != requesterUserId)
                throw new UnauthorizedAccessException("Only the creator can delete this task.");

            await _taskRepo.DeleteAsync(task, ct);
            return true;
        }

        private static TaskDto ToDto(TaskItem t) => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            DueDate = t.DueDate,
            CreatedById = t.CreatedById,
            AssignedToId = t.AssignedToId,
            AssignedToUsername = t.AssignedTo?.Username,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        };
    }
}
