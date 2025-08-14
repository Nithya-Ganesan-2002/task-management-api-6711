using taskflow_api_backend.DTOs.Users;
using taskflow_api_backend.Repositories;

namespace taskflow_api_backend.Services
{
    /// <summary>
    /// User service implementation.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // PUBLIC_INTERFACE
        public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var user = await _userRepo.GetByIdAsync(id, ct);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }

        // PUBLIC_INTERFACE
        public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken ct = default)
        {
            var users = await _userRepo.GetAllAsync(ct);
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role
            }).ToList();
        }
    }
}
