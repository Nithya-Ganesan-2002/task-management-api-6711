using TaskFlow.Api.Models.Entities;

namespace TaskFlow.Api.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        // PUBLIC_INTERFACE
        Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<List<TaskItem>> GetAllAsync(CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task AddAsync(TaskItem task, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task UpdateAsync(TaskItem task, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task DeleteAsync(TaskItem task, CancellationToken ct = default);
    }
}
