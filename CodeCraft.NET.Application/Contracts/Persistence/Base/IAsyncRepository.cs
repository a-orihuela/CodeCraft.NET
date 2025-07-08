using CodeCraft.NET.Application.CQRS.Base.Specifications;
using CodeCraft.NET.Cross.Domain;
using System.Linq.Expressions;

namespace CodeCraft.NET.Application.Contracts.Persistence.Base
{
	public interface IAsyncRepository<T> where T : BaseDomainModel
	{
		Task<IReadOnlyList<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
		Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
									   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
									   string includeString = null,
									   bool disableTracking = true);
		Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
									   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
									   List<Expression<Func<T, object>>> includes = null,
									   bool disableTracking = true);

		Task<T> AddAsync(T entity);
		Task<T> UpdateAsync(T entity);
		Task DeleteAsync(T entity);

		void AddEntity(T entity);
		void UpdateEntity(T entity);
		void DeleteEntity(T entity);

		Task<T> GetByIdWithSpecifications(ISpecification<T> spec);
		Task<IReadOnlyList<T>> GetAllWithSpecifications(ISpecification<T> spec);
		Task<int> CountAsync(ISpecification<T> spec);

		Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate);
	}
}
