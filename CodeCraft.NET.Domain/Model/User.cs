using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Domain.Model;
public class User : BaseDomainModel
{
	public string Email { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public int? OrganizationId { get; set; }
	public string IdentityId { get; set; }
}