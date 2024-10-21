using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TaskManager.Web.Services;

namespace TaskManager.Web.Pages.Tasks
{
	public class DeleteModel : PageModel
	{
		private readonly ApiClient _apiClient;

		public DeleteModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		[BindProperty]
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

		public async Task<IActionResult> OnPostAsync(Guid id)
		{
			var response = await _apiClient.DeleteAsync($"api/todotasks/{id}");

			if (response.IsSuccessStatusCode)
			{
				return RedirectToPage("./Index");
			}

			return Page();
		}
	}


}