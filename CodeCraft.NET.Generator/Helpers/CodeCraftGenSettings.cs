using CodeCraft.NET.Generator.Models;
using System.Text.Json;

namespace CodeCraft.NET.Generator.Helpers
{
	public static class CodeCraftGenSettings
	{
		private static readonly CodeCraftConfig _config = LoadConfiguration();

		// Solution settings
		public static string SolutionFileName => _config.SolutionFileName;
		public static string SolutionRootOverride => _config.SolutionRootOverride;
		public static string DomainProjectName => _config.DomainProjectName;
		public static string ApplicationProjectName => _config.ApplicationProjectName;
		public static string InfrastructureProjectName => _config.InfrastructureProjectName;
		public static string ServerProjectName => _config.ServerProjectName;
		public static string CodeCraftNETGeneratorName => _config.CodeCraftNETGeneratorName;
		public static string CodeCraftNETCrossName => _config.CodeCraftNETCrossName;

		// Templates structure
		public static string TemplatesFolder => _config.Folders.TemplatesFolder;
		public static string CQRSFolder => _config.Folders.CQRSFolder;
		public static string FeaturesFolder => _config.Folders.FeaturesFolder;
		public static string CommandsFolder => _config.Folders.CommandsFolder;
		public static string CreateFolder => _config.Folders.CreateFolder;
		public static string UpdateFolder => _config.Folders.UpdateFolder;
		public static string DeleteFolder => _config.Folders.DeleteFolder;
		public static string QueriesFolder => _config.Folders.QueriesFolder;
		public static string SpecificationsFolder => _config.Folders.SpecificationsFolder;
		public static string MappingFolder => _config.Folders.MappingFolder;
		public static string RepositoriesFolder => _config.Folders.RepositoriesFolder;
		public static string ControllersFolder => _config.Folders.ControllersFolder;
		public static string HttpRequestsFolder => _config.Folders.HttpRequestsFolder;

		// Application structure
		public static string ContractsFolder => _config.Folders.ContractsFolder;
		public static string PersistenceFolder => _config.Folders.PersistenceFolder;

		// Tests
		public static string TestsFolder => _config.Folders.TestsFolder;
		public static string ApiRequestsFolder => _config.Folders.ApiRequestsFolder;

		// File naming
		public static string MappingProfileFileName => _config.Folders.MappingProfileFileName;
		public static string UnitOfWorkInterfaceFileName => _config.Folders.UnitOfWorkInterfaceFileName;
		public static string UnitOfWorkInterfaceName => _config.Folders.UnitOfWorkInterfaceName;
		public static string UnitOfWorkImplementationFileName => _config.Folders.UnitOfWorkImplementationFileName;
		public static string UnitOfWorkImplementationName => _config.Folders.UnitOfWorkImplementationName;

		// Template files
		// Command templates
		public static string CreateTemplate => _config.Templates.CreateTemplate;
		public static string CreateHandlerTemplate => _config.Templates.CreateHandlerTemplate;
		public static string CreateValidatorTemplate => _config.Templates.CreateValidatorTemplate;
		public static string UpdateTemplate => _config.Templates.UpdateTemplate;
		public static string UpdateHandlerTemplate => _config.Templates.UpdateHandlerTemplate;
		public static string UpdateValidatorTemplate => _config.Templates.UpdateValidatorTemplate;
		public static string DeleteTemplate => _config.Templates.DeleteTemplate;
		public static string DeleteHandlerTemplate => _config.Templates.DeleteHandlerTemplate;

		// Query templates
		public static string GetByIdQueryTemplate => _config.Templates.GetByIdQueryTemplate;
		public static string GetByIdHandlerTemplate => _config.Templates.GetByIdHandlerTemplate;
		public static string GetWithRelatedQueryTemplate => _config.Templates.GetWithRelatedQueryTemplate;
		public static string GetWithRelatedHandlerTemplate => _config.Templates.GetWithRelatedHandlerTemplate;

