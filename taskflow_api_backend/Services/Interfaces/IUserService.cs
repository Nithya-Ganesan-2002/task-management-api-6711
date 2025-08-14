using TaskFlow.Api.Models.DTOs;

namespace TaskFlow.Api.Services.Interfaces
{
    public interface IUserService
    {
        // PUBLIC_INTERFACE
        Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        Task<List<UserDto>> GetAllAsync(CancellationToken ct = default);
    }
}
