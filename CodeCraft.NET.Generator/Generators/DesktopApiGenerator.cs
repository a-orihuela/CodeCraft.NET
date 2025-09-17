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
			// Desktop Service (using Services layer)
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("DesktopServiceWithServices"),
				ConfigHelper.GetDesktopServicePath(entity.Name),
				CreateTemplateContext(entity));
		}

		public void GenerateServiceRegistration(IEnumerable<EntityMetadata> entities)
		{
			var templatePath = ConfigHelper.GetTemplatePath("DesktopServiceRegistration");
			var outputPath = ConfigHelper.GetDesktopServiceRegistrationPath();

			var config = ConfigurationContext.Options;
			var context = new
			{
				entities,
				ApplicationProjectName = config.Shared.ProjectNames["Application"],
				DomainProjectName = config.Shared.ProjectNames["Domain"],
				DesktopProjectName = config.Shared.ProjectNames["Desktop"],
				ServicesProjectName = config.Shared.ProjectNames["Services"]
			};

			_templateRenderer.Render(templatePath, outputPath, context);
		}

		private object CreateTemplateContext(EntityMetadata entity)
		{
			var config = ConfigurationContext.Options;
			return new
			{
				entity.Name,
				entity.NamePlural,
				entity.Properties,
				entity.Usings,
				name = entity.Name.ToLowerInvariant(),
				ApplicationProjectName = config.Shared.ProjectNames["Application"],
				DomainProjectName = config.Shared.ProjectNames["Domain"],
				DesktopProjectName = config.Shared.ProjectNames["Desktop"],
				ServicesProjectName = config.Shared.ProjectNames["Services"]
			};
		}
	}
}