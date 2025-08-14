using TaskFlow.Api.Models.DTOs;

namespace TaskFlow.Api.Services.Interfaces
{
    public interface ITaskService
    {
        // PUBLIC_INTERFACE
        Task<List<TaskDto>> GetAllAsync(CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<TaskDto> CreateAsync(TaskCreateDto dto, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<TaskDto?> UpdateAsync(Guid id, TaskUpdateDto dto, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
