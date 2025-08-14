using taskflow_api_backend.DTOs.Tasks;

namespace taskflow_api_backend.Services
{
    /// <summary>
    /// Service abstraction for task operations.
    /// </summary>
    public interface ITaskService
    {
        // PUBLIC_INTERFACE
        Task<TaskDto> CreateAsync(Guid creatorUserId, TaskCreateRequest request, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<IReadOnlyList<TaskDto>> GetForUserAsync(Guid userId, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<IReadOnlyList<TaskDto>> GetAllAsync(CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<TaskDto?> UpdateAsync(Guid taskId, TaskUpdateRequest request, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<bool> DeleteAsync(Guid taskId, Guid requesterUserId, CancellationToken ct = default);
    }
}
