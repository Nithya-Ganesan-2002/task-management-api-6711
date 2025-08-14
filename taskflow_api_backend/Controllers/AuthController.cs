using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TaskFlow.Api.Models.DTOs;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        /// <summary>
        /// Register a new user account.
        /// </summary>
        /// <param name="request">Registration request containing username, email, and password.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>JWT token along with user details.</returns>
        // PUBLIC_INTERFACE
        [HttpPost("register")]
        [OpenApiOperation(operationId: "RegisterUser")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request, CancellationToken ct)
        {
            try
            {
                var result = await _auth.RegisterAsync(request, ct);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Login with username/email and password.
        /// </summary>
        /// <param name="request">Login request containing username/email and password.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>JWT token along with user details.</returns>
        // PUBLIC_INTERFACE
        [HttpPost("login")]
        [OpenApiOperation(operationId: "LoginUser")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            try
            {
                var result = await _auth.LoginAsync(request, ct);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}
