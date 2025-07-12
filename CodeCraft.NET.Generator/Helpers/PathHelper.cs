namespace CodeCraft.NET.Generator.Helpers
{
	public static class PathHelper
	{
		// -------------------------------
		//  Solutions
		// -------------------------------
		private static string ApplicationBase => GetSolutionRelativePath(CodeCraftGenSettings.ApplicationProjectName);
		private static string InfrastructureBase => GetSolutionRelativePath(CodeCraftGenSettings.InfrastructureProjectName);
		private static string ServerBase => GetSolutionRelativePath(CodeCraftGenSettings.ServerProjectName);
		private static string CodeCraftGen => GetSolutionRelativePath(CodeCraftGenSettings.CodeCraftNETGeneratorName);
		private static string CodeCraftCross => GetSolutionRelativePath(CodeCraftGenSettings.CodeCraftNETCrossName);

		// -------------------------------
		//  Templates
		// -------------------------------
		private static string TemplatesBase => Path.Combine(CodeCraftGen, CodeCraftGenSettings.TemplatesFolder);
		private static string CQRSTemplates => Path.Combine(TemplatesBase, CodeCraftGenSettings.CQRSFolder);

		private static string FeaturesBase => Path.Combine(CQRSTemplates, CodeCraftGenSettings.FeaturesFolder);
		public static string PathCreateTemplates => Path.Combine(FeaturesBase, CodeCraftGenSettings.CommandsFolder, CodeCraftGenSettings.CreateFolder);
		public static string PathUpdateTemplates => Path.Combine(FeaturesBase, CodeCraftGenSettings.CommandsFolder, CodeCraftGenSettings.UpdateFolder);
		public static string PathDeleteTemplates => Path.Combine(FeaturesBase, CodeCraftGenSettings.CommandsFolder, CodeCraftGenSettings.DeleteFolder);
		public static string PathQueriesTemplates => Path.Combine(FeaturesBase, CodeCraftGenSettings.QueriesFolder);

		public static string PathSpecificationsTemplates => Path.Combine(CQRSTemplates, CodeCraftGenSettings.SpecificationsFolder);
		public static string PathMappingTemplates => Path.Combine(TemplatesBase, CodeCraftGenSettings.MappingFolder);
		public static string PathMappingProfileScriban => Path.Combine(PathMappingTemplates, CodeCraftGenSettings.MappingProfileFileName);
		public static string PathRepositoriesTemplates => Path.Combine(TemplatesBase, CodeCraftGenSettings.RepositoriesFolder);
		public static string PathControllersTemplates => Path.Combine(TemplatesBase, CodeCraftGenSettings.ControllersFolder);
		public static string PathHttpTemplates => Path.Combine(TemplatesBase, CodeCraftGenSettings.HttpRequestsFolder);

		// -------------------------------
		// Application
		// -------------------------------
		public static string PathAppPersistence => Path.Combine(ApplicationBase, CodeCraftGenSettings.ContractsFolder, CodeCraftGenSettings.PersistenceFolder);
		public static string PathAppRepositories => Path.Combine(PathAppPersistence, CodeCraftGenSettings.RepositoriesFolder);
		public static string GetPathAppIUnitOfWorkFile(string fileName) =>
			Path.Combine(PathAppPersistence, fileName);

		public static string PathAppFeatures => Path.Combine(ApplicationBase, CodeCraftGenSettings.CQRSFolder, CodeCraftGenSettings.FeaturesFolder);
		public static string GetPathAppFeatureCreate(string entityName, string fileName) =>
			Path.Combine(PathAppFeatures, PluralizeName(entityName), CodeCraftGenSettings.CommandsFolder, CodeCraftGenSettings.CreateFolder, fileName);
		public static string GetPathAppFeatureUpdate(string entityName, string fileName) =>
			Path.Combine(PathAppFeatures, PluralizeName(entityName), CodeCraftGenSettings.CommandsFolder, CodeCraftGenSettings.UpdateFolder, fileName);
		public static string GetPathAppFeatureDelete(string entityName, string fileName) =>
			Path.Combine(PathAppFeatures, PluralizeName(entityName), CodeCraftGenSettings.CommandsFolder, CodeCraftGenSettings.DeleteFolder, fileName);
		public static string GetPathAppFeatureQueries(string entityName, string fileName) =>
			Path.Combine(PathAppFeatures, PluralizeName(entityName), CodeCraftGenSettings.QueriesFolder, fileName);

		public static string PathAppSpecifications => Path.Combine(ApplicationBase, CodeCraftGenSettings.CQRSFolder, CodeCraftGenSettings.SpecificationsFolder);
		public static string GetPathAppSpecificationFile(string entityName, string fileName) =>
			Path.Combine(PathAppSpecifications, PluralizeName(entityName), fileName);

		public static string PathAppMapping => Path.Combine(ApplicationBase, CodeCraftGenSettings.MappingFolder);

		// -------------------------------
		// Infrastructure
		// -------------------------------
		public static string PathInfraPersistence => Path.Combine(InfrastructureBase, CodeCraftGenSettings.PersistenceFolder);
		public static string PathInfraRepositories => Path.Combine(PathInfraPersistence, CodeCraftGenSettings.RepositoriesFolder);
		public static string GetPathInfraUnitOfWorkFile(string fileName) =>
			Path.Combine(PathInfraPersistence, fileName);

		public static string GetPathInfraDbContextFile(string suffix)
		{
			return Path.Combine(PathInfraPersistence, $"ApplicationDbContext{suffix}");
		}

		public static string GetPathInfraDbContextFactoryFile(string suffix)
		{
			return Path.Combine(PathInfraPersistence, "Factories", $"ApplicationDbContextFactory{suffix}");
		}

		// -------------------------------
		// Server
		// -------------------------------
		public static string PathServerControllers => Path.Combine(ServerBase, CodeCraftGenSettings.ControllersFolder);

		public static string PathTestsAPIRequests => Path.Combine(ServerBase, CodeCraftGenSettings.TestsFolder, CodeCraftGenSettings.ApiRequestsFolder);

		public static string GetPathInfraRepositoryFile(string name, string suffix)
		{
			return Path.Combine(PathInfraRepositories, string.Format(suffix, name));
		}

		public static string GetPathAppIRepositoryFile(string name, string suffix)
		{
			return Path.Combine(PathAppRepositories, string.Format(suffix, name));
		}

		public static string GetPathAppMappingFile(string suffix)
		{
			return Path.Combine(PathAppMapping, suffix);
		}

		public static string GetPathAppFeatureCommandFile(string namePlural, string commandType, string name, string suffix)
		{
			return Path.Combine(PathAppFeatures, namePlural, CodeCraftGenSettings.CommandsFolder, commandType, $"{name}{suffix}");
		}

		public static string GetPathAppFeatureQueryFile(string namePlural, string name, string suffix)
		{
			return Path.Combine(PathAppFeatures, namePlural, CodeCraftGenSettings.QueriesFolder, $"Get{name}{suffix}");
		}

		public static string GetPathAppSpecificationFile(string namePlural, string name, string suffix)
		{
			return Path.Combine(PathAppSpecifications, namePlural, $"{name}{suffix}");
		}

		public static string GetPathAppIUnitOfWorkFile()
		{
			return Path.Combine(PathAppPersistence, CodeCraftGenSettings.UnitOfWorkInterfaceFileName);
		}

		public static string GetPathInfraUnitOfWorkFile()
		{
			return Path.Combine(PathInfraPersistence, CodeCraftGenSettings.UnitOfWorkImplementationFileName);
		}

		public static string GetPathServerControllerFile(string entityName, string fileName) =>
			Path.Combine(PathServerControllers, entityName, string.Format(fileName, entityName));
		public static string GetPathTestsAPIRequestsFile(string entityName, string fileName) =>
			Path.Combine(PathTestsAPIRequests, entityName, string.Format(fileName, entityName));

		private static string GetSolutionRelativePath(string projectName)
		{
			if (!string.IsNullOrWhiteSpace(CodeCraftGenSettings.SolutionRootOverride) &&
				Directory.Exists(CodeCraftGenSettings.SolutionRootOverride))
				return Path.Combine(CodeCraftGenSettings.SolutionRootOverride, projectName);

			var dir = new DirectoryInfo(AppContext.BaseDirectory);
			while (dir != null && !File.Exists(Path.Combine(dir.FullName, CodeCraftGenSettings.SolutionFileName)))
				dir = dir.Parent;

			if (dir == null)
				throw new InvalidOperationException("Solution root not found. Check codecraft.config.json.");

			return Path.Combine(dir.FullName, projectName);
		}

		public static string PluralizeName(string name)
		{
			if (name.EndsWith('y') && name.Length > 1 && !"aeiou".Contains(name[^2]))
				return name[..^1] + "ies";
			if (name.EndsWith('s') || name.EndsWith('x') || name.EndsWith('z') || name.EndsWith("ch") || name.EndsWith("sh"))
				return name + "es";
			return name + "s";
		}

		public static string InfrastructureRoot => InfrastructureBase;
		public static string ServerRoot => ServerBase;
	}
}
