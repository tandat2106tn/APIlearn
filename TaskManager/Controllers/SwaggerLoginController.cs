using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SwaggerLoginController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly JwtService _jwtService;

		public SwaggerLoginController(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtService = jwtService;
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromForm] LoginDto model)
		{
			var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

			if (result.Succeeded)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				var roles = await _userManager.GetRolesAsync(user);
				var token = _jwtService.GenerateToken(user, roles);
				return Ok(new { token = token });
			}

			return Unauthorized("Invalid login attempt.");
		}
	}
}