using CodeCraft.NET.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class MigrationRunner
{
	public static async Task ApplyMigrationsAsync(IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();

		var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		var identityContext = scope.ServiceProvider.GetRequiredService<BasicIdentityDbContext>();

		await appContext.Database.MigrateAsync();
		await identityContext.Database.MigrateAsync();
	}
}
