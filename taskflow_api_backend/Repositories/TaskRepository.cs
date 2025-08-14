using Microsoft.EntityFrameworkCore;
using taskflow_api_backend.Data;
using taskflow_api_backend.Models;

namespace taskflow_api_backend.Repositories
{
    /// <summary>
    /// EF Core implementation for ITaskRepository.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _db;

        public TaskRepository(AppDbContext db)
        {
            _db = db;
        }

        // PUBLIC_INTERFACE
        public Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            _db.Tasks
               .Include(t => t.AssignedTo)
               .AsNoTracking()
               .FirstOrDefaultAsync(t => t.Id == id, ct);

        // PUBLIC_INTERFACE
        public async Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _db.Tasks
                                .Include(t => t.AssignedTo)
                                .AsNoTracking()
                                .OrderByDescending(t => t.UpdatedAt)
                                .ToListAsync(ct);
            return list;
        }

        // PUBLIC_INTERFACE
        public async Task<IReadOnlyList<TaskItem>> GetAllForUserAsync(Guid userId, CancellationToken ct = default)
        {
            var list = await _db.Tasks
                                .Include(t => t.AssignedTo)
                                .AsNoTracking()
                                .Where(t => t.CreatedById == userId || t.AssignedToId == userId)
                                .OrderByDescending(t => t.UpdatedAt)
                                .ToListAsync(ct);
            return list;
        }

        // PUBLIC_INTERFACE
        public async Task AddAsync(TaskItem task, CancellationToken ct = default)
        {
            await _db.Tasks.AddAsync(task, ct);
            await _db.SaveChangesAsync(ct);
        }

        // PUBLIC_INTERFACE
        public async Task UpdateAsync(TaskItem task, CancellationToken ct = default)
        {
            _db.Tasks.Update(task);
            await _db.SaveChangesAsync(ct);
        }

        // PUBLIC_INTERFACE
        public async Task DeleteAsync(TaskItem task, CancellationToken ct = default)
        {
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync(ct);
        }
    }
}
