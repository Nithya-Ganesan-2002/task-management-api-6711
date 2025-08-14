using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Models.Entities;
using TaskFlow.Api.Repositories.Interfaces;

namespace TaskFlow.Api.Repositories
{
    /// <summary>
    /// EF Core implementation of ITaskRepository.
    /// </summary>
    public class EfTaskRepository : ITaskRepository
    {
        private readonly TaskFlowDbContext _db;

        public EfTaskRepository(TaskFlowDbContext db)
        {
            _db = db;
        }

        // PUBLIC_INTERFACE
        public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Tasks
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        // PUBLIC_INTERFACE
        public async Task<List<TaskItem>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Tasks
                .Include(t => t.AssignedToUser)
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);
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
