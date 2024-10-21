namespace TaskManager.Web.Pages
{
    // Pages/UserInfo.cshtml.cs
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Security.Claims;

    public class UserInfoModel : PageModel
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }

        public void OnGet()
        {
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }
    }
}
