using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Web.Services;

namespace TaskManager.Web.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly JwtService _jwtService;

        public LogoutModel(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public void OnGet()
        {
            // This method is called when the logout page is accessed directly.
            // We don't want to perform the logout action here.
        }

        public IActionResult OnPost(string returnUrl = null)
        {
            _jwtService.RemoveToken();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage("/Index");
            }
        }
    }
}