using taskflow_api_backend.Models;

namespace taskflow_api_backend.Repositories
{
    /// <summary>
    /// Repository abstraction for User entity.
    /// </summary>
    public interface IUserRepository
    {
        // PUBLIC_INTERFACE
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task AddAsync(User user, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task UpdateAsync(User user, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task DeleteAsync(User user, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default);
    }
}
