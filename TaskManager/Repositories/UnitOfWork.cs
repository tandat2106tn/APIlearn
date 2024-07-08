using AutoMapper;

namespace TaskManager.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{

		public readonly ApplicationDbContext dbContext;
		private readonly IMapper mapper;
		private readonly ILoggerFactory loggerFactory;

		public ITodoTaskRepository TodoTasks { get; private set; }
		public IUserRepository Users { get; private set; }


		public UnitOfWork(ApplicationDbContext dbContext, IMapper mapper, ILoggerFactory loggerFactory)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
			this.loggerFactory = loggerFactory;
			TodoTasks = new TodoTaskRepository(this.dbContext, this.mapper, loggerFactory.CreateLogger<TodoTaskRepository>());
			Users = new UserRepository(this.dbContext, this.mapper, loggerFactory.CreateLogger<UserRepository>());


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