		// Specification templates
		public static string SpecificationTemplate => _config.Templates.SpecificationTemplate;
		public static string SpecificationParamsTemplate => _config.Templates.SpecificationParamsTemplate;
		public static string WithRelatedTemplate => _config.Templates.WithRelatedTemplate;
		public static string WithRelatedSpecificationTemplate => _config.Templates.WithRelatedSpecificationTemplate;

		// Repository templates
		public static string RepositoryInterfaceTemplate => _config.Templates.RepositoryInterfaceTemplate;
		public static string RepositoryImplementationTemplate => _config.Templates.RepositoryImplementationTemplate;
		public static string UnitOfWorkInterfaceTemplate => _config.Templates.UnitOfWorkInterfaceTemplate;
		public static string UnitOfWorkImplementationTemplate => _config.Templates.UnitOfWorkImplementationTemplate;

		// Controller templates
		public static string ControllerTemplate => _config.Templates.ControllerTemplate;
		public static string HttpRequestTemplate => _config.Templates.HttpRequestTemplate;

		// Output file suffixes
		public static string CreateSuffix => _config.Templates.CreateSuffix;
		public static string CreateHandlerSuffix => _config.Templates.CreateHandlerSuffix;
		public static string CreateValidatorSuffix => _config.Templates.CreateValidatorSuffix;
		public static string UpdateSuffix => _config.Templates.UpdateSuffix;
		public static string UpdateHandlerSuffix => _config.Templates.UpdateHandlerSuffix;
		public static string UpdateValidatorSuffix => _config.Templates.UpdateValidatorSuffix;
		public static string DeleteSuffix => _config.Templates.DeleteSuffix;
		public static string DeleteHandlerSuffix => _config.Templates.DeleteHandlerSuffix;
		public static string GetByIdQuerySuffix => _config.Templates.GetByIdQuerySuffix;
		public static string GetByIdHandlerSuffix => _config.Templates.GetByIdHandlerSuffix;
		public static string GetWithRelatedQuerySuffix => _config.Templates.GetWithRelatedQuerySuffix;
		public static string GetWithRelatedHandlerSuffix => _config.Templates.GetWithRelatedHandlerSuffix;
		public static string SpecificationSuffix => _config.Templates.SpecificationSuffix;
		public static string SpecificationParamsSuffix => _config.Templates.SpecificationParamsSuffix;
		public static string WithRelatedSuffix => _config.Templates.WithRelatedSuffix;
		public static string WithRelatedSpecificationSuffix => _config.Templates.WithRelatedSpecificationSuffix;
		public static string RepositoryInterfaceSuffix => _config.Templates.RepositoryInterfaceSuffix;
		public static string RepositoryImplementationSuffix => _config.Templates.RepositoryImplementationSuffix;
		public static string ControllerSuffix => _config.Templates.ControllerSuffix;
		public static string HttpRequestSuffix => _config.Templates.HttpRequestSuffix;

		// Database contexts
		public static IReadOnlyList<string> DbContexts => _config.DbContexts;

		private static CodeCraftConfig LoadConfiguration()
		{
			// Try multiple potential locations for the config file
			var locations = new[]
			{
				Path.Combine(AppContext.BaseDirectory, "codecraft.config.json"),
				"codecraft.config.json", // Current directory
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "codecraft.config.json"),
				Path.Combine(Directory.GetCurrentDirectory(), "codecraft.config.json")
			};

			string? configPath = locations.FirstOrDefault(File.Exists);

			if (configPath == null)
				throw new FileNotFoundException("Configuration file 'codecraft.config.json' not found. Searched in: " +
											   string.Join(", ", locations));

			var json = File.ReadAllText(configPath);
			return JsonSerializer.Deserialize<CodeCraftConfig>(json)
				   ?? throw new InvalidOperationException("Invalid config format.");
		}
	}
}