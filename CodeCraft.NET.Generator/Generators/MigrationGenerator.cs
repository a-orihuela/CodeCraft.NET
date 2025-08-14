using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;
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
			Console.WriteLine("🔍 Checking for model changes...");

			// Check if there's a recent migration to avoid duplicates
			if (ModelChangeDetector.HasRecentMigration(context, migrationPrefix, withinMinutes: 2))
			{
				Console.WriteLine("   ⏭️  Skipping migration - recent migration already exists");
				return;
			}

			// Check if this is the first migration (no migrations exist)
			if (ModelChangeDetector.ShouldCreateInitialMigration(context))
			{
				Console.WriteLine("   🆕 Creating initial migration...");
				GenerateMigration(context, outputDir, $"{migrationPrefix}Initial");
				return;
			}

			// Check if there are actual model changes
			if (!ModelChangeDetector.HasPendingModelChanges(context))
			{
				Console.WriteLine("   ⏭️  Skipping migration - no model changes detected");
				return;
			}

			// Generate migration since there are actual changes
			Console.WriteLine("   📝 Creating migration for detected changes...");
			GenerateMigration(context, outputDir, migrationPrefix);
		}

		private static void GenerateMigration(string context, string outputDir, string migrationPrefix)
		{
			string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
			string migrationName = migrationPrefix.EndsWith("Initial") 
				? migrationPrefix 
				: $"{migrationPrefix}{timestamp}";

			var config = CodeCraftConfig.Instance;

			// Build complete paths to .csproj files
			var infrastructureProjectPath = Path.Combine(
				config.GetSolutionRelativePath(config.ProjectNames.Infrastructure),
				$"{config.ProjectNames.Infrastructure}.csproj");

			var serverProjectPath = Path.Combine(
				config.GetSolutionRelativePath(config.ProjectNames.Server),
				$"{config.ProjectNames.Server}.csproj");

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
				WorkingDirectory = config.GetSolutionRelativePath("")
			};

			using var process = new Process { StartInfo = startInfo };
			process.Start();

			string output = process.StandardOutput.ReadToEnd();
			string error = process.StandardError.ReadToEnd();
			process.WaitForExit();

			if (process.ExitCode == 0)
			{
				// Check if the migration was actually created or if no changes were detected
				if (output.Contains("No changes were detected") || output.Contains("No model changes"))
				{
					Console.WriteLine("   ℹ️  No model changes detected by EF Core");
				}
				else
				{
					Console.WriteLine($"✅ Migration created successfully: {migrationName}");
				}
				
				if (!string.IsNullOrWhiteSpace(output) && !output.Contains("No changes"))
				{
					// Only show output if it's meaningful
					var relevantLines = output.Split('\n')
						.Where(line => !string.IsNullOrWhiteSpace(line) && 
									  !line.Contains("Build started") && 
									  !line.Contains("Build succeeded"))
						.Take(5); // Limit output
					
					foreach (var line in relevantLines)
					{
						Console.WriteLine($"   {line.Trim()}");
					}
				}
			}
			else
			{
				Console.WriteLine($"⚠️  Migration creation completed with warnings: {migrationName}");
				if (!string.IsNullOrWhiteSpace(error))
				{
					// Filter and show only relevant errors
					var errorLines = error.Split('\n')
						.Where(line => !string.IsNullOrWhiteSpace(line) && 
									  !line.Contains("warning") &&
									  !line.Contains("Build started"))
						.Take(3); // Limit error output
					
					foreach (var line in errorLines)
					{
						Console.WriteLine($"   ⚠️  {line.Trim()}");
					}
				}
			}
		}

		/// <summary>
		/// Forces migration generation without checks (for backward compatibility)
		/// </summary>
		public static void ForceGenerateMigration()
		{
			Console.WriteLine("🔧 Forcing migration creation...");
			GenerateMigration(
				context: "ApplicationDbContext",
				outputDir: "Migrations",
				migrationPrefix: "AutoGen_"
			);
		}
	}
}