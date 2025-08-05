using CodeCraft.NET.Generator.Models;
using System.Diagnostics;

namespace CodeCraft.NET.Generator.Helpers
{
	public static class MigrationChecker
	{
		public static void CheckPendingMigrations(string context)
		{
			var config = CodeCraftConfig.Instance;

			// Build complete paths to .csproj files
			var infrastructureProjectPath = Path.Combine(
				config.GetSolutionRelativePath(config.ProjectNames.Infrastructure),
				$"{config.ProjectNames.Infrastructure}.csproj");

			var serverProjectPath = Path.Combine(
				config.GetSolutionRelativePath(config.ProjectNames.Server),
				$"{config.ProjectNames.Server}.csproj");

			var psi = new ProcessStartInfo
			{
				FileName = "dotnet",
				Arguments = $"ef migrations list --project \"{infrastructureProjectPath}\" --startup-project \"{serverProjectPath}\" --context {context}",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WorkingDirectory = config.GetSolutionRelativePath("") // Solution root directory
			};

			using var process = Process.Start(psi);
			var output = process.StandardOutput.ReadToEnd();
			var error = process.StandardError.ReadToEnd();
			process.WaitForExit();

			if (!string.IsNullOrWhiteSpace(error))
			{
				Console.WriteLine($"EF Core warning: {error.Trim()}");
				return;
			}

			if (!output.Contains("Pending: 0"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Model changes detected that are not yet in a migration.");
				Console.WriteLine($"    Please run:\n    dotnet ef migrations add [Name] --context {context}");
				Console.ResetColor();
			}
		}
	}
}