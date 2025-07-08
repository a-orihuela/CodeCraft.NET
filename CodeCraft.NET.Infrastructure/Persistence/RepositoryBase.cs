using CodeCraft.NET.Application.Contracts.Persistence.Base;
using CodeCraft.NET.Application.CQRS.Base.Specifications;
using CodeCraft.NET.Cross.Domain;
using CodeCraft.NET.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CodeCraft.NET.Infrastructure.Persistence
{
	public abstract partial class RepositoryBase<T, TContext> : IAsyncRepository<T>
		where T : BaseDomainModel
		where TContext : DbContext
	{
		protected readonly TContext _context;
		protected TContext Context => _context;

		public RepositoryBase(TContext context)
		{
			_context = context;
		}

		public virtual async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
		{
			return await _context.Set<T>().Where(predicate).ToListAsync();
		}

		public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
									   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
									   string includeString = null,
									   bool disableTracking = true)
		{
			IQueryable<T> query = _context.Set<T>();
			if (disableTracking) query = query.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

			if (predicate != null) query = query.Where(predicate);

			if (orderBy != null)
				return await orderBy(query).ToListAsync();


			return await query.ToListAsync();
		}

		public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
									 Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
									 List<Expression<Func<T, object>>> includes = null,
									 bool disableTracking = true)
		{

			IQueryable<T> query = _context.Set<T>();
			if (disableTracking) query = query.AsNoTracking();

			if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

			if (predicate != null) query = query.Where(predicate);

			if (orderBy != null)
				return await orderBy(query).ToListAsync();


			return await query.ToListAsync();
		}

		public virtual async Task<T> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public virtual async Task<T> AddAsync(T entity)
		{
			_context.Set<T>().Add(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public virtual async Task<T> UpdateAsync(T entity)
		{
			_context.Set<T>().Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return entity;
		}

		public virtual async Task DeleteAsync(T entity)
		{
			_context.Set<T>().Remove(entity);
			await _context.SaveChangesAsync();
		}

		public virtual void AddEntity(T entity)
		{
			_context.Set<T>().Add(entity);
		}

		public virtual void UpdateEntity(T entity)
		{
			_context.Set<T>().Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}

		public virtual void DeleteEntity(T entity)
		{
			_context.Set<T>().Remove(entity);
		}

		public virtual async Task<T> GetByIdWithSpecifications(ISpecification<T> spec)
		{
			return await ApplySpecification(spec).FirstOrDefaultAsync();
		}

		public virtual async Task<IReadOnlyList<T>> GetAllWithSpecifications(ISpecification<T> spec)
		{
			return await ApplySpecification(spec).ToListAsync();
		}

		public virtual async Task<int> CountAsync(ISpecification<T> spec)
		{
			return await ApplySpecification(spec).CountAsync();
		}

		public virtual IQueryable<T> ApplySpecification(ISpecification<T> spec)
		{
			return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
		}

		public virtual async Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate)
		{
			return await _context.Set<T>().Where(predicate).ToListAsync();
		}

	}
}
