using Microsoft.EntityFrameworkCore;
using taskflow_api_backend.Data;
using taskflow_api_backend.Models;

namespace taskflow_api_backend.Repositories
{
    /// <summary>
    /// EF Core implementation for IUserRepository.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        // PUBLIC_INTERFACE
        public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

        // PUBLIC_INTERFACE
        public Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
            _db.Users.FirstOrDefaultAsync(u => u.Username == username, ct);

        // PUBLIC_INTERFACE
        public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
            _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

        // PUBLIC_INTERFACE
        public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
            _db.Users.AsNoTracking().AnyAsync(u => u.Email == email, ct);

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
        public async Task DeleteAsync(User user, CancellationToken ct = default)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync(ct);
        }

        // PUBLIC_INTERFACE
        public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default)
        {
            var users = await _db.Users.AsNoTracking().ToListAsync(ct);
            return users;
        }
    }
}
