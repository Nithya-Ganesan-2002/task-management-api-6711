using taskflow_api_backend.DTOs.Users;

namespace taskflow_api_backend.Services
{
    /// <summary>
    /// Service abstraction for user-related operations.
    /// </summary>
    public interface IUserService
    {
        // PUBLIC_INTERFACE
        Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken ct = default);
    }
}
