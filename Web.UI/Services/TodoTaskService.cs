using Microsoft.Extensions.Options;
using TaskManager.Models;
using TaskManagerMVC.ViewModels;
using Web.UI.Models;

namespace TaskManagerMVC.Services
{
    public interface ITodoTaskService
    {
        Task<IEnumerable<TodoTaskViewModel>> GetAllTasksAsync();
        Task<TodoTaskViewModel> GetTaskByIdAsync(Guid id);
        Task<TodoTaskViewModel> CreateTaskAsync(CreateTodoTaskViewModel model);
        Task<TodoTaskViewModel> UpdateTaskAsync(Guid id, UpdateTodoTaskViewModel model);
        Task DeleteTaskAsync(Guid id);
        Task<IEnumerable<TaskType>> GetTaskTypesAsync();
        Task<IEnumerable<TaskDifficulty>> GetTaskDifficultiesAsync();
    }

    public class TodoTaskService : BaseApiService, ITodoTaskService
    {
        public TodoTaskService(IHttpClientFactory clientFactory, IOptions<ApiConfig> apiConfig, ITokenManager tokenManager)
            : base(clientFactory, apiConfig, tokenManager)
        {
        }

        public async Task<IEnumerable<TodoTaskViewModel>> GetAllTasksAsync()
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.GetAsync("api/TodoTasks"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<TodoTaskViewModel>>();
            }
            return new List<TodoTaskViewModel>();
        }

        public async Task<TodoTaskViewModel> GetTaskByIdAsync(Guid id)
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.GetAsync($"api/TodoTasks/{id}"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TodoTaskViewModel>();
            }
            return null;
        }

        public async Task<TodoTaskViewModel> CreateTaskAsync(CreateTodoTaskViewModel model)
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.PostAsJsonAsync("api/TodoTasks", model));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TodoTaskViewModel>();
        }

        public async Task<TodoTaskViewModel> UpdateTaskAsync(Guid id, UpdateTodoTaskViewModel model)
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.PutAsJsonAsync($"api/TodoTasks/{id}", model));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TodoTaskViewModel>();
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.DeleteAsync($"api/TodoTasks/{id}"));
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<TaskType>> GetTaskTypesAsync()
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.GetAsync("api/TodoTasks/taskTypes"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<TaskType>>();
            }
            return new List<TaskType>();
        }

        public async Task<IEnumerable<TaskDifficulty>> GetTaskDifficultiesAsync()
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.GetAsync("api/TodoTasks/taskDifficulties"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<TaskDifficulty>>();
            }
            return new List<TaskDifficulty>();
        }
    }
}