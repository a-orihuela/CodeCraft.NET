using CodeCraft.NET.Application.CQRS.Base.Specifications;
using CodeCraft.NET.Cross.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodeCraft.NET.Infrastructure.Specifications
{
	internal class SpecificationEvaluator<T> where T : BaseDomainModel
	{
		public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
		{
			if (spec.Criteria != null)
			{
				query = query.Where(spec.Criteria);
			}

			if (spec.OrderBy != null)
			{
				query.OrderBy(spec.OrderBy);
			}

			if (spec.OrderByDescending != null)
			{
				query.OrderBy(spec.OrderByDescending);
			}

			if (spec.IsPagingEnable)
			{
				query = query.Skip(spec.Skip).Take(spec.Take);
			}

			query = spec.Includes.Aggregate(query, (c, i) => c.Include(i));

			return query;
		}
	}
}
