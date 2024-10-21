using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerMVC.Services;
using TaskManagerMVC.ViewModels;

namespace TaskManagerMVC.Controllers
{
    [Authorize]
    public class ReminderController : Controller
    {
        private readonly IReminderService _reminderService;

        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        public async Task<IActionResult> Index()
        {
            var reminders = await _reminderService.GetAllRemindersAsync();
            return View(reminders);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var reminder = await _reminderService.GetReminderByIdAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }
            return View(reminder);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReminderViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _reminderService.CreateReminderAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var reminder = await _reminderService.GetReminderByIdAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }
            var updateModel = new UpdateReminderViewModel
            {
                Id = reminder.Id,
                Message = reminder.Message,
                ReminderTime = reminder.ReminderTime,
                IsTriggered = reminder.IsTriggered
            };
            return View(updateModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, UpdateReminderViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _reminderService.UpdateReminderAsync(id, model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _reminderService.DeleteReminderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}