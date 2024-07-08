using AutoMapper;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{

	private readonly IMapper _mapper;
	private readonly ILogger<UserRepository> logger;

	public UserRepository(ApplicationDbContext dbcontext, IMapper mapper, ILogger<UserRepository> logger) : base(dbcontext)
	{

		_mapper = mapper;
		this.logger = logger;
	}

	public async Task<UserDto> GetByIdAsync(Guid id)
	{
		try
		{


			var user = await base.GetByIdAsync(id);
			return _mapper.Map<UserDto>(user);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred while getting User by Id");
			throw;
		}
	}

	public async Task<IEnumerable<UserDto>> GetAllAsync()
	{
		try
		{
			var users = await base.GetAllAsync();
			return _mapper.Map<IEnumerable<UserDto>>(users);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred while getting all User");
			throw;
		}
	}

	public async Task<UserDto> AddAsync(CreateUserDto createUserDto)
	{
		try
		{
			var user = _mapper.Map<User>(createUserDto);
			await base.AddAsync(user);
			return _mapper.Map<UserDto>(user);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred while Adding User");
			throw;
		}
	}

	public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
	{
		try
		{
			var user = await base.GetByIdAsync(id);
			if (user == null)
			{
				return null;
			}
			_mapper.Map(updateUserDto, user);
			base.Update(user);
			return _mapper.Map<UserDto>(user);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred while Updating User");
			throw;
		}
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		try
		{
			var user = await base.GetByIdAsync(id);
			if (user != null)
			{
				base.Remove(user);
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred while Deleting User");
			throw;
		}
	}
}