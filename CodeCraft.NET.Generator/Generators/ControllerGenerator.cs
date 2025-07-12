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
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.Controller)),
				ConfigHelper.GetControllerPath(entity.Name),
				entity);

			// HTTP Request
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.HttpRequest)),
				ConfigHelper.GetHttpRequestPath(entity.Name),
				entity);
		}
	}
}