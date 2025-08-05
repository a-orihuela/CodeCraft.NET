using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator
{
	internal static class PrepareEscenario
	{
		public static void DeleteOldFiles()
		{
			// Create instance to get directory paths
			var config = CodeCraftConfig.Instance;

			// Helper to get directory paths
			string GetDirectoryFromFilePath(string filePath)
			{
				var samplePath = string.Format(filePath, "Sample", "Sample");
				return Path.GetDirectoryName(samplePath) ?? "";
			}

			try
			{
				// Delete CQRS Features directory
				var featuresDir = GetDirectoryFromFilePath(config.Files.CommandCreate);
				var appCqrsDir = Path.GetDirectoryName(featuresDir);
				if (!string.IsNullOrEmpty(appCqrsDir) && Directory.Exists(appCqrsDir))
					Directory.Delete(appCqrsDir, recursive: true);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete CQRS Features: {ex.Message}");
			}

			try
			{
				// Delete Repositories directory
				var repoDir = Path.GetDirectoryName(ConfigHelper.GetRepositoryInterfacePath("Sample"));
				if (!string.IsNullOrEmpty(repoDir) && Directory.Exists(repoDir))
					Directory.Delete(repoDir, recursive: true);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete Repository interfaces: {ex.Message}");
			}

			try
			{
				// Delete Repository implementations directory
				var repoImplDir = Path.GetDirectoryName(ConfigHelper.GetRepositoryImplementationPath("Sample"));
				if (!string.IsNullOrEmpty(repoImplDir) && Directory.Exists(repoImplDir))
					Directory.Delete(repoImplDir, recursive: true);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete Repository implementations: {ex.Message}");
			}

			try
			{
				// Delete Unit of Work Interface
				if (File.Exists(ConfigHelper.GetUnitOfWorkInterfacePath()))
					File.Delete(ConfigHelper.GetUnitOfWorkInterfacePath());
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete Unit of Work interface: {ex.Message}");
			}

			try
			{
				// Delete Unit of Work Implementation
				if (File.Exists(ConfigHelper.GetUnitOfWorkImplementationPath()))
					File.Delete(ConfigHelper.GetUnitOfWorkImplementationPath());
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete Unit of Work implementation: {ex.Message}");
			}

			try
			{
				// Delete Mapping Profile
				if (File.Exists(ConfigHelper.GetMappingProfilePath()))
					File.Delete(ConfigHelper.GetMappingProfilePath());
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete Mapping Profile: {ex.Message}");
			}

			try
			{
				// Delete DbContext files
				if (File.Exists(ConfigHelper.GetDbContextPath()))
					File.Delete(ConfigHelper.GetDbContextPath());

				if (File.Exists(ConfigHelper.GetDbContextFactoryPath()))
					File.Delete(ConfigHelper.GetDbContextFactoryPath());
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete DbContext files: {ex.Message}");
			}

			try
			{
				// Delete Controllers directory
				var controllerDir = Path.GetDirectoryName(ConfigHelper.GetControllerPath("Sample"));
				if (!string.IsNullOrEmpty(controllerDir) && Directory.Exists(controllerDir))
					Directory.Delete(controllerDir, recursive: true);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete Controllers: {ex.Message}");
			}
		}
	}
}