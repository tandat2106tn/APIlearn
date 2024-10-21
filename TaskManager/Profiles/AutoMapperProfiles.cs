using AutoMapper;
using TaskManager.DTOs;
using TaskManager.Models;

namespace TaskManager.Profiles
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<User, UserDto>();
			CreateMap<CreateUserDto, User>();
			CreateMap<UpdateUserDto, User>();



			CreateMap<TodoTask, TodoTaskDto>()
			.ForMember(dest => dest.TaskTypeName, opt => opt.MapFrom(src => src.TaskType.Name))
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
			.ForMember(dest => dest.TaskDifficultyName, opt => opt.MapFrom(src => src.TaskDifficulty.Name)).ReverseMap();

			CreateMap<CreateTodoTaskDto, TodoTask>().ReverseMap();
			CreateMap<UpdateTodoTaskDto, TodoTask>().ReverseMap();


			CreateMap<Reminder, ReminderDto>();
			CreateMap<CreateReminderDto, Reminder>();
			CreateMap<UpdateReminderDto, Reminder>();


		}
	}
}
