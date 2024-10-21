using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TaskManager.Web.Services;

namespace TaskManager.Web.Pages.Tasks
{

    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public List<TodoTaskDto> Tasks { get; set; }
        [Authorize]
        public async Task OnGetAsync()
        {
            var response = await _apiClient.GetAsync("api/todotasks");
            Console.WriteLine($"API Response Status: {response.StatusCode}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response Content: {content}");
                Tasks = JsonSerializer.Deserialize<List<TodoTaskDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error Content: {errorContent}");
                Tasks = new List<TodoTaskDto>();
                ModelState.AddModelError(string.Empty, $"Error retrieving tasks. Status: {response.StatusCode}");
            }
        }
    }
}