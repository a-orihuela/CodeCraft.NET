using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCraft.NET.Infrastructure.Persistence.Configuration.OrganizationAccess
{
	public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
	{
		public void Configure(EntityTypeBuilder<IdentityRole> builder)
		{
			_ = builder.HasData(
				new IdentityRole
				{
					Id = "79ba8e3f-5c28-42cb-a03e-babcfb0b5bd8",
					Name = "Administrator",
					NormalizedName = "ADMINISTRATOR"
				}
			);
		}
	}
}
