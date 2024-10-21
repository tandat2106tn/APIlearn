using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TaskManager.Web.Services;

namespace TaskManager.Web.Pages.Reminders
{
	public class DetailsModel : PageModel
	{
		private readonly ApiClient _apiClient;

		public DetailsModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		public ReminderDto Reminder { get; set; }

		public async Task<IActionResult> OnGetAsync(Guid id)
		{
			var response = await _apiClient.GetAsync($"api/reminder/{id}");

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				Reminder = JsonSerializer.Deserialize<ReminderDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				return Page();
			}

			return NotFound();
		}
	}


}