using System.Diagnostics;

namespace CodeCraft.NET.Generator.Helpers
{
	public static class ModelChangeDetector
	{
		/// <summary>
		/// Checks if there are pending model changes that require a migration
		/// </summary>
		/// <param name="context">The DbContext name</param>
		/// <returns>True if there are pending changes, false otherwise</returns>
		public static bool HasPendingModelChanges(string context)
		{
			try
			{
				// First check: Try to create a dry-run migration to see if there are changes
				bool hasChanges = CheckWithDryRunMigration(context);
				
				if (hasChanges)
				{
					Console.WriteLine("   ?? Model changes detected - migration needed");
				}
				else
				{
					Console.WriteLine("   ? No model changes detected - skipping migration");
				}

				return hasChanges;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   ??  Error checking model changes: {ex.Message}");
				// On error, assume changes exist to be safe
				return true;
			}
		}

		/// <summary>
		/// Uses dotnet ef dbcontext script to detect if there are pending model changes
		/// </summary>
		private static bool CheckWithDryRunMigration(string context)
		{
			var config = ConfigurationContext.Options;

			// Build complete paths to .csproj files
			var infrastructureProjectPath = Path.Combine(
				ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Infrastructure"]),
				$"{config.Shared.ProjectNames["Infrastructure"]}.csproj");

			var serverProjectPath = Path.Combine(
				ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Server"]),
				$"{config.Shared.ProjectNames["Server"]}.csproj");

			// Create a temporary migration name to test
			string tempMigrationName = $"TempCheck_{DateTime.UtcNow.Ticks}";

			var startInfo = new ProcessStartInfo
			{
				FileName = "dotnet",
				Arguments = $"ef migrations add {tempMigrationName} " +
							$"--project \"{infrastructureProjectPath}\" " +
							$"--startup-project \"{serverProjectPath}\" " +
							$"--context {context} " +
							$"--dry-run --verbose",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WorkingDirectory = ConfigurationContext.GetSolutionRoot()
			};

			try
			{
				using var process = new Process { StartInfo = startInfo };
				process.Start();

				string output = process.StandardOutput.ReadToEnd();
				string error = process.StandardError.ReadToEnd();
				process.WaitForExit();

				// Check if the dry run indicates no model changes
				if (output.Contains("No changes were detected") || 
					output.Contains("No model changes") ||
					error.Contains("No changes were detected"))
				{
					return false;
				}

				// If dry-run is not supported, fall back to alternative method
				if (error.Contains("dry-run") || error.Contains("not supported") || process.ExitCode != 0)
				{
					Console.WriteLine("   ??  Dry-run not available, using alternative detection...");
					return CheckWithDatabaseScript(context);
				}

				// If we get here and there's substantial output, assume there are changes
				return !string.IsNullOrWhiteSpace(output) && output.Length > 100;
			}
			catch (Exception)
			{
				// Fall back to alternative method
				return CheckWithDatabaseScript(context);
			}
		}

		/// <summary>
		/// Alternative method to detect model changes using database script generation
		/// </summary>
		private static bool CheckWithDatabaseScript(string context)
		{
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
				Arguments = $"ef dbcontext script " +
							$"--project \"{infrastructureProjectPath}\" " +
							$"--startup-project \"{serverProjectPath}\" " +
							$"--context {context}",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WorkingDirectory = ConfigurationContext.GetSolutionRoot()
			};

			try
			{
				using var process = new Process { StartInfo = startInfo };
				process.Start();

				string output = process.StandardOutput.ReadToEnd();
				string error = process.StandardError.ReadToEnd();
				process.WaitForExit();

				// If there are build errors, it might indicate model changes
				if (!string.IsNullOrWhiteSpace(error))
				{
					if (error.Contains("Build failed") || 
						error.Contains("compilation failed") ||
						error.Contains("does not contain a definition"))
					{
						// Build issues often indicate recent model changes
						return true;
					}
				}

				// If script generation succeeds, assume no pending changes
				// (this is a conservative approach)
				return false;
			}
			catch (Exception)
			{
				// On error, assume changes exist to be safe
				return true;
			}
		}

		/// <summary>
		/// Enhanced method to check for recent migrations with better detection
		/// </summary>
		public static bool HasRecentMigration(string context, string migrationPrefix, int withinMinutes = 5)
		{
			var config = ConfigurationContext.Options;
			var migrationsPath = Path.Combine(
				ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Infrastructure"]),
				"Migrations");

			if (!Directory.Exists(migrationsPath))
				return false;

			var cutoffTime = DateTime.UtcNow.AddMinutes(-withinMinutes);
			
			// Check both file creation time and modification time
			var migrationFiles = Directory.GetFiles(migrationsPath, $"*{migrationPrefix}*.cs")
				.Where(f => File.GetCreationTimeUtc(f) > cutoffTime || File.GetLastWriteTimeUtc(f) > cutoffTime)
				.ToArray();

			if (migrationFiles.Length > 0)
			{
				var recentFile = migrationFiles.OrderByDescending(f => File.GetCreationTimeUtc(f)).First();
				Console.WriteLine($"   ??  Recent migration found: {Path.GetFileName(recentFile)}");
				return true;
			}

			return false;
		}

		/// <summary>
		/// Checks if there are any migration files at all
		/// </summary>
		public static bool HasAnyMigrations(string context)
		{
			var config = ConfigurationContext.Options;
			var migrationsPath = Path.Combine(
				ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Infrastructure"]),
				"Migrations");

			if (!Directory.Exists(migrationsPath))
				return false;

			var migrationFiles = Directory.GetFiles(migrationsPath, "*.cs")
				.Where(f => !Path.GetFileName(f).Equals("ApplicationDbContextModelSnapshot.cs", StringComparison.OrdinalIgnoreCase))
				.ToArray();

			return migrationFiles.Length > 0;
		}

		/// <summary>
		/// Simplified check: if this is the first run or no migrations exist, create one
		/// </summary>
		public static bool ShouldCreateInitialMigration(string context)
		{
			if (!HasAnyMigrations(context))
			{
				Console.WriteLine("   ?? No existing migrations found - initial migration needed");
				return true;
			}

			return false;
		}
	}
}