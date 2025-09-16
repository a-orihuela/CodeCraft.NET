using CodeCraft.NET.Generator.Helpers;
using System.Diagnostics;

namespace CodeCraft.NET.Generator.Generators
{
	public static class MigrationGenerator
	{
		public static void GenerateAllMigrations()
		{
			GenerateSmartMigration(
				context: "ApplicationDbContext",
				outputDir: "Migrations",
				migrationPrefix: "AutoGen_"
			);
		}

		/// <summary>
		/// Generates a migration only if there are actual model changes
		/// </summary>
		private static void GenerateSmartMigration(string context, string outputDir, string migrationPrefix)
		{
			Console.WriteLine("Checking for model changes...");

			// Check if there's a recent migration to avoid duplicates
			if (ModelChangeDetector.HasRecentMigration(context, migrationPrefix, withinMinutes: 2))
			{
				Console.WriteLine("   Skipping migration - recent migration already exists");
				return;
			}

			// Check if this is the first migration (no migrations exist)
			if (ModelChangeDetector.ShouldCreateInitialMigration(context))
			{
				Console.WriteLine("   Creating initial migration...");
				GenerateMigration(context, outputDir, $"{migrationPrefix}Initial");
				return;
			}

			// Check if there are actual model changes
			if (!ModelChangeDetector.HasPendingModelChanges(context))
			{
				Console.WriteLine("   Skipping migration - no model changes detected");
				return;
			}

			// Generate migration since there are actual changes
			Console.WriteLine("   Creating migration for detected changes...");
			GenerateMigration(context, outputDir, migrationPrefix);
		}

		private static void GenerateMigration(string context, string outputDir, string migrationPrefix)
		{
			string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
			string migrationName = migrationPrefix.EndsWith("Initial") 
				? migrationPrefix 
				: $"{migrationPrefix}{timestamp}";

			var config = ConfigurationContext.Options;

			// Build complete paths to .csproj files
			var infrastructureProjectPath = Path.Combine(
				ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Infrastructure"]),
				$"{config.Shared.ProjectNames["Infrastructure"]}.csproj");

			var serverProjectPath = Path.Combine(
				ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Server"]),
				$"{config.Shared.ProjectNames["Server"]}.csproj");

			var startInfo = new ProcessStartInfo
			{
				FileName = "dotnet",
				Arguments = $"ef migrations add {migrationName} " +
							$"--project \"{infrastructureProjectPath}\" " +
							$"--startup-project \"{serverProjectPath}\" " +
							$"--context {context} " +
							$"--output-dir {outputDir}",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WorkingDirectory = ConfigurationContext.GetSolutionRoot()
			};

			Console.WriteLine($"Command executed: {startInfo.FileName} {startInfo.Arguments}");

			try
			{
				using var process = Process.Start(startInfo);
				if (process != null)
				{
					var output = process.StandardOutput.ReadToEnd();
					var error = process.StandardError.ReadToEnd();

					process.WaitForExit();

					Console.WriteLine("Standard output:");
					Console.WriteLine(output);

					if (!string.IsNullOrEmpty(error))
					{
						Console.WriteLine("Standard error:");
						Console.WriteLine(error);
					}

					if (process.ExitCode == 0)
					{
						Console.WriteLine($"Migration '{migrationName}' created successfully!");
					}
					else
					{
						Console.WriteLine($"Migration creation failed with exit code: {process.ExitCode}");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error running migration command: {ex.Message}");
			}
		}
	}
}