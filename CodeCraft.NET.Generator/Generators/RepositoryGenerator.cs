using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
	public class RepositoryGenerator
	{
		private readonly ITemplateRenderer _templateRenderer;

		public RepositoryGenerator(ITemplateRenderer templateRenderer)
		{
			_templateRenderer = templateRenderer;
		}

		public void Generate(IEnumerable<EntityMetadata> entities)
		{
			foreach (var entity in entities)
			{
				Generate(entity);
			}
			GenerateUnitOfWork(entities);
		}

		private void Generate(EntityMetadata entity)
		{
			// Repository Interface
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.RepositoryInterface)),
				ConfigHelper.GetRepositoryInterfacePath(entity.Name),
				entity);

			// Repository Implementation
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.RepositoryImplementation)),
				ConfigHelper.GetRepositoryImplementationPath(entity.Name),
				entity);
		}

		private void GenerateUnitOfWork(IEnumerable<EntityMetadata> entities)
		{
			// Unit of Work Interface
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.UnitOfWorkInterface)),
				ConfigHelper.GetUnitOfWorkInterfacePath(),
				new { entities });

			// Unit of Work Implementation
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.UnitOfWorkImplementation)),
				ConfigHelper.GetUnitOfWorkImplementationPath(),
				new { entities });
		}
	}
}