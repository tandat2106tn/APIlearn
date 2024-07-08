using System.Linq.Expressions;

namespace TaskManager.Repositories
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T> GetByIdAsync(Guid id);
		Task<IEnumerable<T>> GetAllAsync();
		Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expresstion);
		Task AddAsync(T entity);
		Task AddRangeAsync(IEnumerable<T> entities);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entities);
		void Update(T entity);

	}
}
