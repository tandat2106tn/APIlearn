using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Web.UI.Models;

public abstract class BaseApiService
{
    protected readonly HttpClient _httpClient;
    protected readonly ApiConfig _apiConfig;
    protected readonly ITokenManager _tokenManager;

    public BaseApiService(IHttpClientFactory clientFactory, IOptions<ApiConfig> apiConfig, ITokenManager tokenManager)
    {
        _httpClient = clientFactory.CreateClient("TaskManagerApi");
        _apiConfig = apiConfig.Value;
        _tokenManager = tokenManager;
    }

    protected async Task<HttpResponseMessage> SendAuthorizedRequestAsync(Func<Task<HttpResponseMessage>> requestFunc)
    {
        var token = _tokenManager.GetToken();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine($"Token used: {token}"); // Temporary logging
        }
        else
        {
            Console.WriteLine("No token found"); // Temporary logging
        }

        var response = await requestFunc();

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _tokenManager.SetToken(null);
            Console.WriteLine("Unauthorized response received, token cleared"); // Temporary logging
            throw new UnauthorizedAccessException("Authentication failed. Please log in again.");
        }

        return response;
    }
}