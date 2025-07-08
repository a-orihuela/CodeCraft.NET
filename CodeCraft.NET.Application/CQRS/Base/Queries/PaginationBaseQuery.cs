namespace CodeCraft.NET.Application.CQRS.Base.Queries
{
	public class PaginationBaseQuery
	{
		public string? Search { get; set; }
		public string? Sort { get; set; }
		public int PageIndex { get; set; }
		public int PageSize { get; set; }

	}
}
