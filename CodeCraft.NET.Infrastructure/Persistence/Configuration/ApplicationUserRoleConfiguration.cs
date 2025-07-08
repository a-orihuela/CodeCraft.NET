using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCraft.NET.Infrastructure.Persistence.Configuration.OrganizationAccess
{
	public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
		{
			builder.HasKey(r => new { r.UserId, r.RoleId });
			builder.HasData(
				new IdentityUserRole<string>
				{
					RoleId = "79ba8e3f-5c28-42cb-a03e-babcfb0b5bd8",
					UserId = "f284b3fd-f2cf-476e-a9b6-6560689cc48c"
				}
			);
		}
	}
}
