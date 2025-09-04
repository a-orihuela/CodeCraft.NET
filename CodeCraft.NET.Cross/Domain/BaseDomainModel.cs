namespace CodeCraft.NET.Cross.Domain
{
	public class BaseDomainModel : IAuditable
	{
		public int Id { get; set; }
		public bool Active { get; set; }
		public DateTime CreatedOn { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public string? ModifiedBy { get; set; }
	}
}
