using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Models.Entities;
using TaskFlow.Api.Repositories.Interfaces;

namespace TaskFlow.Api.Repositories
{
    /// <summary>
    /// EF Core implementation of IUserRepository.
    /// </summary>
    public class EfUserRepository : IUserRepository
    {
        private readonly TaskFlowDbContext _db;

        public EfUserRepository(TaskFlowDbContext db)
        {
            _db = db;
        }

        // PUBLIC_INTERFACE
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        // PUBLIC_INTERFACE
        public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Username == username, ct);
        }

        // PUBLIC_INTERFACE
        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        }

        // PUBLIC_INTERFACE
        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            await _db.Users.AddAsync(user, ct);
            await _db.SaveChangesAsync(ct);
        }

        // PUBLIC_INTERFACE
        public async Task UpdateAsync(User user, CancellationToken ct = default)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync(ct);
        }

        // PUBLIC_INTERFACE
        public async Task<List<User>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Users.AsNoTracking().ToListAsync(ct);
        }
    }
}
