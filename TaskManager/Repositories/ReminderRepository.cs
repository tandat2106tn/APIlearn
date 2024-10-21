using Microsoft.EntityFrameworkCore;
using TaskManager.DTOs;
using TaskManager.Models;

namespace TaskManager.Repositories
{
    public interface IReminderRepository
    {
        Task<IEnumerable<ReminderDto>> GetAllAsync();
        Task<ReminderDto> GetByIdAsync(Guid id);
        Task<IEnumerable<ReminderDto>> GetRemindersByTaskIdAsync(Guid taskId);
        Task<ReminderDto> CreateReminderAsync(CreateReminderDto createReminderDto);
        Task<ReminderDto> UpdateReminderAsync(Guid id, UpdateReminderDto updateReminderDto);

        Task<bool> DeleteReminderAsync(Guid id);
    }
    public class ReminderRepository : IReminderRepository
    {
        private readonly UnitOfWork unitOfWork;
        public ReminderRepository(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ReminderDto>> GetAllAsync()
        {
            var reminders = await unitOfWork.Reminders.GetAllAsync();
            var reminderDtos = unitOfWork.mapper.Map<IEnumerable<ReminderDto>>(reminders);
            return reminderDtos;
        }

        public async Task<ReminderDto> GetByIdAsync(Guid id)
        {
            var reminder = await unitOfWork.Reminders.GetByIdAsync(id);
            var reminderDto = unitOfWork.mapper.Map<ReminderDto>(reminder);
            return reminderDto;
        }

        public async Task<ReminderDto> CreateReminderAsync(CreateReminderDto createReminderDto)
        {
            var reminder = unitOfWork.mapper.Map<Reminder>(createReminderDto);

            await unitOfWork.Reminders.AddAsync(reminder);
            await unitOfWork.CompleteAsync();

            return unitOfWork.mapper.Map<ReminderDto>(reminder);
        }

        public async Task<IEnumerable<ReminderDto>> GetRemindersByTaskIdAsync(Guid taskId)
        {
            var reminders = await unitOfWork.dbContext.Reminders.Where(t => t.TodoTaskId == taskId).ToListAsync();
            return unitOfWork.mapper.Map<IEnumerable<ReminderDto>>(reminders);
        }

        public async Task<ReminderDto> UpdateReminderAsync(Guid id, UpdateReminderDto updateReminderDto)
        {
            var reminder = await unitOfWork.Reminders.GetByIdAsync(id);
            if (reminder == null)
            {
                return null;
            }

            // Cập nhật các thuộc tính
            reminder.Message = updateReminderDto.Message;
            reminder.ReminderTime = updateReminderDto.ReminderTime;
            reminder.IsTriggered = updateReminderDto.IsTriggered;


            await unitOfWork.CompleteAsync();

            return unitOfWork.mapper.Map<ReminderDto>(reminder);
        }

        public async Task<bool> DeleteReminderAsync(Guid reminderId)
        {
            var reminder = await unitOfWork.Reminders.GetByIdAsync(reminderId);
            if (reminder == null)
            {
                return false;
            }
            else
            {
                unitOfWork.Reminders.Remove(reminder);
                await unitOfWork.CompleteAsync();
                return true;
            }
        }
    }
}
