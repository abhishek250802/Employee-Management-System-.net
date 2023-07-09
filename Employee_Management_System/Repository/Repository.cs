
using EmployeeManagement.Data;
using EmployeeManagement.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeManagement.Repository
{
    public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _dbContext;
		internal DbSet<T> dbSet;
		public Repository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
			this.dbSet = _dbContext.Set<T>();
		}
		public async Task CreateAsync(T entity)
		{
			
			await dbSet.AddAsync(entity);
			await SaveAsync();
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool tracked, string? includeProperties = null)
		{
			IQueryable<T> query = dbSet;
			if (!tracked)
			{
				query = query.AsNoTracking();
			}
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (includeProperties != null)
			{
				foreach (var includProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includProp);
				}
			}

			return await query.FirstOrDefaultAsync();
		}

		public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
		{
			IQueryable<T> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (includeProperties != null)
			{
				foreach (var includProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includProp);
				}
			}

			return await query.ToListAsync();
		}

		public async Task RemoveAsync(T entity)
		{
			dbSet.Remove(entity);
			await SaveAsync();
		}

		public async Task SaveAsync()
		{
			await _dbContext.SaveChangesAsync();
		}
	}
}
