using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
	public class CQRSGenerator
	{
		private readonly ITemplateRenderer _templateRenderer;

		public CQRSGenerator(ITemplateRenderer templateRenderer)
		{
			_templateRenderer = templateRenderer;
		}

		public void Generate(EntityMetadata entity)
		{
			GenerateCommands(entity);
			GenerateQueries(entity);
			GenerateSpecifications(entity);
		}

		public void GenerateMapping(IEnumerable<EntityMetadata> entities)
		{
			var templates = TemplateLocator.GetMappingTemplates();
			foreach (var template in templates) 
			{
				var outputPath = PathHelper.GetPathAppMappingFile(template.Suffix);
				_templateRenderer.Render(template.Path, outputPath, new { entities });
			}
		}

		private void GenerateCommands(EntityMetadata entity)
		{
			var templates = TemplateLocator.GetCommandTemplates();
			foreach (var template in templates)
			{
				var outputPath = PathHelper.GetPathAppFeatureCommandFile(entity.NamePlural, template.Type, entity.Name, template.Suffix);
				_templateRenderer.Render(template.Path, outputPath, entity);
			}
		}

		private void GenerateQueries(EntityMetadata entity)
		{
			var templates = TemplateLocator.GetQueryTemplates();
			foreach (var template in templates)
			{
				var outputPath = PathHelper.GetPathAppFeatureQueryFile(entity.NamePlural, entity.Name, template.Suffix);
				_templateRenderer.Render(template.Path, outputPath, entity);
			}
		}

		private void GenerateSpecifications(EntityMetadata entity)
		{
			var templates = TemplateLocator.GetSpecificationTemplates();
			foreach (var template in templates)
			{
				var outputPath = PathHelper.GetPathAppSpecificationFile(entity.NamePlural, entity.Name, template.Suffix);
				_templateRenderer.Render(template.Path, outputPath, entity);
			}
		}
	}
}
