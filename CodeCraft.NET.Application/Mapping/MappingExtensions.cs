using AutoMapper;
using System;
using System.Linq.Expressions;

namespace CodeCraft.NET.Application.Mapping
{
	public static class MappingExtensions
	{
		public static IMappingExpression<TSource, TDestination> MapFromIdOrDefault<TSource, TDestination, TProperty, TId>(
			this IMappingExpression<TSource, TDestination> mappingExpression,
			Expression<Func<TDestination, TId>> destinationMember,
			Expression<Func<TSource, TProperty>> navigationProperty)
			where TProperty : class
		{
			var navPropFunc = navigationProperty.Compile();

			return mappingExpression.ForMember(destinationMember, opt =>
				opt.MapFrom(src => ExtractIdOrDefault<TId>(navPropFunc(src)))
			);
		}

		private static TId ExtractIdOrDefault<TId>(object? navigationInstance)
		{
			if (navigationInstance == null)
				return default!;

			var idProperty = navigationInstance.GetType().GetProperty("Id");
			if (idProperty == null)
				throw new InvalidOperationException($"Navigation property {navigationInstance.GetType().Name} does not contain an 'Id' property.");

			var idValue = idProperty.GetValue(navigationInstance);

			if (idValue is TId typedId)
				return typedId;

			if (typeof(TId) == typeof(Guid))
				return (TId)(object)Guid.Empty;

			if (typeof(TId) == typeof(Guid?))
				return (TId)(object)(Guid?)null;

			if (typeof(TId) == typeof(int))
				return (TId)(object)0;

			if (typeof(TId) == typeof(int?))
				return (TId)(object)(int?)null;

			throw new InvalidCastException($"Cannot cast 'Id' value to {typeof(TId).Name}.");
		}
	}
}
