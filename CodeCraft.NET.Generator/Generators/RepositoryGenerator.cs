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

		private object CreateTemplateContext(EntityMetadata entity, object? additionalData = null)
		{
			var config = ConfigurationContext.Options;
			var baseContext = new
			{
				// Entity data
				entity.Name,
				entity.NamePlural,
				entity.Properties,
				entity.Usings,

				// Project names
				ApplicationProjectName = config.Shared.ProjectNames["Application"],
				DomainProjectName = config.Shared.ProjectNames["Domain"],
				InfrastructureProjectName = config.Shared.ProjectNames["Infrastructure"],

				// Interface names
				UnitOfWorkInterfaceName = config.Shared.Files["UnitOfWorkInterfaceName"]
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

		private void Generate(EntityMetadata entity)
		{
			// Repository Interface
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("RepositoryInterface"),
				ConfigHelper.GetRepositoryInterfacePath(entity.Name),
				CreateTemplateContext(entity));

			// Repository Implementation
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("RepositoryImplementation"),
				ConfigHelper.GetRepositoryImplementationPath(entity.Name),
				CreateTemplateContext(entity));
		}

		private void GenerateUnitOfWork(IEnumerable<EntityMetadata> entities)
		{
			var config = ConfigurationContext.Options;
			var context = new
			{
				entities,
				ApplicationProjectName = config.Shared.ProjectNames["Application"],
				DomainProjectName = config.Shared.ProjectNames["Domain"],
				InfrastructureProjectName = config.Shared.ProjectNames["Infrastructure"],
				UnitOfWorkInterfaceName = config.Shared.Files["UnitOfWorkInterfaceName"],
				UnitOfWorkImplementationName = config.Shared.Files["UnitOfWorkImplementationName"],
				CrossProjectName = config.Shared.ProjectNames["Cross"]
			};

			// Unit of Work Interface
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("UnitOfWorkInterface"),
				ConfigHelper.GetUnitOfWorkInterfacePath(),
				context);

			// Unit of Work Implementation
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("UnitOfWorkImplementation"),
				ConfigHelper.GetUnitOfWorkImplementationPath(),
				context);
		}
	}
}