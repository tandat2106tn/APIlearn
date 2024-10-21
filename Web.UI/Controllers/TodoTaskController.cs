using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagerMVC.Services;
using TaskManagerMVC.ViewModels;

namespace TaskManagerMVC.Controllers
{

    public class TodoTaskController : Controller
    {
        private readonly ITodoTaskService _todoTaskService;

        public TodoTaskController(ITodoTaskService todoTaskService)
        {
            _todoTaskService = todoTaskService;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _todoTaskService.GetAllTasksAsync();
            return View(tasks);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var task = await _todoTaskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PrepareTaskFormDataAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTodoTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _todoTaskService.CreateTaskAsync(model);
                return RedirectToAction(nameof(Index));
            }
            await PrepareTaskFormDataAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var task = await _todoTaskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            var updateModel = new UpdateTodoTaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                DueDate = task.DueDate,
                TaskTypeId = task.TaskTypeId,
                TaskDifficultyId = task.TaskDifficultyId
            };
            await PrepareTaskFormDataAsync();
            return View(updateModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, UpdateTodoTaskViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _todoTaskService.UpdateTaskAsync(id, model);
                return RedirectToAction(nameof(Index));
            }
            await PrepareTaskFormDataAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _todoTaskService.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareTaskFormDataAsync()
        {
            ViewBag.TaskTypes = new SelectList(await _todoTaskService.GetTaskTypesAsync(), "Id", "Name");
            ViewBag.TaskDifficulties = new SelectList(await _todoTaskService.GetTaskDifficultiesAsync(), "Id", "Name");
        }
    }
}