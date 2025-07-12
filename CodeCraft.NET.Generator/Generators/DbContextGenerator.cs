using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
	public class DbContextGenerator
	{
		private const string APP_DBCONTEXT = "ApplicationDbContext";
		private const string DBCONTEXT_FACTORY = "DbContextFactory";

		private readonly ITemplateRenderer _templateRenderer;

		public DbContextGenerator(ITemplateRenderer templateRenderer)
		{
			_templateRenderer = templateRenderer;
		}

		public void Generate(IEnumerable<EntityMetadata> entities)
		{
			var templates = TemplateLocator.GetDbContextTemplates();

			foreach (var template in templates)
			{
				string outputPath = template.Type switch
				{
					APP_DBCONTEXT => PathHelper.GetPathInfraDbContextFile(template.Suffix),
					DBCONTEXT_FACTORY => PathHelper.GetPathInfraDbContextFactoryFile(template.Suffix),
					_ => throw new ArgumentException($"Unknown template type: {template.Type}")
				};

				_templateRenderer.Render(template.Path, outputPath, new { entities });
			}
		}
	}
}