using System.Text.Json;

namespace CodeCraft.NET.Cross.Configuration
{
	public class DevEnvironmentConfig
	{
		public string ConnectionString { get; set; } = string.Empty;
		public DockerServiceConfig DockerServices { get; set; } = new DockerServiceConfig();

		private static DevEnvironmentConfig? _instance;
		public static DevEnvironmentConfig Instance => _instance ??= LoadConfig();

		private static DevEnvironmentConfig LoadConfig()
		{
			var locations = new[]
			{
				Path.Combine(AppContext.BaseDirectory, "devenvironment.config.json"),
				"devenvironment.config.json",
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "devenvironment.config.json"),
				Path.Combine(Directory.GetCurrentDirectory(), "devenvironment.config.json")
			};

			string? configPath = locations.FirstOrDefault(File.Exists);

			if (configPath != null)
			{
				var json = File.ReadAllText(configPath);
				var data = JsonSerializer.Deserialize<DevEnvironmentConfig>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (data != null)
					return data;
			}

			// Return default configuration if file not found
			return new DevEnvironmentConfig
			{
				ConnectionString = "Host=localhost;Port=5432;Database=CodeCraftDb;Username=postgres;Password=yourStrong(!)Password;",
				DockerServices = new DockerServiceConfig()
			};
		}

		public string GetSolutionRoot()
		{
			var dir = new DirectoryInfo(AppContext.BaseDirectory);
			while (dir != null && !File.Exists(Path.Combine(dir.FullName, "CodeCraft.NET.sln")))
				dir = dir.Parent;

			if (dir == null)
				throw new InvalidOperationException("Solution root not found.");

			return dir.FullName;
		}
	}

	public class DockerServiceConfig
	{
		public string ComposeFileName { get; set; } = "docker-compose.yml";
		public PostgreSqlConfig PostgreSql { get; set; } = new PostgreSqlConfig();
		public PgAdminConfig PgAdmin { get; set; } = new PgAdminConfig();
	}

	public class PostgreSqlConfig
	{
		public string ContainerName { get; set; } = "codecraft-postgres";
		public string Host { get; set; } = "localhost";
		public int Port { get; set; } = 5432;
		public string Database { get; set; } = "CodeCraftDb";
		public string Username { get; set; } = "postgres";
		public string Password { get; set; } = "yourStrong(!)Password";
	}

	public class PgAdminConfig
	{
		public string ContainerName { get; set; } = "codecraft-pgadmin";
		public string Host { get; set; } = "localhost";
		public int Port { get; set; } = 8080;
		public string Email { get; set; } = "admin@codecraft.com";
		public string Password { get; set; } = "admin123";
	}
}