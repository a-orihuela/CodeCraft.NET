using System.Diagnostics;

namespace CodeCraft.NET.Generator.Helpers
{
	public static class MigrationChecker
	{
		public static void CheckPendingMigrations(string context)
		{
			var config = ConfigurationContext.Options;

			// Build complete paths to .csproj files
			var infrastructureProjectPath = Path.Combine(
				ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Infrastructure"]),
				$"{config.Shared.ProjectNames["Infrastructure"]}.csproj");

			var serverProjectPath = Path.Combine(
				ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Server"]),
				$"{config.Shared.ProjectNames["Server"]}.csproj");

			var psi = new ProcessStartInfo
			{
				FileName = "dotnet",
				Arguments = $"ef migrations list --project \"{infrastructureProjectPath}\" --startup-project \"{serverProjectPath}\" --context {context}",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WorkingDirectory = ConfigurationContext.GetSolutionRoot()
			};

			using var process = Process.Start(psi);
			if (process != null)
			{
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
}