using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
    /// <summary>
    /// Generator for Application test classes
    /// Focuses on Command Handlers, Query Handlers, and Validators
    /// </summary>
    public class ApplicationTestGenerator
    {
        private readonly ITemplateRenderer _templateRenderer;

        public ApplicationTestGenerator(ITemplateRenderer templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }

        /// <summary>
        /// Generates all Application tests for a given entity
        /// </summary>
        public void Generate(EntityMetadata entity)
        {
            // Command Handler Tests - Most important for Application
            GenerateCommandHandlerTests(entity);
            
            // Query Handler Tests
            GenerateQueryHandlerTests(entity);
            
            // Validator Tests
            GenerateValidatorTests(entity);
        }

        /// <summary>
        /// Generates Command Handler test classes
        /// Tests business logic, validation, and persistence operations
        /// </summary>
        public void GenerateCommandHandlerTests(EntityMetadata entity)
        {
            // Create Handler Tests
            _templateRenderer.Render(
                ConfigHelper.GetTemplatePath("CommandCreateHandlerTest"),
                ConfigHelper.GetCommandCreateHandlerTestPath(entity.NamePlural, entity.Name),
                CreateCommandHandlerTestContext(entity));

            // Update Handler Tests
            _templateRenderer.Render(
                ConfigHelper.GetTemplatePath("CommandUpdateHandlerTest"),
                ConfigHelper.GetCommandUpdateHandlerTestPath(entity.NamePlural, entity.Name),
                CreateCommandHandlerTestContext(entity));

            // Delete Handler Tests
            _templateRenderer.Render(
                ConfigHelper.GetTemplatePath("CommandDeleteHandlerTest"),
                ConfigHelper.GetCommandDeleteHandlerTestPath(entity.NamePlural, entity.Name),
                CreateCommandHandlerTestContext(entity));
        }

        /// <summary>
        /// Generates Query Handler test classes
        /// </summary>
        public void GenerateQueryHandlerTests(EntityMetadata entity)
        {
            // GetById Handler Tests
            _templateRenderer.Render(
                ConfigHelper.GetTemplatePath("QueryGetByIdHandlerTest"),
                ConfigHelper.GetQueryGetByIdHandlerTestPath(entity.NamePlural, entity.Name),
                CreateQueryHandlerTestContext(entity));

            // GetWithRelated Handler Tests
            _templateRenderer.Render(
                ConfigHelper.GetTemplatePath("QueryGetWithRelatedHandlerTest"),
                ConfigHelper.GetQueryGetWithRelatedHandlerTestPath(entity.NamePlural, entity.Name),
                CreateQueryHandlerTestContext(entity));
        }

        /// <summary>
        /// Generates Validator test classes
        /// </summary>
        public void GenerateValidatorTests(EntityMetadata entity)
        {
            _templateRenderer.Render(
                ConfigHelper.GetTemplatePath("ValidatorTest"),
                ConfigHelper.GetValidatorTestPath(entity.NamePlural, entity.Name),
                CreateValidatorTestContext(entity));
        }

        /// <summary>
        /// Generates Mapping Profile tests (shared across all entities)
        /// </summary>
        public void GenerateMappingProfileTests()
        {
            var entities = EntityAnalyzer.AnalyzeDomainEntities();
            
            _templateRenderer.Render(
                ConfigHelper.GetTemplatePath("MappingProfileTest"),
                ConfigHelper.GetMappingProfileTestPath(),
                CreateMappingProfileTestContext(entities));
        }

        private object CreateCommandHandlerTestContext(EntityMetadata entity)
        {
            var config = ConfigurationContext.Options;
            return new
            {
                entity.Name,
                entity.NamePlural,
                entity.Properties,
                entity.Usings,
                name = entity.Name.ToLowerInvariant(),
                ApplicationProjectName = config.Shared.ProjectNames["Application"],
                DomainProjectName = config.Shared.ProjectNames["Domain"],
                TestApplicationProjectName = "CodeCraft.NET.Test.Application"
            };
        }

        private object CreateQueryHandlerTestContext(EntityMetadata entity)
        {
            var config = ConfigurationContext.Options;
            return new
            {
                entity.Name,
                entity.NamePlural,
                entity.Properties,
                entity.Usings,
                ApplicationProjectName = config.Shared.ProjectNames["Application"],
                DomainProjectName = config.Shared.ProjectNames["Domain"],
                TestApplicationProjectName = "CodeCraft.NET.Test.Application"
            };
        }

        private object CreateValidatorTestContext(EntityMetadata entity)
        {
            var config = ConfigurationContext.Options;
            return new
            {
                entity.Name,
                entity.NamePlural,
                entity.Properties,
                entity.Usings,
                ApplicationProjectName = config.Shared.ProjectNames["Application"],
                DomainProjectName = config.Shared.ProjectNames["Domain"],
                TestApplicationProjectName = "CodeCraft.NET.Test.Application"
            };
        }

        private object CreateMappingProfileTestContext(IEnumerable<EntityMetadata> entities)
        {
            var config = ConfigurationContext.Options;
            return new
            {
                entities,
                ApplicationProjectName = config.Shared.ProjectNames["Application"],
                DomainProjectName = config.Shared.ProjectNames["Domain"],
                TestApplicationProjectName = "CodeCraft.NET.Test.Application"
            };
        }
    }
}