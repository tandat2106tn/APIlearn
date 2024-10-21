using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using TaskManager.Web.Services;

namespace TaskManager.Web.Pages.Tasks
{
	public class EditModel : PageModel
	{
		private readonly ApiClient _apiClient;

		public EditModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		[BindProperty]
		public UpdateTodoTaskDto Task { get; set; }

		public SelectList Users { get; set; }
		public SelectList TaskTypes { get; set; }
		public SelectList TaskDifficulties { get; set; }

		public async Task<IActionResult> OnGetAsync(Guid id)
		{
			var response = await _apiClient.GetAsync($"api/todotasks/{id}");

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var taskDto = JsonSerializer.Deserialize<UpdateTodoTaskDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				Task = new UpdateTodoTaskDto
				{
					Id = taskDto.Id,
					Title = taskDto.Title,
					Description = taskDto.Description,
					IsCompleted = taskDto.IsCompleted,
					DueDate = taskDto.DueDate,
					TaskTypeId = taskDto.TaskTypeId,
					TaskDifficultyId = taskDto.TaskDifficultyId,
					UserId = taskDto.UserId
				};

				await LoadSelectLists();
				return Page();
			}

			return NotFound();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				Console.WriteLine("ModelState is invalid");
				foreach (var modelState in ModelState.Values)
				{
					foreach (var error in modelState.Errors)
					{
						Console.WriteLine($"Error: {error.ErrorMessage}");
					}
				}
				await LoadSelectLists();
				return Page();
			}

			try
			{
				var json = JsonSerializer.Serialize(Task);
				Console.WriteLine($"Sending update request: {json}");

				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await _apiClient.PutAsync($"api/todotasks/{Task.Id}", content);

				Console.WriteLine($"Received response: {response.StatusCode}");

				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					Console.WriteLine($"Response content: {responseContent}");
					return RedirectToPage("./Index");
				}
				else
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					Console.WriteLine($"Error content: {errorContent}");
					ModelState.AddModelError(string.Empty, $"Error updating task. Status: {response.StatusCode}. Details: {errorContent}");
					await LoadSelectLists();
					return Page();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception occurred: {ex}");
				ModelState.AddModelError(string.Empty, $"An error occurred while updating the task: {ex.Message}");
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

	public class UpdateTodoTaskDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime DueDate { get; set; }
		public Guid TaskTypeId { get; set; }
		public Guid TaskDifficultyId { get; set; }
		public Guid UserId { get; set; }
	}


}