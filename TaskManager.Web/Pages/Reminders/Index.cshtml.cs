using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TaskManager.Web.Services;


namespace TaskManager.Web.Pages.Reminders
{
	public class IndexModel : PageModel
	{
		private readonly ApiClient _apiClient;

		public IndexModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		public List<ReminderDto> Reminders { get; set; }

		public async Task OnGetAsync()
		{
			var response = await _apiClient.GetAsync("api/reminder");
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				Reminders = JsonSerializer.Deserialize<List<ReminderDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			}
			else
			{
				Reminders = new List<ReminderDto>();
				ModelState.AddModelError(string.Empty, "Error retrieving reminders.");
			}
		}
	}

	public class ReminderDto
	{
		public Guid Id { get; set; }
		public string Message { get; set; }
		public DateTime ReminderTime { get; set; }
		public bool IsTriggered { get; set; }
		public Guid TodoTaskId { get; set; }
		public string TaskTitle { get; set; }
	}
}