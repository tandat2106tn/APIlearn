using Microsoft.Extensions.Options;
using TaskManagerMVC.ViewModels;
using Web.UI.Models;

namespace TaskManagerMVC.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
        Task<UserViewModel> GetUserByIdAsync(Guid id);
        Task<UserViewModel> UpdateUserAsync(Guid id, UpdateUserViewModel model);
        Task DeleteUserAsync(Guid id);
    }

    public class UserService : BaseApiService, IUserService
    {
        public UserService(IHttpClientFactory clientFactory, IOptions<ApiConfig> apiConfig, ITokenManager tokenManager)
            : base(clientFactory, apiConfig, tokenManager)
        {
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.GetAsync("api/User"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<UserViewModel>>();
            }
            return new List<UserViewModel>();
        }

        public async Task<UserViewModel> GetUserByIdAsync(Guid id)
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.GetAsync($"api/User/{id}"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserViewModel>();
            }
            return null;
        }

        public async Task<UserViewModel> UpdateUserAsync(Guid id, UpdateUserViewModel model)
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.PutAsJsonAsync($"api/User/{id}", model));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserViewModel>();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var response = await SendAuthorizedRequestAsync(() => _httpClient.DeleteAsync($"api/User/{id}"));
            response.EnsureSuccessStatusCode();
        }
    }
}