using CodeCraft.NET.Generator.Helpers;
using System.Diagnostics;

namespace CodeCraft.NET.Generator.Generators
{
	public static class MigrationGenerator
	{
		public static void GenerateAllMigrations()
		{
			GenerateMigration(
				context: "ApplicationDbContext",
				outputDir: "Migrations",
				migrationPrefix: "AutoGen_"
			);
		}

		private static void GenerateMigration(string context, string outputDir, string migrationPrefix)
		{
			string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
			string migrationName = $"{migrationPrefix}{timestamp}";

			var startInfo = new ProcessStartInfo
			{
				FileName = "dotnet",
				Arguments = $"ef migrations add {migrationName} " +
							$"--project {CodeCraftGenSettings.InfrastructureProjectName} " +
							$"--startup-project {CodeCraftGenSettings.ServerProjectName} " +
							$"--context {context} " +
							$"--output-dir {outputDir}",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WorkingDirectory = Path.GetFullPath(Path.Combine(PathHelper.PathInfraPersistence, "..", ".."))
			};

			using var process = new Process { StartInfo = startInfo };
			process.Start();

			string output = process.StandardOutput.ReadToEnd();
			string error = process.StandardError.ReadToEnd();
			process.WaitForExit();

			Console.WriteLine($"Migration created for context: {context}");
			Console.WriteLine(output);
			if (!string.IsNullOrWhiteSpace(error))
				Console.Error.WriteLine($"Error generating migration for {context}:\n{error}");
		}
	}
}
