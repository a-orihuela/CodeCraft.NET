using System.Text.Json;

namespace CodeCraft.NET.Cross.Configuration
{
	public class DevEnvironmentConfig
	{
		public string ConnectionString { get; set; } = string.Empty;

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
				ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=CodeCraftDb;Trusted_Connection=true;MultipleActiveResultSets=true;"
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
}