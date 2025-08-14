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
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _tasks;

        public TasksController(ITaskService tasks)
        {
            _tasks = tasks;
        }

        /// <summary>
        /// Get all tasks.
        /// </summary>
        /// <returns>List of tasks.</returns>
        // PUBLIC_INTERFACE
        [HttpGet]
        [OpenApiOperation(operationId: "GetTasks")]
        [ProducesResponseType(typeof(List<TaskDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TaskDto>>> GetAll(CancellationToken ct)
        {
            var items = await _tasks.GetAllAsync(ct);
            return Ok(items);
        }

        /// <summary>
        /// Get a task by id.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <returns>Task.</returns>
        // PUBLIC_INTERFACE
        [HttpGet("{id:guid}")]
        [OpenApiOperation(operationId: "GetTaskById")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDto>> GetById(Guid id, CancellationToken ct)
        {
            var item = await _tasks.GetByIdAsync(id, ct);
            if (item == null) return NotFound();
            return Ok(item);
        }

        /// <summary>
        /// Create a new task.
        /// </summary>
        /// <param name="dto">Task creation payload.</param>
        /// <returns>Created task.</returns>
        // PUBLIC_INTERFACE
        [HttpPost]
        [OpenApiOperation(operationId: "CreateTask")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<TaskDto>> Create([FromBody] TaskCreateDto dto, CancellationToken ct)
        {
            var created = await _tasks.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update a task.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <param name="dto">Task update payload.</param>
        /// <returns>Updated task.</returns>
        // PUBLIC_INTERFACE
        [HttpPut("{id:guid}")]
        [OpenApiOperation(operationId: "UpdateTask")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDto>> Update(Guid id, [FromBody] TaskUpdateDto dto, CancellationToken ct)
        {
            var updated = await _tasks.UpdateAsync(id, dto, ct);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Delete a task.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <returns>Result of deletion.</returns>
        // PUBLIC_INTERFACE
        [HttpDelete("{id:guid}")]
        [OpenApiOperation(operationId: "DeleteTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _tasks.DeleteAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
