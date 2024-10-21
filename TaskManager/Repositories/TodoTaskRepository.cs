using Microsoft.EntityFrameworkCore;
using TaskManager.DTOs;
using TaskManager.Models;

namespace TaskManager.Repositories
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly UnitOfWork unitOfWork;


        public TodoTaskRepository(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

        }



        public async Task<IEnumerable<TodoTaskDto>> GetAllAsync()
        {
            try
            {
                var tasks = await unitOfWork.dbContext.Tasks
                    .Include(t => t.TaskType)
                    .Include(t => t.TaskDifficulty)
                    .Include(t => t.User)
                    .Include(t => t.Reminders)
                    .ToListAsync();
                return unitOfWork.mapper.Map<IEnumerable<TodoTaskDto>>(tasks);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw; // Re-throw the exception to be caught by the global exception handler
            }
        }

        public async Task<TodoTaskDto> GetByIdAsync(Guid id)
        {

            var task = await unitOfWork.dbContext.Tasks.Include(t => t.TaskType).Include(t => t.TaskDifficulty).Include(t => t.User).Include(t => t.Reminders).FirstOrDefaultAsync(t => t.Id == id);
            return unitOfWork.mapper.Map<TodoTaskDto>(task);

        }


        public async Task<IEnumerable<TodoTaskDto>> GetTasksByUserIdAsync(Guid id)
        {

            var tasks = await unitOfWork.dbContext.Tasks.Include(t => t.TaskType).Include(t => t.TaskDifficulty).Include(t => t.Reminders).Include(t => t.User)
            .Where(t => t.UserId == id)
            .ToListAsync();
            return unitOfWork.mapper.Map<IEnumerable<TodoTaskDto>>(tasks);

        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await unitOfWork.TodoTasks.GetByIdAsync(id);
            if (task == null)
            {
                return false;
            }
            unitOfWork.TodoTasks.Remove(task);
            await unitOfWork.CompleteAsync();
            return true;


        }


        public async Task<TodoTaskDto> AddAsync(CreateTodoTaskDto createTaskDto)
        {

            var task = unitOfWork.mapper.Map<TodoTask>(createTaskDto);
            await unitOfWork.TodoTasks.AddAsync(task);
            var taskDto = unitOfWork.mapper.Map<TodoTaskDto>(task);
            await unitOfWork.CompleteAsync();
            return taskDto;



        }

        public async Task<TodoTaskDto> UpdateAsync(Guid id, UpdateTodoTaskDto updateTaskDto)
        {
            var task = await unitOfWork.TodoTasks.GetByIdAsync(id);
            if (task == null)
            {
                return null;
            }

            // Cập nhật các thuộc tính
            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.IsCompleted = updateTaskDto.IsCompleted;
            task.DueDate = updateTaskDto.DueDate;
            task.TaskTypeId = updateTaskDto.TaskTypeId;
            task.TaskDifficultyId = updateTaskDto.TaskDifficultyId;
            task.UserId = updateTaskDto.UserId;

            unitOfWork.TodoTasks.Update(task, id);
            await unitOfWork.CompleteAsync();

            return unitOfWork.mapper.Map<TodoTaskDto>(task);
        }

    }
}

