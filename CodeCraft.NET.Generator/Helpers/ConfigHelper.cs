using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Helpers
{
	public static class ConfigHelper
	{
		private static CodeCraftConfig Config => CodeCraftConfig.Instance;

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
			=> GetFilePath(Config.Files.Controller, entityName);

		public static string GetDesktopServicePath(string entityName)
			=> GetFilePath(Config.Files.DesktopService, entityName);

		public static string GetDesktopServiceRegistrationPath()
			=> Config.Files.DesktopServiceRegistration;

		public static string GetHttpRequestPath(string entityName)
			=> GetFilePath(Config.Files.HttpRequest, entityName);

		// Commands
		public static string GetCommandCreatePath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.CommandCreate, entityPlural, entityName);

		public static string GetCommandCreateHandlerPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.CommandCreateHandler, entityPlural, entityName);

		public static string GetCommandCreateValidatorPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.CommandCreateValidator, entityPlural, entityName);

		public static string GetCommandUpdatePath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.CommandUpdate, entityPlural, entityName);

		public static string GetCommandUpdateHandlerPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.CommandUpdateHandler, entityPlural, entityName);

		public static string GetCommandUpdateValidatorPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.CommandUpdateValidator, entityPlural, entityName);

		public static string GetCommandDeletePath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.CommandDelete, entityPlural, entityName);

		public static string GetCommandDeleteHandlerPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.CommandDeleteHandler, entityPlural, entityName);

		// Queries
		public static string GetQueryGetByIdPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.QueryGetById, entityPlural, entityName);

		public static string GetQueryGetByIdHandlerPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.QueryGetByIdHandler, entityPlural, entityName);

		public static string GetQueryGetWithRelatedPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.QueryGetWithRelated, entityPlural, entityName);

		public static string GetQueryGetWithRelatedHandlerPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.QueryGetWithRelatedHandler, entityPlural, entityName);

		// Specifications
		public static string GetSpecificationPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.Specification, entityPlural, entityName);

		public static string GetSpecificationParamsPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.SpecificationParams, entityPlural, entityName);

		public static string GetWithRelatedPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.WithRelated, entityPlural, entityName);

		public static string GetWithRelatedSpecificationPath(string entityPlural, string entityName)
			=> GetFilePath(Config.Files.WithRelatedSpecification, entityPlural, entityName);

		// Repository
		public static string GetRepositoryInterfacePath(string entityName)
			=> GetFilePath(Config.Files.RepositoryInterface, entityName);

		public static string GetRepositoryImplementationPath(string entityName)
			=> GetFilePath(Config.Files.RepositoryImplementation, entityName);

		// Unit of Work
		public static string GetUnitOfWorkInterfacePath()
			=> Config.Files.UnitOfWorkInterface;

		public static string GetUnitOfWorkImplementationPath()
			=> Config.Files.UnitOfWorkImplementation;

		// Mapping
		public static string GetMappingProfilePath()
			=> Config.Files.MappingProfile;

		// DbContext
		public static string GetDbContextPath()
			=> Config.Files.DbContext;

		public static string GetDbContextFactoryPath()
			=> Config.Files.DbContextFactory;

		// Get template paths
		public static string GetTemplatePath(string templateProperty)
		{
			var templatePath = templateProperty switch
			{
				nameof(Config.Templates.Controller) => Config.Templates.Controller,
				nameof(Config.Templates.DesktopService) => Config.Templates.DesktopService,
				nameof(Config.Templates.DesktopServiceRegistration) => Config.Templates.DesktopServiceRegistration,
				nameof(Config.Templates.CommandCreate) => Config.Templates.CommandCreate,
				nameof(Config.Templates.CommandCreateHandler) => Config.Templates.CommandCreateHandler,
				nameof(Config.Templates.CommandCreateValidator) => Config.Templates.CommandCreateValidator,
				nameof(Config.Templates.CommandUpdate) => Config.Templates.CommandUpdate,
				nameof(Config.Templates.CommandUpdateHandler) => Config.Templates.CommandUpdateHandler,
				nameof(Config.Templates.CommandUpdateValidator) => Config.Templates.CommandUpdateValidator,
				nameof(Config.Templates.CommandDelete) => Config.Templates.CommandDelete,
				nameof(Config.Templates.CommandDeleteHandler) => Config.Templates.CommandDeleteHandler,
				nameof(Config.Templates.QueryGetById) => Config.Templates.QueryGetById,
				nameof(Config.Templates.QueryGetByIdHandler) => Config.Templates.QueryGetByIdHandler,
				nameof(Config.Templates.QueryGetWithRelated) => Config.Templates.QueryGetWithRelated,
				nameof(Config.Templates.QueryGetWithRelatedHandler) => Config.Templates.QueryGetWithRelatedHandler,
				nameof(Config.Templates.Specification) => Config.Templates.Specification,
				nameof(Config.Templates.SpecificationParams) => Config.Templates.SpecificationParams,
				nameof(Config.Templates.WithRelated) => Config.Templates.WithRelated,
				nameof(Config.Templates.WithRelatedSpecification) => Config.Templates.WithRelatedSpecification,
				nameof(Config.Templates.HttpRequest) => Config.Templates.HttpRequest,
				nameof(Config.Templates.MappingProfile) => Config.Templates.MappingProfile,
				nameof(Config.Templates.DbContext) => Config.Templates.DbContext,
				nameof(Config.Templates.DbContextFactory) => Config.Templates.DbContextFactory,
				nameof(Config.Templates.RepositoryInterface) => Config.Templates.RepositoryInterface,
				nameof(Config.Templates.RepositoryImplementation) => Config.Templates.RepositoryImplementation,
				nameof(Config.Templates.UnitOfWorkInterface) => Config.Templates.UnitOfWorkInterface,
				nameof(Config.Templates.UnitOfWorkImplementation) => Config.Templates.UnitOfWorkImplementation,
				_ => throw new ArgumentException($"Unknown template property: {templateProperty}")
			};

			return templatePath;
		}

		public static string PluralizeName(string name) => Config.PluralizeName(name);

		public static string GetInfrastructureRoot() => Config.GetSolutionRelativePath(Config.ProjectNames.Infrastructure);
		public static string GetServerRoot() => Config.GetSolutionRelativePath(Config.ProjectNames.Server);
		public static string GetDesktopRoot() => Config.GetSolutionRelativePath(Config.ProjectNames.Desktop);
	}
}