using MediatR;

namespace CodeCraft.NET.Application.CQRS.Base.Queries
{
	public class EmptyPaginationBaseQuery : PaginationBaseQuery, IRequest<Pagination<object>>
	{
	}
}
