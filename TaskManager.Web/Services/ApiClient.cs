using System.Net.Http.Headers;

namespace TaskManager.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JwtService _jwtService;

        public ApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, JwtService jwtService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]);
            _jwtService = jwtService;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            SetAuthorizationHeader();
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            SetAuthorizationHeader();
            return await _httpClient.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            SetAuthorizationHeader();
            return await _httpClient.PutAsync(url, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            SetAuthorizationHeader();
            return await _httpClient.DeleteAsync(url);
        }

        private void SetAuthorizationHeader()
        {
            var token = _jwtService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}