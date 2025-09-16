using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
	public class MauiGenerator
	{
		private readonly ITemplateRenderer _templateRenderer;

		public MauiGenerator(ITemplateRenderer templateRenderer)
		{
			_templateRenderer = templateRenderer;
		}

		public void Generate(EntityMetadata entity)
		{
			// ServiceHelper
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiServiceHelper"),
				ConfigHelper.GetMauiServiceHelperPath(entity.Name),
				CreateTemplateContext(entity));

			// Mapper
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiMapperHelper"),
				ConfigHelper.GetMauiMapperHelperPath(entity.Name),
				CreateTemplateContext(entity));

			// Validation
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiValidationHelper"),
				ConfigHelper.GetMauiValidationHelperPath(entity.Name),
				CreateTemplateContext(entity));
		}

		public void GenerateMAUIServiceRegistration(IEnumerable<EntityMetadata> entities)
		{
			var templatePath = ConfigHelper.GetTemplatePath("MappingProfile");
			var outputPath = ConfigHelper.GetMappingProfilePath();

			var config = ConfigurationContext.Options;
			var context = new
			{
				entities,
				ApplicationProjectName = config.Shared.ProjectNames["Application"],
				DomainProjectName = config.Shared.ProjectNames["Domain"],
				InfrastructureProjectName = config.Shared.ProjectNames["Infrastructure"],
				UnitOfWorkInterfaceName = config.Shared.Files["UnitOfWorkInterfaceName"]
			};

			_templateRenderer.Render(templatePath, outputPath, context);
		}

		private object CreateTemplateContext(EntityMetadata entity)
		{
			var config = ConfigurationContext.Options;
			return new
			{
				entity.Name,
				name = entity.Name.ToLowerInvariant(),
				entity.NamePlural,
				entity.Properties,
				entity.Usings,
				MauiProjectName = "CodeCraft.NET.MAUI",
				DesktopProjectName = config.Shared.ProjectNames["Desktop"],
				ApplicationProjectName = config.Shared.ProjectNames["Application"],
				DomainProjectName = config.Shared.ProjectNames["Domain"]
			};
		}

	}
}