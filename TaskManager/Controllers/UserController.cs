using Microsoft.AspNetCore.Mvc;
using TaskManager.DTOs;
using TaskManager.Repositories;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UserController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }


    [HttpGet]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        var createdUser = userRepository.AddAsync(createUserDto);

        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id}")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto updateUserDto)
    {
        var updatedUser = await userRepository.UpdateAsync(id, updateUserDto);
        if (updatedUser == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminRole")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await userRepository.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

}