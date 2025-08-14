using TaskFlow.Api.Models.Entities;

namespace TaskFlow.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        // PUBLIC_INTERFACE
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task AddAsync(User user, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task UpdateAsync(User user, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<List<User>> GetAllAsync(CancellationToken ct = default);
    }
}
