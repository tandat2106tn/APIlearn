using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TaskManager.Web.Services;

namespace TaskManager.Web.Pages.Tasks
{
	public class DetailsModel : PageModel
	{
		private readonly ApiClient _apiClient;

		public DetailsModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		public TodoTaskDto Task { get; set; }

		public async Task<IActionResult> OnGetAsync(Guid id)
		{
			var response = await _apiClient.GetAsync($"api/todotasks/{id}");

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				Task = JsonSerializer.Deserialize<TodoTaskDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				return Page();
			}

			return NotFound();
		}
	}

	public class TodoTaskDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime DueDate { get; set; }
		public string UserName { get; set; }
		public string TaskTypeName { get; set; }
		public string TaskDifficultyName { get; set; }
	}
}