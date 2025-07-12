using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
	public class DbContextGenerator
	{
		private readonly ITemplateRenderer _templateRenderer;

		public DbContextGenerator(ITemplateRenderer templateRenderer)
		{
			_templateRenderer = templateRenderer;
		}

		public void Generate(IEnumerable<EntityMetadata> entities)
		{
			// DbContext
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.DbContext)),
				ConfigHelper.GetDbContextPath(),
				new { entities });

			// DbContext Factory
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.DbContextFactory)),
				ConfigHelper.GetDbContextFactoryPath(),
				new { entities });
		}
	}
}