using CodeCraft.NET.Application.DTOs.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCraft.NET.Infrastructure.Persistence.Configuration.OrganizationAccess
{
	public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			var hasher = new PasswordHasher<ApplicationUser>();

			var user = new ApplicationUser
			{
				Id = "f284b3fd-f2cf-476e-a9b6-6560689cc48c",
				Email = "admin@localhost.com",
				NormalizedEmail = "ADMIN@LOCALHOST.COM",
				UserName = "adminuser",
				NormalizedUserName = "ADMINUSER",
				EmailConfirmed = true,
				PasswordHash = "AQAAAAIAAYagAAAAECfeHyfErMhDMdbJOf5KULBdgI6ohallQVWi6k3fYjzQYhmAxlbO/fNzTgDOn04zqA==",
				SecurityStamp = "3c1a5d52-b4e8-4b60-bafe-35f6a7e00001",
				ConcurrencyStamp = "5d2b8e91-1b2a-4b52-aabc-10a5f9f00002"
			};

			builder.HasData(user);
		}
	}
}
