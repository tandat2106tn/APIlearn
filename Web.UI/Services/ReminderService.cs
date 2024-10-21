using Microsoft.Extensions.Options;
using TaskManagerMVC.ViewModels;
using Web.UI.Models;

namespace TaskManagerMVC.Services
{
    public interface IReminderService
    {
        Task<IEnumerable<ReminderViewModel>> GetAllRemindersAsync();
        Task<ReminderViewModel> GetReminderByIdAsync(Guid id);
        Task<ReminderViewModel> CreateReminderAsync(CreateReminderViewModel model);
        Task<ReminderViewModel> UpdateReminderAsync(Guid id, UpdateReminderViewModel model);
        Task DeleteReminderAsync(Guid id);
    }

    public class ReminderService : BaseApiService, IReminderService
    {
        public ReminderService(IHttpClientFactory clientFactory, IOptions<ApiConfig> apiConfig, ITokenManager tokenManager)
            : base(clientFactory, apiConfig, tokenManager)
        {
        }

        public async Task<IEnumerable<ReminderViewModel>> GetAllRemindersAsync()
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.GetAsync("api/Reminder"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<ReminderViewModel>>();
            }
            return new List<ReminderViewModel>();
        }

        public async Task<ReminderViewModel> GetReminderByIdAsync(Guid id)
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.GetAsync($"api/Reminder/{id}"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ReminderViewModel>();
            }
            return null;
        }

        public async Task<ReminderViewModel> CreateReminderAsync(CreateReminderViewModel model)
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.PostAsJsonAsync("api/Reminder", model));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReminderViewModel>();
        }

        public async Task<ReminderViewModel> UpdateReminderAsync(Guid id, UpdateReminderViewModel model)
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.PutAsJsonAsync($"api/Reminder/{id}", model));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReminderViewModel>();
        }

        public async Task DeleteReminderAsync(Guid id)
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.DeleteAsync($"api/Reminder/{id}"));
            response.EnsureSuccessStatusCode();
        }
    }
}