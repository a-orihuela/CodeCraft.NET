using CodeCraft.NET.Application.DTOs.Identity;
using CodeCraft.NET.Infrastructure.Persistence.Configuration.OrganizationAccess;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeCraft.NET.Infrastructure.Persistence
{
	public class BasicIdentityDbContext : IdentityDbContext<ApplicationUser>
	{
		public BasicIdentityDbContext(DbContextOptions<BasicIdentityDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfiguration(new IdentityRoleConfiguration());
			builder.ApplyConfiguration(new ApplicationUserConfiguration());
			builder.ApplyConfiguration(new ApplicationUserRoleConfiguration());
		}
	}
}
