using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using TaskManager.Web.Services;

namespace TaskManager.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApiClient _apiClient;
        private readonly JwtService _jwtService;

        public LoginModel(ApiClient apiClient, JwtService jwtService)
        {
            _apiClient = apiClient;
            _jwtService = jwtService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var loginDto = new
                {
                    Email = Input.Email,
                    Password = Input.Password
                };

                var json = JsonSerializer.Serialize(loginDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _apiClient.PostAsync("api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    // API login successful, now create our own JWT
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResult = JsonSerializer.Deserialize<LoginResult>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Create and set our own JWT
                    var token = _jwtService.GenerateToken(loginResult.UserId, Input.Email, loginResult.Roles);
                    _jwtService.SetToken(token);

                    return LocalRedirect(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return Page();
        }
    }

    public class LoginResult
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}