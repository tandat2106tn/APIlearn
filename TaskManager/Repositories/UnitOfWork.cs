using AutoMapper;
using TaskManager.Models;

namespace TaskManager.Repositories
{
	public interface IUnitOfWork : IDisposable
	{
		IGenericRepository<Reminder> Reminders { get; }
		IGenericRepository<TodoTask> TodoTasks { get; }
		IGenericRepository<User> Users { get; }
		Task<int> CompleteAsync();

	}
	public class UnitOfWork : IUnitOfWork
	{

		public readonly ApplicationDbContext dbContext;
		public readonly IMapper mapper;


		public IGenericRepository<TodoTask> TodoTasks { get; private set; }
		public IGenericRepository<User> Users { get; private set; }
		public IGenericRepository<Reminder> Reminders { get; private set; }


		public UnitOfWork(ApplicationDbContext dbContext, IMapper mapper, ILoggerFactory loggerFactory)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;

			Reminders = new GenericRepository<Reminder>(this.dbContext, this.mapper);
			TodoTasks = new GenericRepository<TodoTask>(this.dbContext, this.mapper);
			Users = new GenericRepository<User>(this.dbContext, this.mapper);



		}
		public async Task<int> CompleteAsync()
		{
			return await dbContext.SaveChangesAsync();
		}

		public void Dispose()
		{
			dbContext.Dispose();
		}
	}
}
