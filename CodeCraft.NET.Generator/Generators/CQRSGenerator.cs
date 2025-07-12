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
			var templatePath = ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MappingProfile));
			var outputPath = ConfigHelper.GetMappingProfilePath();
			_templateRenderer.Render(templatePath, outputPath, new { entities });
		}

		private void GenerateCommands(EntityMetadata entity)
		{
			var entityPlural = ConfigHelper.PluralizeName(entity.Name);

			// Create Command
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandCreate)),
				ConfigHelper.GetCommandCreatePath(entityPlural, entity.Name),
				entity);

			// Create Handler
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandCreateHandler)),
				ConfigHelper.GetCommandCreateHandlerPath(entityPlural, entity.Name),
				entity);

			// Create Validator
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandCreateValidator)),
				ConfigHelper.GetCommandCreateValidatorPath(entityPlural, entity.Name),
				entity);

			// Update Command
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandUpdate)),
				ConfigHelper.GetCommandUpdatePath(entityPlural, entity.Name),
				entity);

			// Update Handler
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandUpdateHandler)),
				ConfigHelper.GetCommandUpdateHandlerPath(entityPlural, entity.Name),
				entity);

			// Update Validator
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandUpdateValidator)),
				ConfigHelper.GetCommandUpdateValidatorPath(entityPlural, entity.Name),
				entity);

			// Delete Command
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandDelete)),
				ConfigHelper.GetCommandDeletePath(entityPlural, entity.Name),
				entity);

			// Delete Handler
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.CommandDeleteHandler)),
				ConfigHelper.GetCommandDeleteHandlerPath(entityPlural, entity.Name),
				entity);
		}

		private void GenerateQueries(EntityMetadata entity)
		{
			var entityPlural = ConfigHelper.PluralizeName(entity.Name);

			// GetById Query
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.QueryGetById)),
				ConfigHelper.GetQueryGetByIdPath(entityPlural, entity.Name),
				entity);

			// GetById Handler
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.QueryGetByIdHandler)),
				ConfigHelper.GetQueryGetByIdHandlerPath(entityPlural, entity.Name),
				entity);

			// GetWithRelated Query
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.QueryGetWithRelated)),
				ConfigHelper.GetQueryGetWithRelatedPath(entityPlural, entity.Name),
				entity);

			// GetWithRelated Handler
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.QueryGetWithRelatedHandler)),
				ConfigHelper.GetQueryGetWithRelatedHandlerPath(entityPlural, entity.Name),
				entity);
		}

		private void GenerateSpecifications(EntityMetadata entity)
		{
			var entityPlural = ConfigHelper.PluralizeName(entity.Name);

			// Specification
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.Specification)),
				ConfigHelper.GetSpecificationPath(entityPlural, entity.Name),
				entity);

			// Specification Params
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.SpecificationParams)),
				ConfigHelper.GetSpecificationParamsPath(entityPlural, entity.Name),
				entity);

			// WithRelated
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.WithRelated)),
				ConfigHelper.GetWithRelatedPath(entityPlural, entity.Name),
				entity);

			// WithRelated Specification
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.WithRelatedSpecification)),
				ConfigHelper.GetWithRelatedSpecificationPath(entityPlural, entity.Name),
				entity);
		}
	}
}