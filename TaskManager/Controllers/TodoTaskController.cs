using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Repositories;

[ApiController]


[Route("api/[controller]")]

public class TodoTasksController : ControllerBase
{

    private readonly ITodoTaskRepository todoTaskRepository;
    private readonly ApplicationDbContext context;

    public TodoTasksController(ITodoTaskRepository todoTaskRepository, ApplicationDbContext _context)
    {

        this.todoTaskRepository = todoTaskRepository;
        context = _context;
    }

    [HttpGet]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireUserRole")]
    public async Task<ActionResult<IEnumerable<TodoTaskDto>>> GetAllTasks()
    {

        var tasks = await todoTaskRepository.GetAllAsync();
        return Ok(tasks);

    }

    [HttpGet("{id}")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireUserRole")]
    public async Task<ActionResult<TodoTaskDto>> GetTaskById(Guid id)
    {
        var task = await todoTaskRepository.GetByIdAsync(id);
        if (task == null)
            if (task == null)
            {
                return NotFound();
            }
        return Ok(task);
    }
    [HttpPost]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
    public async Task<ActionResult<TodoTaskDto>> CreateTask(CreateTodoTaskDto createTodoTaskDto)
    {
        var createdTask = await todoTaskRepository.AddAsync(createTodoTaskDto);
        return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id}")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
    public async Task<IActionResult> UpdateTask(Guid id, UpdateTodoTaskDto updateTodoTaskDto)
    {
        var updatedTask = await todoTaskRepository.UpdateAsync(id, updateTodoTaskDto);
        if (updatedTask == null)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var result = await todoTaskRepository.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("taskTypes")]
    public async Task<ActionResult<IEnumerable<TaskType>>> GetTaskTypes()
    {
        return await context.TaskTypes.ToListAsync();
    }


    [HttpGet("taskDifficulties")]
    public async Task<ActionResult<IEnumerable<TaskDifficulty>>> GetTaskDifficulties()
    {
        return await context.TaskDifficulties.ToListAsync();
    }


}
