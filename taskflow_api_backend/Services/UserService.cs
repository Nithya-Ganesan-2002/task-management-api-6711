using TaskFlow.Api.Mapping;
using TaskFlow.Api.Models.DTOs;
using TaskFlow.Api.Repositories.Interfaces;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Services
{
    /// <summary>
    /// Provides read operations for users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _users;

        public UserService(IUserRepository users)
        {
            _users = users;
        }

        // PUBLIC_INTERFACE
        public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _users.GetByIdAsync(id, ct);
            return entity?.ToDto();
        }

        // PUBLIC_INTERFACE
        public async Task<List<UserDto>> GetAllAsync(CancellationToken ct = default)
        {
            var items = await _users.GetAllAsync(ct);
            return items.Select(x => x.ToDto()).ToList();
        }
    }
}
