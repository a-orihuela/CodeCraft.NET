using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
	public class ControllerGenerator
	{
		private const string CONTROLLER = "Controller";
		private const string HTTP_REQUEST = "HttpRequest";
		private readonly ITemplateRenderer _templateRenderer;

		public ControllerGenerator(ITemplateRenderer templateRenderer)
		{
			_templateRenderer = templateRenderer;
		}

		public void Generate(EntityMetadata entity)
		{
			var templates = TemplateLocator.GetControllerTemplates();

			foreach (var template in templates)
			{
				string outputPath = template.Type switch
				{
					CONTROLLER => PathHelper.GetPathServerControllerFile(entity.NamePlural, template.Suffix),
					HTTP_REQUEST => PathHelper.GetPathTestsAPIRequestsFile(entity.NamePlural, template.Suffix),
					_ => throw new NotSupportedException($"Unsupported template type: {template.Type}")
				};

				_templateRenderer.Render(template.Path, outputPath, entity);
			}
		}
	}
}
