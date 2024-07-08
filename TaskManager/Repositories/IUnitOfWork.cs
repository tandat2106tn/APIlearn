namespace TaskManager.Repositories
{
	public interface IUnitOfWork : IDisposable
	{
		ITodoTaskRepository TodoTasks { get; }
		IUserRepository Users { get; }
		Task<int> CompleteAsync();

	}
}
