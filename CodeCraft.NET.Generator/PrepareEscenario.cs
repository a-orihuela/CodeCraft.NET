using CodeCraft.NET.Generator.Helpers;

namespace CodeCraft.NET.Generator
{
	internal static class PrepareEscenario
	{
		public static void DeleteOldFiles()
		{
			try
			{
				if (Directory.Exists(PathHelper.PathAppFeatures))
					Directory.Delete(PathHelper.PathAppFeatures, recursive: true);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete {PathHelper.PathTestsAPIRequests}: {ex.Message}");
			}
			try
			{
				if (Directory.Exists(PathHelper.PathAppSpecifications))
					Directory.Delete(PathHelper.PathAppSpecifications, recursive: true);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete {PathHelper.PathTestsAPIRequests}: {ex.Message}");
			}
			try
			{
				if (Directory.Exists(PathHelper.PathAppRepositories))
					Directory.Delete(PathHelper.PathAppRepositories, recursive: true);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete {PathHelper.PathTestsAPIRequests}: {ex.Message}");
			}
			try
			{
				if (File.Exists(PathHelper.GetPathAppIUnitOfWorkFile(CodeCraftGenSettings.UnitOfWorkInterfaceName)))
					File.Delete(PathHelper.GetPathAppIUnitOfWorkFile(CodeCraftGenSettings.UnitOfWorkInterfaceName));
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete {PathHelper.PathTestsAPIRequests}: {ex.Message}");
			}
			try
			{
				if (File.Exists(PathHelper.GetPathAppMappingFile("MappingProfile.cs")))
					File.Delete(PathHelper.GetPathAppMappingFile("MappingProfile.cs"));
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete {PathHelper.PathTestsAPIRequests}: {ex.Message}");
			}
			try
			{
				if (Directory.Exists(PathHelper.PathInfraRepositories))
					Directory.Delete(PathHelper.PathInfraRepositories, recursive: true);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete {PathHelper.PathTestsAPIRequests}: {ex.Message}");
			}
			try
			{
				if (File.Exists(PathHelper.GetPathInfraUnitOfWorkFile(CodeCraftGenSettings.UnitOfWorkImplementationName)))
					File.Delete(PathHelper.GetPathInfraUnitOfWorkFile(CodeCraftGenSettings.UnitOfWorkImplementationName));
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete {PathHelper.PathTestsAPIRequests}: {ex.Message}");
			}
			try
			{
				if (Directory.Exists(PathHelper.PathServerControllers))
					Directory.Delete(PathHelper.PathServerControllers, recursive: true);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete {PathHelper.PathTestsAPIRequests}: {ex.Message}");
			}
			try
			{
				if (Directory.Exists(PathHelper.PathTestsAPIRequests))
					Directory.Delete(PathHelper.PathTestsAPIRequests, recursive: true);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[WARN] Cannot delete {PathHelper.PathTestsAPIRequests}: {ex.Message}");
			}
			

		}
	}
}
