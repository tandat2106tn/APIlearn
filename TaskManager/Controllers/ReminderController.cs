using Microsoft.AspNetCore.Mvc;
using TaskManager.DTOs;
using TaskManager.Repositories;

namespace TaskManager.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderRepository _reminderRepository;

        public ReminderController(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReminderDto>>> GetAllReminders()
        {
            var reminders = await _reminderRepository.GetAllAsync();
            return Ok(reminders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReminderDto>> GetReminderById(Guid id)
        {
            var reminder = await _reminderRepository.GetByIdAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }
            return Ok(reminder);
        }

        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<IEnumerable<ReminderDto>>> GetRemindersByTaskId(Guid taskId)
        {
            var reminders = await _reminderRepository.GetRemindersByTaskIdAsync(taskId);
            return Ok(reminders);
        }

        [HttpPost]
        public async Task<ActionResult<ReminderDto>> CreateReminder(CreateReminderDto createReminderDto)
        {
            var reminder = await _reminderRepository.CreateReminderAsync(createReminderDto);
            return CreatedAtAction(nameof(GetReminderById), new { id = reminder.Id }, reminder);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReminderDto>> UpdateReminder(Guid id, UpdateReminderDto updateReminderDto)
        {
            var reminder = await _reminderRepository.UpdateReminderAsync(id, updateReminderDto);
            if (reminder == null)
            {
                return NotFound();
            }
            return Ok(reminder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReminder(Guid id)
        {
            var result = await _reminderRepository.DeleteReminderAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
