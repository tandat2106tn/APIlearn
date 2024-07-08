using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.DTOs;
using TaskManager.Repositories;

[ApiController]


[Route("api/[controller]")]
[Authorize]
public class TodoTasksController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<TodoTasksController> logger;

	public TodoTasksController(IUnitOfWork unitOfWork, ILogger<TodoTasksController> logger)
	{
		_unitOfWork = unitOfWork;
		this.logger = logger;
	}

	[HttpGet]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireUserRole")]
	public async Task<ActionResult<IEnumerable<TodoTaskDto>>> GetAllTasks()
	{
		var tasks = await _unitOfWork.TodoTasks.GetAllAsync();
		return Ok(tasks);
	}

	[HttpGet("{id}")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireUserRole")]
	public async Task<ActionResult<TodoTaskDto>> GetTaskById(Guid id)
	{
		var task = await _unitOfWork.TodoTasks.GetByIdAsync(id);
		if (task == null)
		{
			return NotFound();
		}
		return Ok(task);
	}

	[HttpPost]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
	public async Task<ActionResult<TodoTaskDto>> CreateTask(CreateTodoTaskDto createTodoTaskDto)
	{
		var createdTask = await _unitOfWork.TodoTasks.AddAsync(createTodoTaskDto);
		await _unitOfWork.CompleteAsync();
		return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
	}

	[HttpPut("{id}")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
	public async Task<IActionResult> UpdateTask(Guid id, UpdateTodoTaskDto updateTodoTaskDto)
	{
		var updatedTask = await _unitOfWork.TodoTasks.UpdateAsync(id, updateTodoTaskDto);
		if (updatedTask == null)
		{
			return NotFound();
		}
		await _unitOfWork.CompleteAsync();
		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
	public async Task<IActionResult> DeleteTask(Guid id)
	{
		var result = await _unitOfWork.TodoTasks.DeleteAsync(id);
		if (!result)
		{
			return NotFound();
		}
		await _unitOfWork.CompleteAsync();
		return NoContent();
	}
}