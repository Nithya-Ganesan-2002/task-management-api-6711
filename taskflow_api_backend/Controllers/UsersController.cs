using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TaskFlow.Api.Models.DTOs;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _users;

        public UsersController(IUserService users)
        {
            _users = users;
        }

        /// <summary>
        /// Get the current authenticated user's profile.
        /// </summary>
        /// <returns>User profile.</returns>
        // PUBLIC_INTERFACE
        [HttpGet("me")]
        [OpenApiOperation(operationId: "GetCurrentUser")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> Me(CancellationToken ct)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            if (!Guid.TryParse(userIdStr, out var userId))
            {
                // fallback if token doesn't include GUID in NameIdentifier
                var sub = User.FindFirstValue("sub");
                if (!Guid.TryParse(sub, out userId))
                {
                    return Unauthorized(new { error = "Invalid token." });
                }
            }

            var user = await _users.GetByIdAsync(userId, ct);
            if (user == null) return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>List of users.</returns>
        // PUBLIC_INTERFACE
        [HttpGet]
        [OpenApiOperation(operationId: "GetUsers")]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserDto>>> GetAll(CancellationToken ct)
        {
            var users = await _users.GetAllAsync(ct);
            return Ok(users);
        }
    }
}
