using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
	public class DesktopApiGenerator
	{
		private readonly ITemplateRenderer _templateRenderer;

		public DesktopApiGenerator(ITemplateRenderer templateRenderer)
		{
			_templateRenderer = templateRenderer;
		}

		public void Generate(EntityMetadata entity)
		{
			// Desktop Service
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.DesktopService)),
				ConfigHelper.GetDesktopServicePath(entity.Name),
				CreateTemplateContext(entity));
		}

		public void GenerateServiceRegistration(IEnumerable<EntityMetadata> entities)
		{
			var templatePath = ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.DesktopServiceRegistration));
			var outputPath = ConfigHelper.GetDesktopServiceRegistrationPath();

			var config = CodeCraftConfig.Instance;
			var context = new
			{
				entities,
				ApplicationProjectName = config.ProjectNames.Application,
				DomainProjectName = config.ProjectNames.Domain,
				InfrastructureProjectName = config.ProjectNames.Infrastructure,
				DesktopProjectName = config.ProjectNames.Desktop
			};

			_templateRenderer.Render(templatePath, outputPath, context);
		}

		private object CreateTemplateContext(EntityMetadata entity, object? additionalData = null)
		{
			var config = CodeCraftConfig.Instance;
			var baseContext = new
			{
				// Entity data
				entity.Name,
				entity.NamePlural,
				entity.Properties,
				entity.Usings,

				// Project names
				ApplicationProjectName = config.ProjectNames.Application,
				DomainProjectName = config.ProjectNames.Domain,
				InfrastructureProjectName = config.ProjectNames.Infrastructure,
				DesktopProjectName = config.ProjectNames.Desktop,

				// Interface names
				UnitOfWorkInterfaceName = config.Files.UnitOfWorkInterfaceName
			};

			// If we have additional data, merge it with the base context
			if (additionalData != null)
			{
				var additionalProps = additionalData.GetType().GetProperties();
				var mergedData = new Dictionary<string, object>();

				// Add base context properties
				foreach (var prop in baseContext.GetType().GetProperties())
				{
					mergedData[prop.Name] = prop.GetValue(baseContext)!;
				}

				// Add additional properties
				foreach (var prop in additionalProps)
				{
					mergedData[prop.Name] = prop.GetValue(additionalData)!;
				}

				return mergedData;
			}

			return baseContext;
		}
	}
}