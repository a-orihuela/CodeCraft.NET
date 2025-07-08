using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CodeCraft.NET.Infrastructure.Persistence.Factories
{
	public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			var basePath = Directory.GetCurrentDirectory();

			var serverConfigPath = Path.Combine(basePath, "../CodeCraft.NET.Server/appsettings.json");
			var localConfigPath = Path.Combine(basePath, "appsettings.json");

			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile(File.Exists(localConfigPath) ? "appsettings.json" : serverConfigPath, optional: true)
				.AddEnvironmentVariables()
				.Build();

			var connectionString =
				configuration["ConnectionStrings:Application"] ??
				configuration["APPLICATION_CONNECTION_STRING"];

			if (string.IsNullOrWhiteSpace(connectionString))
				throw new InvalidOperationException("Missing Application connection string.");

			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			optionsBuilder.UseNpgsql(connectionString);

			return new ApplicationDbContext(optionsBuilder.Options);
		}
	}
}
