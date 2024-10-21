using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using TaskManager.Web.Services;

namespace TaskManager.Web.Pages.Tasks
{
	public class CreateModel : PageModel
	{
		private readonly ApiClient _apiClient;

		public CreateModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		[BindProperty]
		public CreateTodoTaskDto Task { get; set; }

		public SelectList Users { get; set; }
		public SelectList TaskTypes { get; set; }
		public SelectList TaskDifficulties { get; set; }

		public async Task OnGetAsync()
		{
			Task = new CreateTodoTaskDto
			{
				IsCompleted = false // Set default value
			};
			await LoadSelectLists();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				await LoadSelectLists();
				return Page();
			}

			var json = JsonSerializer.Serialize(Task);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _apiClient.PostAsync("api/todotasks", content);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToPage("./Index");
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				ModelState.AddModelError(string.Empty, $"Error creating task. Status: {response.StatusCode}. Details: {errorContent}");
				await LoadSelectLists();
				return Page();
			}
		}

		private async Task LoadSelectLists()
		{
			var usersResponse = await _apiClient.GetAsync("api/user");
			var taskTypesResponse = await _apiClient.GetAsync("api/todotasks/taskTypes");
			var taskDifficultiesResponse = await _apiClient.GetAsync("api/todotasks/taskDifficulties");

			if (usersResponse.IsSuccessStatusCode)
			{
				var usersContent = await usersResponse.Content.ReadAsStringAsync();
				var users = JsonSerializer.Deserialize<List<UserDto>>(usersContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				Users = new SelectList(users, "Id", "Name");
			}

			if (taskTypesResponse.IsSuccessStatusCode)
			{
				var taskTypesContent = await taskTypesResponse.Content.ReadAsStringAsync();
				var taskTypes = JsonSerializer.Deserialize<List<TaskTypeDto>>(taskTypesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				TaskTypes = new SelectList(taskTypes, "Id", "Name");
			}

			if (taskDifficultiesResponse.IsSuccessStatusCode)
			{
				var taskDifficultiesContent = await taskDifficultiesResponse.Content.ReadAsStringAsync();
				var taskDifficulties = JsonSerializer.Deserialize<List<TaskDifficultyDto>>(taskDifficultiesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				TaskDifficulties = new SelectList(taskDifficulties, "Id", "Name");
			}
		}
	}

	public class CreateTodoTaskDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime DueDate { get; set; }
		public Guid UserId { get; set; }
		public bool IsCompleted { get; set; }
		public Guid TaskTypeId { get; set; }
		public Guid TaskDifficultyId { get; set; }
	}

	public class UserDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}

	public class TaskTypeDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}

	public class TaskDifficultyDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}