using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
	public class RepositoryGenerator
	{
		private const string REPOSITORY_INTERFACE = "RepositoryInterface";
		private const string REPOSITORY_IMPLEMENTATION = "RepositoryImplementation";
		private const string UNIT_OF_WORK_INTERFACE = "UnitOfWorkInterface";
		private const string UNIT_OF_WORK_IMPLEMENTATION = "UnitOfWorkImplementation";

		private readonly ITemplateRenderer _templateRenderer;

		public RepositoryGenerator(ITemplateRenderer templateRenderer)
		{
			_templateRenderer = templateRenderer;
		}

		public void Generate(IEnumerable<EntityMetadata> entities)
		{
			var templates = TemplateLocator.GetRepositoryTemplates();

			foreach (var entity in entities)
			{
				Generate(entity, templates);
			}
			templates = TemplateLocator.GetUnitOfWorkTemplates();
			GenerateUnitOfWork(templates, entities);
		}

		private void Generate(EntityMetadata entity, List<TemplateDefinition> templates)
		{
			foreach (var template in templates)
			{
				string outputPath = template.Type switch
				{
					REPOSITORY_INTERFACE => PathHelper.GetPathAppIRepositoryFile(entity.Name, template.Suffix),
					REPOSITORY_IMPLEMENTATION => PathHelper.GetPathInfraRepositoryFile(entity.Name, template.Suffix)
				};
				_templateRenderer.Render(template.Path, outputPath, entity);
			}
		}

		private void GenerateUnitOfWork(List<TemplateDefinition> templates, IEnumerable<EntityMetadata> entities)
		{
			foreach (var template in templates)
			{
				string outputPath = template.Type switch
				{
					UNIT_OF_WORK_INTERFACE => PathHelper.GetPathAppIUnitOfWorkFile(template.Suffix),
					UNIT_OF_WORK_IMPLEMENTATION => PathHelper.GetPathInfraUnitOfWorkFile(template.Suffix)
				};
				_templateRenderer.Render(template.Path, outputPath, new { entities });
			}
		}
	}
}
