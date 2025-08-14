using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taskflow_api_backend.DTOs.Users;
using taskflow_api_backend.Services;

namespace taskflow_api_backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Gets the current authenticated user's profile.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>User profile of the current authenticated user.</returns>
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Me(CancellationToken ct)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                return NotFound();
            }

            var user = await _userService.GetByIdAsync(userId, ct);
            if (user == null) return NotFound();

            return Ok(user);
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Lists all users.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var users = await _userService.GetAllAsync(ct);
            return Ok(users);
        }
    }
}
