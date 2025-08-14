using taskflow_api_backend.Models;

namespace taskflow_api_backend.Repositories
{
    /// <summary>
    /// Repository abstraction for TaskItem entity.
    /// </summary>
    public interface ITaskRepository
    {
        // PUBLIC_INTERFACE
        Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<IReadOnlyList<TaskItem>> GetAllForUserAsync(Guid userId, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task AddAsync(TaskItem task, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task UpdateAsync(TaskItem task, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task DeleteAsync(TaskItem task, CancellationToken ct = default);
    }
}
