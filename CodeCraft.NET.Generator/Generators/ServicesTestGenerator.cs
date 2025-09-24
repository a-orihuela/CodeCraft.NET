using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
    /// <summary>
    /// Generator for Services test classes
    /// Focuses on testing BaseEntityService implementations and business logic
    /// </summary>
    public class ServicesTestGenerator
    {
        private readonly ITemplateRenderer _templateRenderer;

        public ServicesTestGenerator(ITemplateRenderer templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }

        /// <summary>
        /// Generates all Services tests for a given entity
        /// </summary>
        public void Generate(EntityMetadata entity)
        {
            // Entity Service Tests - Test business logic, MediatR integration, error handling
            GenerateEntityServiceTests(entity);
        }

        /// <summary>
        /// Generates Service test classes for specific entities
        /// Tests the service layer integration with MediatR and error handling
        /// </summary>
        public void GenerateEntityServiceTests(EntityMetadata entity)
        {
            _templateRenderer.Render(
                ConfigHelper.GetTemplatePath("ServiceTest"),
                ConfigHelper.GetServiceTestPath(entity.Name),
                CreateServiceTestContext(entity));
        }

        private object CreateServiceTestContext(EntityMetadata entity)
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
                ServicesProjectName = config.Shared.ProjectNames["Services"],
                TestServicesProjectName = "CodeCraft.NET.Test.Services"
            };
        }
    }
}