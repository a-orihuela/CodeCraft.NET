using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
	public class ControllerGenerator
	{
		private readonly ITemplateRenderer _templateRenderer;

		public ControllerGenerator(ITemplateRenderer templateRenderer)
		{
			_templateRenderer = templateRenderer;
		}

		public void Generate(EntityMetadata entity)
		{
			// Controller
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("Controller"),
				ConfigHelper.GetControllerPath(entity.Name),
				CreateTemplateContext(entity));

			// HTTP Request files
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("HttpRequest"),
				ConfigHelper.GetHttpRequestPath(entity.Name),
				CreateTemplateContext(entity));
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
				ApplicationProjectName = config.Shared.ProjectNames["Application"],
				DomainProjectName = config.Shared.ProjectNames["Domain"],
				ServerProjectName = config.Shared.ProjectNames["Server"]
			};
		}
	}
}