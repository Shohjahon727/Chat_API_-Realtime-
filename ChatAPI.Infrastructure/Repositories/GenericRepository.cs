using ChatAPI.Application.Interfaces;
using ChatAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatAPI.Infrastructure.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected readonly AppDbContext _context;
		protected readonly DbSet<T> _dbSet;

		public GenericRepository(AppDbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
			=> await _dbSet.FindAsync(id, cancellationToken);

		public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
			=> await _dbSet.ToListAsync(cancellationToken);

		public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
			=> await _dbSet.AddAsync(entity, cancellationToken);

		public void Update(T entity)
			=> _dbSet.Update(entity);

		public void Delete(T entity)
			=> _dbSet.Remove(entity);

		public IQueryable<T> Query()
			=> _dbSet.AsQueryable();

		public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
			=> await _dbSet.AnyAsync(predicate, cancellationToken);

		public async Task<int> CountAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
			=> predicate == null ? await _dbSet.CountAsync(cancellationToken) : await _dbSet.CountAsync(predicate, cancellationToken);
	}
}
