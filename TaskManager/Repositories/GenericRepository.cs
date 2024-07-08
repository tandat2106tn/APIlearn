using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TaskManager.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected readonly ApplicationDbContext dbContext;

		public GenericRepository(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task AddAsync(T entity)
		{
			await dbContext.Set<T>().AddAsync(entity);
		}

		public async Task AddRangeAsync(IEnumerable<T> entities)
		{
			await dbContext.Set<T>().AddRangeAsync(entities);
		}

		public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expresstion)
		{
			return await dbContext.Set<T>().Where(expresstion).ToListAsync();
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await dbContext.Set<T>().ToListAsync();
		}

		public async Task<T> GetByIdAsync(Guid id)
		{
			return await dbContext.Set<T>().FindAsync(id);
		}

		public void Remove(T entity)
		{
			dbContext.Set<T>().Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			dbContext.Set<T>().RemoveRange(entities);
		}

		public void Update(T entity)
		{
			dbContext.Set<T>().Update(entity);
		}
	}
}
