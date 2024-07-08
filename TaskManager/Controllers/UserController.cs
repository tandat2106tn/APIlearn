using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.DTOs;
using TaskManager.Repositories;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
	private readonly IUnitOfWork unitOfWork;

	public UserController(IUnitOfWork unitOfWork)
	{
		this.unitOfWork = unitOfWork;
	}

	[HttpGet]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
	public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
	{
		var users = await unitOfWork.Users.GetAllAsync();
		return Ok(users);
	}

	[HttpGet("{id}")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
	public async Task<ActionResult<UserDto>> GetUser(Guid id)
	{
		var user = await unitOfWork.Users.GetByIdAsync(id);
		if (user == null)
		{
			return NotFound();
		}
		return Ok(user);
	}

	[HttpPost]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
	public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
	{
		var createdUser = await unitOfWork.Users.AddAsync(createUserDto);
		await unitOfWork.CompleteAsync();
		return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
	}

	[HttpPut("{id}")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
	public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto updateUserDto)
	{
		var updatedUser = await unitOfWork.Users.UpdateAsync(id, updateUserDto);
		if (updatedUser == null)
		{
			return NotFound();
		}
		await unitOfWork.CompleteAsync();
		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
	public async Task<IActionResult> DeleteUser(Guid id)
	{
		var result = await unitOfWork.Users.DeleteAsync(id);
		if (!result)
		{
			return NotFound();
		}
		await unitOfWork.CompleteAsync();
		return NoContent();
	}

}