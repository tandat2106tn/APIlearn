using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
		void Update(T entity, Guid id);

	}
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected readonly ApplicationDbContext dbContext;
		protected readonly IMapper mapper;
		protected readonly DbSet<T> entities;

		public GenericRepository(ApplicationDbContext dbContext, IMapper mapper)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
			entities = dbContext.Set<T>();
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

		public void Update(T entity, Guid id)
		{
			var existingEntity = dbContext.Set<T>().Find(id);
			if (existingEntity != null)
			{
				dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
			}
			else
			{
				throw new KeyNotFoundException($"Entity with id {id} not found.");
			}
		}
	}
}
