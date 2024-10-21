using System.Text.Json;
using TaskManagerMVC.ViewModels;

namespace TaskManagerMVC.Services
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(LoginViewModel model);
        Task<AuthResult> RegisterAsync(RegisterViewModel model);
        Task LogoutAsync();
        string GetToken();
        void SetToken(string token);
    }

    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }




    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenManager _tokenManager;

        public AuthService(IHttpClientFactory clientFactory, ITokenManager tokenManager)
        {
            _httpClient = clientFactory.CreateClient("TaskManagerApi");
            _tokenManager = tokenManager;
        }

        public async Task<AuthResult> LoginAsync(LoginViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResult>();

                if (result.Succeeded && !string.IsNullOrEmpty(result.Token))
                {
                    _tokenManager.SetToken(result.Token);
                }
                return result;
            }
            return new AuthResult { Succeeded = false, Errors = new List<string> { "Invalid login attempt" } };
        }

        public async Task<AuthResult> RegisterAsync(RegisterViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", model);
            if (response.IsSuccessStatusCode)
            {
                return new AuthResult { Succeeded = true };
            }
            var content = await response.Content.ReadAsStringAsync();
            var errors = JsonSerializer.Deserialize<List<string>>(content);
            return new AuthResult { Succeeded = false, Errors = errors };
        }

        public Task LogoutAsync()
        {
            _tokenManager.SetToken(null);

            return Task.CompletedTask;
        }

        public string GetToken()
        {
            return _tokenManager.GetToken();
        }

        public void SetToken(string token)
        {
            _tokenManager.SetToken(token);
        }
    }
}