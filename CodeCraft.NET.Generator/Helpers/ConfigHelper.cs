using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Helpers
{
	public static class ConfigHelper
	{
		private static CodeCraftOptions Config => ConfigurationContext.Options;
		private static string GetProjectName(string key) => key switch
		{
			"Domain" => Config.Shared.ProjectNames["Domain"],
			"Application" => Config.Shared.ProjectNames["Application"],
			"Infrastructure" => Config.Shared.ProjectNames["Infrastructure"],
			"Server" => Config.Shared.ProjectNames["Server"],
			"Desktop" => Config.Shared.ProjectNames["Desktop"],
			"Cross" => Config.Shared.ProjectNames["Cross"],
			_ => throw new ArgumentException($"Unknown project key: {key}")
		};

		// Methods to get formatted file paths
		public static string GetFilePath(string filePattern, params object[] args)
		{
			var fullPath = string.Format(filePattern, args);
			var directoryPath = Path.GetDirectoryName(fullPath);

			if (!string.IsNullOrEmpty(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			return fullPath;
		}

		// Specific methods for each file type
		public static string GetControllerPath(string entityName)
			=> GetFilePath(Config.Shared.Files["Controller"], entityName);

		public static string GetDesktopServicePath(string entityName)
			=> GetFilePath(Config.Shared.Files["DesktopService"], entityName);

		public static string GetDesktopServiceRegistrationPath()
			=> Config.Shared.Files["DesktopServiceRegistration"];

		public static string GetInfrastructureServiceRegistrationPath()
			=> Config.Shared.Files["InfrastructureServiceRegistration"];

		public static string GetHttpRequestPath(string entityName)
			=> GetFilePath(Config.Shared.Files["HttpRequest"], entityName);

		// Commands
		public static string GetCommandCreatePath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["CommandCreate"], entityPlural, entityName);

		public static string GetCommandCreateHandlerPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["CommandCreateHandler"], entityPlural, entityName);

		public static string GetCommandCreateValidatorPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["CommandCreateValidator"], entityPlural, entityName);

		public static string GetCommandUpdatePath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["CommandUpdate"], entityPlural, entityName);

		public static string GetCommandUpdateHandlerPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["CommandUpdateHandler"], entityPlural, entityName);

		public static string GetCommandUpdateValidatorPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["CommandUpdateValidator"], entityPlural, entityName);

		public static string GetCommandDeletePath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["CommandDelete"], entityPlural, entityName);

		public static string GetCommandDeleteHandlerPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["CommandDeleteHandler"], entityPlural, entityName);

		// Queries
		public static string GetQueryGetByIdPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["QueryGetById"], entityPlural, entityName);

		public static string GetQueryGetByIdHandlerPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["QueryGetByIdHandler"], entityPlural, entityName);

		public static string GetQueryGetWithRelatedPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["QueryGetWithRelated"], entityPlural, entityName);

		public static string GetQueryGetWithRelatedHandlerPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["QueryGetWithRelatedHandler"], entityPlural, entityName);

		// Specifications
		public static string GetSpecificationPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["Specification"], entityPlural, entityName);

		public static string GetSpecificationParamsPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["SpecificationParams"], entityPlural, entityName);

		public static string GetWithRelatedPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["WithRelated"], entityPlural, entityName);

		public static string GetWithRelatedSpecificationPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Shared.Files["WithRelatedSpecification"], entityPlural, entityName);

		// Repository
		public static string GetRepositoryInterfacePath(string entityName)
			=> GetFilePath(Config.Shared.Files["RepositoryInterface"], entityName);

		public static string GetRepositoryImplementationPath(string entityName)
			=> GetFilePath(Config.Shared.Files["RepositoryImplementation"], entityName);

		// Unit of Work
		public static string GetUnitOfWorkInterfacePath()
			=> Config.Shared.Files["UnitOfWorkInterface"];

		public static string GetUnitOfWorkImplementationPath()
			=> Config.Shared.Files["UnitOfWorkImplementation"];

		// Mapping
		public static string GetMappingProfilePath()
			=> Config.Shared.Files["MappingProfile"];

		public static string GetEntityDtosPath(string entityName)
			=> GetFilePath(Config.Shared.Files["EntityDtos"], entityName);

		public static string GetEntityWithRelatedDtoPath(string entityName)
			=> GetFilePath(Config.Shared.Files["EntityWithRelatedDto"], entityName);

		// DbContext
		public static string GetDbContextPath()
			=> Config.Shared.Files["DbContext"];

		public static string GetDbContextFactoryPath()
			=> Config.Shared.Files["DbContextFactory"];

		public static string GetMauiServiceRegistrationPath()
			=> Config.Shared.Files["MauiServiceRegistration"];

		public static string GetMauiMapperHelperPath(string entityName)
			=> GetFilePath(Config.Shared.Files["MauiMapperHelper"], entityName);

		public static string GetMauiValidationHelperPath(string entityName)
			=> GetFilePath(Config.Shared.Files["MauiValidationHelper"], entityName);

		public static string GetMauiServiceHelperPath(string entityName)
			=> GetFilePath(Config.Shared.Files["MauiServiceHelper"], entityName);

		// Get template paths
		public static string GetTemplatePath(string templateProperty)
		{
			var templatePath = templateProperty switch
			{
				"Controller" => Config.Shared.Templates["Controller"],
				"DesktopService" => Config.Shared.Templates["DesktopService"],
				"DesktopServiceRegistration" => Config.Shared.Templates["DesktopServiceRegistration"],
				"InfrastructureServiceRegistration" => Config.Shared.Templates["InfrastructureServiceRegistration"],
				
				"MauiServiceHelper" => Config.Shared.Templates["MauiServiceHelper"],
				"MauiMapper" => Config.Shared.Templates["MauiMapper"],
				"MauiValidationHelper" => Config.Shared.Templates["MauiValidationHelper"],
				"MauiServiceRegistration" => Config.Shared.Templates["MauiServiceRegistration"],
				
				"EntityDtos" => Config.Shared.Templates["EntityDtos"],
				"EntityWithRelatedDto" => Config.Shared.Templates["EntityWithRelatedDto"],
				
				"CommandCreate" => Config.Shared.Templates["CommandCreate"],
				"CommandCreateHandler" => Config.Shared.Templates["CommandCreateHandler"],
				"CommandCreateValidator" => Config.Shared.Templates["CommandCreateValidator"],
				"CommandUpdate" => Config.Shared.Templates["CommandUpdate"],
				"CommandUpdateHandler" => Config.Shared.Templates["CommandUpdateHandler"],
				"CommandUpdateValidator" => Config.Shared.Templates["CommandUpdateValidator"],
				"CommandDelete" => Config.Shared.Templates["CommandDelete"],
				"CommandDeleteHandler" => Config.Shared.Templates["CommandDeleteHandler"],
				"QueryGetById" => Config.Shared.Templates["QueryGetById"],
				"QueryGetByIdHandler" => Config.Shared.Templates["QueryGetByIdHandler"],
				"QueryGetWithRelated" => Config.Shared.Templates["QueryGetWithRelated"],
				"QueryGetWithRelatedHandler" => Config.Shared.Templates["QueryGetWithRelatedHandler"],
				"Specification" => Config.Shared.Templates["Specification"],
				"SpecificationParams" => Config.Shared.Templates["SpecificationParams"],
				"WithRelated" => Config.Shared.Templates["WithRelated"],
				"WithRelatedSpecification" => Config.Shared.Templates["WithRelatedSpecification"],
				"HttpRequest" => Config.Shared.Templates["HttpRequest"],
				"MappingProfile" => Config.Shared.Templates["MappingProfile"],
				"DbContext" => Config.Shared.Templates["DbContext"],
				"DbContextFactory" => Config.Shared.Templates["DbContextFactory"],
				"RepositoryInterface" => Config.Shared.Templates["RepositoryInterface"],
				"RepositoryImplementation" => Config.Shared.Templates["RepositoryImplementation"],
				"UnitOfWorkInterface" => Config.Shared.Templates["UnitOfWorkInterface"],
				"UnitOfWorkImplementation" => Config.Shared.Templates["UnitOfWorkImplementation"],
				_ => throw new ArgumentException($"Unknown template property: {templateProperty}")
			};

			return templatePath;
		}

		public static string PluralizeName(string name) => ConfigurationContext.PluralizeName(name);

		public static string GetInfrastructureRoot() => ConfigurationContext.GetSolutionRelativePath(GetProjectName("Infrastructure"));
		public static string GetServerRoot() => ConfigurationContext.GetSolutionRelativePath(GetProjectName("Server"));
		public static string GetDesktopRoot() => ConfigurationContext.GetSolutionRelativePath(GetProjectName("Desktop"));
	}
}