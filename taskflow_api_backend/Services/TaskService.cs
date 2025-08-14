using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Mapping;
using TaskFlow.Api.Models.DTOs;
using TaskFlow.Api.Models.Entities;
using TaskFlow.Api.Repositories.Interfaces;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Services
{
    /// <summary>
    /// Provides CRUD operations for tasks.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _tasks;

        public TaskService(ITaskRepository tasks)
        {
            _tasks = tasks;
        }

        // PUBLIC_INTERFACE
        public async Task<List<TaskDto>> GetAllAsync(CancellationToken ct = default)
        {
            var items = await _tasks.GetAllAsync(ct);
            return items.Select(t => t.ToDto()).ToList();
        }

        // PUBLIC_INTERFACE
        public async Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var item = await _tasks.GetByIdAsync(id, ct);
            return item?.ToDto();
        }

        // PUBLIC_INTERFACE
        public async Task<TaskDto> CreateAsync(TaskCreateDto dto, CancellationToken ct = default)
        {
            var newTask = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                AssignedToUserId = dto.AssignedToUserId,
                Status = TaskFlow.Api.Models.Entities.TaskStatus.Todo,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _tasks.AddAsync(newTask, ct);
            var created = await _tasks.GetByIdAsync(newTask.Id, ct) ?? newTask;
            return created.ToDto();
        }

        // PUBLIC_INTERFACE
        public async Task<TaskDto?> UpdateAsync(Guid id, TaskUpdateDto dto, CancellationToken ct = default)
        {
            var existing = await _tasks.GetByIdAsync(id, ct);
            if (existing == null)
                return null;

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Status = dto.Status;
            existing.DueDate = dto.DueDate;
            existing.AssignedToUserId = dto.AssignedToUserId;
            existing.UpdatedAt = DateTime.UtcNow;

            await _tasks.UpdateAsync(existing, ct);
            var updated = await _tasks.GetByIdAsync(id, ct) ?? existing;
            return updated.ToDto();
        }

        // PUBLIC_INTERFACE
        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _tasks.GetByIdAsync(id, ct);
            if (existing == null) return false;

            await _tasks.DeleteAsync(existing, ct);
            return true;
        }
    }
}
