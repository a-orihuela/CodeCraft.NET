using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CodeCraft.NET.Domain.Model;

namespace CodeCraft.NET.Infrastructure.Persistence.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("Users");
		builder.HasKey(u => u.Id);
		builder.HasIndex(u => u.IdentityId).IsUnique();
		
		builder.Property(u => u.Email).HasMaxLength(100).IsRequired();
		builder.Property(u => u.FullName).HasMaxLength(100).IsRequired();
	}
}

