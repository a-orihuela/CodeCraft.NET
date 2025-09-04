
namespace CodeCraft.NET.Cross.Domain
{
	public interface IAuditable
	{
		DateTime CreatedOn { get; set; }
		string? CreatedBy { get; set; }
		DateTime? ModifiedOn { get; set; }
		string? ModifiedBy { get; set; }
	}
}
