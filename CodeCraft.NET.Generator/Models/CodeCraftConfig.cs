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
				// 2. En el directorio actual de trabajo
				Path.Combine(Directory.GetCurrentDirectory(), "codecraft.config.json"),
				// 3. En el directorio base del dominio de aplicación
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "codecraft.config.json"),
				// 4. Solo el nombre del archivo (buscar en el PATH)
				"codecraft.config.json",
				// 5. En la raíz de la solución (go up from bin directories)
				Path.Combine(GetSolutionRoot(), "codecraft.config.json"),
				// 6. En el directorio Generator específicamente dentro de la solución
				Path.Combine(GetSolutionRoot(), "CodeCraft.NET.Generator", "codecraft.config.json"),
				// 7. Buscar en directorio padre (para proyectos generados por template)
				Path.Combine(GetSolutionRoot(), "*Generator", "codecraft.config.json")
			};

			string? configPath = null;
			
			// Buscar usando wildcards para proyectos con nombres dinámicos
			foreach (var location in locations)
			{
				if (location.Contains("*Generator"))
				{
					var solutionRoot = GetSolutionRoot();
					var generatorDirs = Directory.GetDirectories(solutionRoot, "*Generator", SearchOption.TopDirectoryOnly);
					foreach (var genDir in generatorDirs)
					{
						var configFile = Path.Combine(genDir, "codecraft.config.json");
						if (File.Exists(configFile))
						{
							configPath = configFile;
							break;
						}
					}
					if (configPath != null) break;
				}
				else if (File.Exists(location))
				{
					configPath = location;
					break;
				}
			}
			
			if (configPath == null)
			{
				Console.WriteLine("🔍 Searching for codecraft.config.json in these locations:");
				foreach (var location in locations.Where(l => !l.Contains("*Generator")))
				{
					Console.WriteLine($"   ❌ {location}");
				}
				
				// También mostrar directorios Generator encontrados
				var solutionRoot = GetSolutionRoot();
				var generatorDirs = Directory.GetDirectories(solutionRoot, "*Generator", SearchOption.TopDirectoryOnly);
				if (generatorDirs.Any())
				{
					Console.WriteLine("   Generator directories found:");
					foreach (var genDir in generatorDirs)
					{
						var configFile = Path.Combine(genDir, "codecraft.config.json");
						Console.WriteLine($"   ❌ {configFile}");
					}
				}
				
				throw new FileNotFoundException("Configuration file 'codecraft.config.json' not found.");
			}

			Console.WriteLine($"✅ Found configuration file: {configPath}");
			var json = File.ReadAllText(configPath);
			var data = JsonSerializer.Deserialize<CodeCraftConfig>(json, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			}) ?? throw new InvalidOperationException("Invalid config format.");

			return data;
		}

		private static string GetSolutionRoot()
		{
			var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
			
			// Navigate up from bin/Debug/net9.0 to solution root
			while (currentDir != null)
			{
				// Look for solution file or characteristic files
				if (currentDir.GetFiles("*.sln").Any() || 
					currentDir.GetDirectories().Any(d => d.Name.Contains("Generator")))
				{
					return currentDir.FullName;
				}
				currentDir = currentDir.Parent;
			}
			
			// Fallback to current directory
			return Directory.GetCurrentDirectory();
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