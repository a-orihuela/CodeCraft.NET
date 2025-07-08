using CodeCraft.NET.Application.Contracts.Persistence.Base;
using CodeCraft.NET.Application.CQRS.Base.Specifications;
using CodeCraft.NET.Cross.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CodeCraft.NET.Infrastructure.Persistence
{
	internal class AsyncRepositoryAdapter<T> : IAsyncRepository<T> where T : BaseDomainModel
	{
		private readonly RepositoryBase<T, DbContext> _repository;

		public AsyncRepositoryAdapter(RepositoryBase<T, DbContext> repository)
		{
			_repository = repository;
		}

		public Task<T> AddAsync(T entity)
		{
			return _repository.AddAsync(entity);
		}

		public void AddEntity(T entity)
		{
			_repository.AddEntity(entity);
		}

		public Task<int> CountAsync(ISpecification<T> spec)
		{
			return _repository.CountAsync(spec);
		}

		public Task DeleteAsync(T entity)
		{
			return _repository.DeleteAsync(entity);
		}

		public void DeleteEntity(T entity)
		{
			_repository.DeleteEntity(entity);
		}

		public Task<IReadOnlyList<T>> GetAllAsync()
		{
			return _repository.GetAllAsync();
		}

		public Task<IReadOnlyList<T>> GetAllWithSpecifications(ISpecification<T> spec)
		{
			return _repository.GetAllWithSpecifications(spec);
		}

		public Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
		{
			return _repository.GetAsync(predicate);
		}

		public Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
		{
			return _repository.GetAsync(predicate, orderBy, includeString, disableTracking);
		}

		public Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
		{
			return _repository.GetAsync(predicate, orderBy, includes, disableTracking);
		}

		public Task<T> GetByIdAsync(int id)
		{
			return _repository.GetByIdAsync(id);
		}

		public Task<T> GetByIdWithSpecifications(ISpecification<T> spec)
		{
			return _repository.GetByIdWithSpecifications(spec);
		}

		public Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate)
		{
			return _repository.ListAsync(predicate);
		}

		public Task<T> UpdateAsync(T entity)
		{
			return _repository.UpdateAsync(entity);
		}

		public void UpdateEntity(T entity)
		{
			_repository.UpdateEntity(entity);
		}
	}
}
