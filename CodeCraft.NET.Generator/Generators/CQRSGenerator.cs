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
			GenerateEntityDtos(entity);
		}

		public void GenerateMapping(IEnumerable<EntityMetadata> entities)
		{
			var templatePath = ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MappingProfile));
			var outputPath = ConfigHelper.GetMappingProfilePath();

			var config = CodeCraftConfig.Instance;
			var context = new
			{
				entities,
				ApplicationProjectName = config.ProjectNames.Application,
				DomainProjectName = config.ProjectNames.Domain,
				InfrastructureProjectName = config.ProjectNames.Infrastructure,
				UnitOfWorkInterfaceName = config.Files.UnitOfWorkInterfaceName
			};

			_templateRenderer.Render(templatePath, outputPath, context);
		}

		private object CreateTemplateContext(EntityMetadata entity, object? additionalData = null)
		{
			var config = CodeCraftConfig.Instance;
			var baseContext = new
			{
				// Entity data
				entity.Name,
				entity.NamePlural,
				entity.Properties,
				entity.Usings,

				// Project names
				ApplicationProjectName = config.ProjectNames.Application,
				DomainProjectName = config.ProjectNames.Domain,
				InfrastructureProjectName = config.ProjectNames.Infrastructure,

				// Interface names
				UnitOfWorkInterfaceName = config.Files.UnitOfWorkInterfaceName
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

		private void GenerateCommands(EntityMetadata entity)
		{
			var entityPlural = ConfigHelper.PluralizeName(entity.Name);

			// Create Command
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandCreate)),
				ConfigHelper.GetCommandCreatePath(entityPlural, entity.Name),
				CreateTemplateContext(entity));

			// Create Handler
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandCreateHandler)),
				ConfigHelper.GetCommandCreateHandlerPath(entityPlural, entity.Name),
				CreateTemplateContext(entity));

			// Create Validator
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandCreateValidator)),
				ConfigHelper.GetCommandCreateValidatorPath(entityPlural, entity.Name),
				CreateTemplateContext(entity));

			// Update Command
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandUpdate)),
				ConfigHelper.GetCommandUpdatePath(entityPlural, entity.Name),
				CreateTemplateContext(entity));

			// Update Handler
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandUpdateHandler)),
				ConfigHelper.GetCommandUpdateHandlerPath(entityPlural, entity.Name),
				CreateTemplateContext(entity));

			// Update Validator
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandUpdateValidator)),
				ConfigHelper.GetCommandUpdateValidatorPath(entityPlural, entity.Name),
				CreateTemplateContext(entity));

			// Delete Command
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandDelete)),
				ConfigHelper.GetCommandDeletePath(entityPlural, entity.Name),
				CreateTemplateContext(entity));

			// Delete Handler
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandDeleteHandler)),
				ConfigHelper.GetCommandDeleteHandlerPath(entityPlural, entity.Name),
				CreateTemplateContext(entity));
		}

		private void GenerateQueries(EntityMetadata entity)
		{
			var entityPlural = ConfigHelper.PluralizeName(entity.Name);

			// GetById Query
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.QueryGetById)),
				ConfigHelper.GetQueryGetByIdPath(entityPlural, entity.Name),
				CreateTemplateContext(entity));

			// GetById Handler
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.QueryGetByIdHandler)),
				ConfigHelper.GetQueryGetByIdHandlerPath(entityPlural, entity.Name),
				CreateTemplateContext(entity));

			// GetWithRelated Query
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.QueryGetWithRelated)),
				ConfigHelper.GetQueryGetWithRelatedPath(entityPlural, entity.Name),
				CreateTemplateContext(entity));

			// GetWithRelated Handler
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.QueryGetWithRelatedHandler)),
				ConfigHelper.GetQueryGetWithRelatedHandlerPath(entityPlural, entity.Name),
				CreateTemplateContext(entity));
		}

		private void GenerateSpecifications(EntityMetadata entity)
		{
			var context = CreateTemplateContext(entity);

			// Generate Specification
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.Specification)),
				ConfigHelper.GetSpecificationPath(entity.NamePlural, entity.Name),
				context);

			// Generate SpecificationParams
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.SpecificationParams)),
				ConfigHelper.GetSpecificationParamsPath(entity.NamePlural, entity.Name),
				context);

			// Generate WithRelated
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.WithRelated)),
				ConfigHelper.GetWithRelatedPath(entity.NamePlural, entity.Name),
				context);

			// Generate WithRelatedSpecification
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.WithRelatedSpecification)),
				ConfigHelper.GetWithRelatedSpecificationPath(entity.NamePlural, entity.Name),
				context);
		}

		private void GenerateEntityDtos(EntityMetadata entity)
		{
			var context = CreateTemplateContext(entity);

			// Generate main DTOs (always regenerated)
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.EntityDtos)),
				ConfigHelper.GetEntityDtosPath(entity.Name),
				context);

			// Generate WithRelatedDto only if it doesn't exist (customizable)
			GenerateWithRelatedDtoIfNotExists(entity, context);
		}

		private void GenerateWithRelatedDtoIfNotExists(EntityMetadata entity, object context)
		{
			var outputPath = ConfigHelper.GetEntityWithRelatedDtoPath(entity.Name);
			var config = CodeCraftConfig.Instance;
			var fullPath = Path.Combine(config.GetSolutionRoot(), outputPath);
			
			if (!File.Exists(fullPath))
			{
				_templateRenderer.Render(
					ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.EntityWithRelatedDto)),
					outputPath,
					context);
				Console.WriteLine($"   ✅ Generated: {entity.Name}WithRelatedDto.cs");
			}
			else
			{
				Console.WriteLine($"   📁 Preserved: DTOs/Custom/{entity.NamePlural}/{entity.Name}WithRelatedDto.cs - Custom file exists");
			}
		}
	}
}