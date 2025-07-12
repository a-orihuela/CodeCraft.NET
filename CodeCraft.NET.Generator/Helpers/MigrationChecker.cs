using System.Diagnostics;

namespace CodeCraft.NET.Generator.Helpers
{
	public static class MigrationChecker
	{
		public static void CheckPendingMigrations(string projectPath, string startupProjectPath, string context)
		{
			var psi = new ProcessStartInfo
			{
				FileName = "dotnet",
				Arguments = $"ef migrations list --project \"{projectPath}\" --startup-project \"{startupProjectPath}\" --context {context}",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
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