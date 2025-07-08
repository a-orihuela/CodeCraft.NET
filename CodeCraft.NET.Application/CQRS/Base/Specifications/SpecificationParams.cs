namespace CodeCraft.NET.Application.CQRS.Base.Specifications
{
	public abstract class SpecificationParams
	{
		private const int MaxPageSize = 50;

		public int? Id { get; set; }
		public string? Sort { get; set; }
		public int PageIndex { get; set; }
		public int PageCount { get; set; }

		private int _pageSize = 3;
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
		}

		public string? Search { get; set; }
	}
}
