using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Repositories;

public class UserRepository : IUserRepository
{
	private readonly UnitOfWork unitOfWork;

	public UserRepository(UnitOfWork unitOfWork)
	{
		this.unitOfWork = unitOfWork;
	}


	public async Task<UserDto> GetByIdAsync(Guid id)
	{
		var user = await unitOfWork.Users.GetByIdAsync(id);
		return unitOfWork.mapper.Map<UserDto>(user);

	}

	public async Task<IEnumerable<UserDto>> GetAllAsync()
	{

		var users = await unitOfWork.Users.GetAllAsync();
		return unitOfWork.mapper.Map<IEnumerable<UserDto>>(users);



	}

	public async Task<UserDto> AddAsync(CreateUserDto createUserDto)
	{
		var user = unitOfWork.mapper.Map<User>(createUserDto);
		await unitOfWork.Users.AddAsync(user);
		await unitOfWork.CompleteAsync();
		return unitOfWork.mapper.Map<UserDto>(user);

	}

	public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
	{

		var user = unitOfWork.mapper.Map<User>(updateUserDto);
		if (user == null)
		{
			return null;
		}
		unitOfWork.Users.Update(user, id);
		await unitOfWork.CompleteAsync();
		return unitOfWork.mapper.Map<UserDto>(user);

	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var user = await unitOfWork.Users.GetByIdAsync(id);
		if (user == null)
		{
			return false;
		}
		unitOfWork.Users.Remove(user);
		await unitOfWork.CompleteAsync();
		return true;
	}
}