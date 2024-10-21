using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using TaskManager.Web.Services;

namespace TaskManager.Web.Pages.Account
{
	public class RegisterModel : PageModel
	{
		private readonly ApiClient _apiClient;

		public RegisterModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public class InputModel
		{
			[Required]
			[Display(Name = "Name")]
			public string Name { get; set; }

			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }
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
				var registerDto = new
				{
					Name = Input.Name,
					Email = Input.Email,
					Password = Input.Password
				};

				var json = JsonSerializer.Serialize(registerDto);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await _apiClient.PostAsync("api/auth/register", content);

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("./Login");
				}
				else
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string[]>>(responseContent);
					foreach (var error in errorResponse)
					{
						foreach (var errorMessage in error.Value)
						{
							ModelState.AddModelError(string.Empty, errorMessage);
						}
					}
				}
			}

			return Page();
		}
	}
}