using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using TaskManager.Web.Services;

namespace TaskManager.Web.Pages.Reminders
{
	public class EditModel : PageModel
	{
		private readonly ApiClient _apiClient;

		public EditModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		[BindProperty]
		public UpdateReminderDto Reminder { get; set; }

		public SelectList Tasks { get; set; }

		public async Task<IActionResult> OnGetAsync(Guid id)
		{
			var response = await _apiClient.GetAsync($"api/reminder/{id}");

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				Reminder = JsonSerializer.Deserialize<UpdateReminderDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				await LoadTasks();
				return Page();
			}

			return NotFound();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				await LoadTasks();
				return Page();
			}

			var json = JsonSerializer.Serialize(Reminder);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _apiClient.PutAsync($"api/reminder/{Reminder.Id}", content);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToPage("./Index");
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				ModelState.AddModelError(string.Empty, $"Error updating reminder. Details: {errorContent}");
				await LoadTasks();
				return Page();
			}
		}

		private async Task LoadTasks()
		{
			var response = await _apiClient.GetAsync("api/todotasks");
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var tasks = JsonSerializer.Deserialize<List<TodoTaskDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				Tasks = new SelectList(tasks, "Id", "Title");
			}
		}
	}

	public class UpdateReminderDto
	{
		public Guid Id { get; set; }
		public string Message { get; set; }
		public DateTime ReminderTime { get; set; }
		public bool IsTriggered { get; set; }
		public Guid TodoTaskId { get; set; }
	}
}