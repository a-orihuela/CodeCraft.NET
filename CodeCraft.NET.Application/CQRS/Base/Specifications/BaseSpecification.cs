using System.Linq.Expressions;

namespace CodeCraft.NET.Application.CQRS.Base.Specifications
{
	public class BaseSpecification<T> : ISpecification<T>
	{
		public Expression<Func<T, bool>> Criteria { get; }

		public List<Expression<Func<T, object>>> Includes { get; } = [];

		public Expression<Func<T, object>> OrderBy { get; private set; }

		public Expression<Func<T, object>> OrderByDescending { get; private set; }

		public int Take { get; private set; }

		public int Skip { get; private set; }

		public bool IsPagingEnable { get; private set; }

		public BaseSpecification() { }

		public BaseSpecification(Expression<Func<T, bool>> criteria)
		{
			Criteria = criteria;
		}

		protected void AddInclude(Expression<Func<T, object>> include)
		{
			Includes.Add(include);
		}

		protected void AddOrderBy(Expression<Func<T, object>> order)
		{
			OrderBy = order;
		}

		protected void AddOrderByDescending(Expression<Func<T, object>> orderDescending)
		{
			OrderByDescending = orderDescending;
		}

		protected void Paging(int skip, int take)
		{
			Skip = skip;
			Take = take;
			IsPagingEnable = true;
		}
	}
}
