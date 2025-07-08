using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CodeCraft.NET.Infrastructure.Persistence.Factories
{
	public class BasicIdentityDbContextFactory : IDesignTimeDbContextFactory<BasicIdentityDbContext>
	{
		public BasicIdentityDbContext CreateDbContext(string[] args)
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
			configuration["ConnectionStrings:Identity"] ??
			configuration["IDENTITY_CONNECTION_STRING"];

			if (string.IsNullOrWhiteSpace(connectionString))
				throw new InvalidOperationException("Missing Identity connection string.");

			var optionsBuilder = new DbContextOptionsBuilder<BasicIdentityDbContext>();
			optionsBuilder.UseNpgsql(connectionString);

			return new BasicIdentityDbContext(optionsBuilder.Options);
		}
	}
}
