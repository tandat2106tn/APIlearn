using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TaskManager.Web.Services;

namespace TaskManager.Web.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ApiClient _apiClient;

		public IndexModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		public int TotalTasks { get; set; }
		public int CompletedTasks { get; set; }
		public int PendingTasks { get; set; }
		public List<TodoTaskDto> RecentTasks { get; set; }
		public List<ReminderDto> UpcomingReminders { get; set; }

		public async Task OnGetAsync()
		{
			var tasksResponse = await _apiClient.GetAsync("api/todotasks");
			var remindersResponse = await _apiClient.GetAsync("api/reminder");

			if (tasksResponse.IsSuccessStatusCode)
			{
				var content = await tasksResponse.Content.ReadAsStringAsync();
				var tasks = JsonSerializer.Deserialize<List<TodoTaskDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				TotalTasks = tasks.Count;
				CompletedTasks = tasks.Count(t => t.IsCompleted);
				PendingTasks = TotalTasks - CompletedTasks;
				RecentTasks = tasks.OrderByDescending(t => t.DueDate).Take(5).ToList();
			}

			if (remindersResponse.IsSuccessStatusCode)
			{
				var content = await remindersResponse.Content.ReadAsStringAsync();
				var reminders = JsonSerializer.Deserialize<List<ReminderDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				UpcomingReminders = reminders
											 .OrderBy(r => r.ReminderTime)
											 .Take(5)
											 .ToList();
			}
		}
	}

	public class TodoTaskDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime DueDate { get; set; }
	}

	public class ReminderDto
	{
		public Guid Id { get; set; }
		public string Message { get; set; }
		public DateTime ReminderTime { get; set; }
	}
}