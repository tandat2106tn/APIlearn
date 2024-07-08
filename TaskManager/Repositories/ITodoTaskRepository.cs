using TaskManager.DTOs;

namespace TaskManager.Repositories
{
	public interface ITodoTaskRepository
	{

		Task<IEnumerable<TodoTaskDto>> GetAllAsync();
		Task<TodoTaskDto> GetByIdAsync(Guid id);
		Task<IEnumerable<TodoTaskDto>> GetTasksByUserIdAsync(Guid id);
		Task<TodoTaskDto> AddAsync(CreateTodoTaskDto createTaskDto);
		Task<TodoTaskDto> UpdateAsync(Guid id, UpdateTodoTaskDto updateTaskDto);
		Task<bool> DeleteAsync(Guid id);
	}
}
