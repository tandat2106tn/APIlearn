using TaskManager.DTOs;

namespace TaskManager.Repositories
{

	public interface IUserRepository
	{
		Task<UserDto> GetByIdAsync(Guid id);
		Task<IEnumerable<UserDto>> GetAllAsync();
		Task<UserDto> AddAsync(CreateUserDto createUserDto);
		Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto);
		Task<bool> DeleteAsync(Guid id);
	}
}
