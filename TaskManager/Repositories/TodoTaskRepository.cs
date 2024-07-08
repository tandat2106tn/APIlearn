using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.DTOs;
using TaskManager.Models;

namespace TaskManager.Repositories
{
	public class TodoTaskRepository : GenericRepository<TodoTask>, ITodoTaskRepository
	{
		private readonly IMapper mapper;
		private readonly ILogger<TodoTaskRepository> logger;

		public TodoTaskRepository(ApplicationDbContext dbContext, IMapper mapper, ILogger<TodoTaskRepository> logger) : base(dbContext)
		{
			this.mapper = mapper;
			this.logger = logger;
		}



		public async Task<IEnumerable<TodoTaskDto>> GetAllAsync()
		{
			try
			{
				var tasks = await dbContext.Tasks.Include(t => t.TaskType).Include(t => t.TaskDifficulty).Include(t => t.User).ToListAsync();
				var taskDtos = mapper.Map<IEnumerable<TodoTaskDto>>(tasks);
				return taskDtos;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error occurred while getting all tasks");
				throw;
			}
		}

		public async Task<TodoTaskDto> GetByIdAsync(Guid id)
		{
			try
			{
				var task = await dbContext.Tasks.Include(t => t.TaskType).Include(t => t.TaskDifficulty).Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
				return mapper.Map<TodoTaskDto>(task);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error occurred while getting task by Id");
				throw;
			}
		}


		public async Task<IEnumerable<TodoTaskDto>> GetTasksByUserIdAsync(Guid id)
		{
			try
			{
				var tasks = await dbContext.Tasks.Include(t => t.TaskType).Include(t => t.TaskDifficulty).Include(t => t.User)
				.Where(t => t.UserId == id)
				.ToListAsync();
				return mapper.Map<IEnumerable<TodoTaskDto>>(tasks);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error occurred while getting tasks by UserId");
				throw;
			}
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			try
			{


				var task = dbContext.Tasks.FirstOrDefault(t => t.Id == id);
				if (task != null)
				{
					base.Remove(task);
					return true;
				}
				return false;

			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error occurred while deleting task");
				throw;
			}


		}


		public async Task<TodoTaskDto> AddAsync(CreateTodoTaskDto createTaskDto)
		{
			try
			{
				var task = mapper.Map<TodoTask>(createTaskDto);
				await base.AddAsync(task);
				var taskDto = mapper.Map<TodoTaskDto>(task);
				return taskDto;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error occurred while Adding task");
				throw;
			}

		}

		public async Task<TodoTaskDto> UpdateAsync(Guid id, UpdateTodoTaskDto updateTaskDto)
		{
			try
			{


				var task = await base.GetByIdAsync(id);
				if (task == null)
				{
					return null;
				}

				// Chỉ update các trường cần thiết
				mapper.Map(updateTaskDto, task);
				base.Update(task);



				var taskDto = mapper.Map<TodoTaskDto>(task);
				return taskDto;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error occurred while Updating task");
				throw;
			}
		}
	}
}
