using Scriban;
using System.Text.Json;

namespace CodeCraft.NET.Generator.Models
{
	public class CodeCraftConfig
	{
		public DataBaseConfig DataBaseConfig { get; set; } = new DataBaseConfig();
		public Files Files { get; set; } = new Files();
		public Folders Folders { get; set; } = new Folders();
		public ProjectNames ProjectNames { get; set; } = new ProjectNames();
		public Templates Templates { get; set; } = new Templates();

		private static CodeCraftConfig? _instance;
		public static CodeCraftConfig Instance => _instance ??= LoadConfig();

		private static CodeCraftConfig LoadConfig()
		{
			var locations = new[]
			{
				Path.Combine(AppContext.BaseDirectory, "codecraft.config.json"),
				"codecraft.config.json",
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "codecraft.config.json"),
				Path.Combine(Directory.GetCurrentDirectory(), "codecraft.config.json")
			};

			string? configPath = locations.FirstOrDefault(File.Exists)
				?? throw new FileNotFoundException("Configuration file 'codecraft.config.json' not found.");

			var json = File.ReadAllText(configPath);
			var data = JsonSerializer.Deserialize<CodeCraftConfig>(json, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			}) ?? throw new InvalidOperationException("Invalid config format.");

			return data;
		}

		// Convenience methods to get paths
		public string GetSolutionRelativePath(string projectName)
		{
			var dir = new DirectoryInfo(AppContext.BaseDirectory);
			while (dir != null && !File.Exists(Path.Combine(dir.FullName, ProjectNames.SolutionFileName)))
				dir = dir.Parent;

			if (dir == null)
				throw new InvalidOperationException("Solution root not found. Check codecraft.config.json.");

			return Path.Combine(dir.FullName, projectName);
		}

		public string PluralizeName(string name)
		{
			if (name.EndsWith('y') && name.Length > 1 && !"aeiou".Contains(name[^2]))
				return name[..^1] + "ies";
			if (name.EndsWith('s') || name.EndsWith('x') || name.EndsWith('z') || name.EndsWith("ch") || name.EndsWith("sh"))
				return name + "es";
			return name + "s";
		}
	}
}