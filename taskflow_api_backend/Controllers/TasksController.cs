using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taskflow_api_backend.DTOs.Tasks;
using taskflow_api_backend.Services;

namespace taskflow_api_backend.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private Guid GetUserIdOrThrow()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                           ?? throw new UnauthorizedAccessException("Invalid user claims.");
            if (!Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedAccessException("Invalid user claims.");
            return userId;
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="request">Task creation payload.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created task.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] TaskCreateRequest request, CancellationToken ct)
        {
            try
            {
                var userId = GetUserIdOrThrow();
                var created = await _taskService.CreateAsync(userId, request, ct);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Gets a task by its identifier.
        /// </summary>
        /// <param name="id">Task identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The task if found.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var task = await _taskService.GetByIdAsync(id, ct);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Lists tasks visible to the current user (created by or assigned to).
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of tasks for the current user.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMine(CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var list = await _taskService.GetForUserAsync(userId, ct);
            return Ok(list);
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">Task identifier.</param>
        /// <param name="request">Update payload.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The updated task if found.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TaskUpdateRequest request, CancellationToken ct)
        {
            try
            {
                var updated = await _taskService.UpdateAsync(id, request, ct);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Deletes a task (only the creator can delete).
        /// </summary>
        /// <param name="id">Task identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>No content if deleted.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            try
            {
                var userId = GetUserIdOrThrow();
                var ok = await _taskService.DeleteAsync(id, userId, ct);
                if (!ok) return NotFound();
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
