using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly JwtService jwtService;

		public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			this.jwtService = jwtService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterDto model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = new User { UserName = model.Email, Email = model.Email, Name = model.Name };
			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				return Ok(new { message = "Registration successful" });
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return BadRequest(ModelState);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginDto model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

			if (result.Succeeded)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				var roles = await _userManager.GetRolesAsync(user);
				var token = jwtService.GenerateToken(user, roles);
				return Ok(new { token = token, userId = user.Id, roles = roles });
			}

			ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			return Unauthorized(ModelState);
		}


	}
}