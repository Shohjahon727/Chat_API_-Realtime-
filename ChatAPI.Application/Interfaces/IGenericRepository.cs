using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatAPI.Application.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
		Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
		Task AddAsync(T entity, CancellationToken cancellationToken = default);
		void Update(T entity);
		void Delete(T entity);
		IQueryable<T> Query();

		Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
		Task<int> CountAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default);
	}
}
