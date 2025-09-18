using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
    /// <summary>
    /// Generator for Infrastructure test classes
    /// Focuses on Repository, UnitOfWork, and DbContext testing
    /// </summary>
    public class InfrastructureTestGenerator
    {
        private readonly ITemplateRenderer _templateRenderer;

        public InfrastructureTestGenerator(ITemplateRenderer templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }

        /// <summary>
        /// Generates all Infrastructure tests for a given entity
        /// </summary>
        public void Generate(EntityMetadata entity)
        {
            // Repository Tests - Most important for Infrastructure
            GenerateRepositoryTests(entity);
        }

        /// <summary>
        /// Generates Repository test classes
        /// Tests CRUD operations, queries, and business logic
        /// </summary>
        public void GenerateRepositoryTests(EntityMetadata entity)
        {
            _templateRenderer.Render(
                ConfigHelper.GetTemplatePath("RepositoryTest"),
                ConfigHelper.GetRepositoryTestPath(entity.Name),
                CreateRepositoryTestContext(entity));
        }

        /// <summary>
        /// Generates UnitOfWork tests (shared across all entities)
        /// </summary>
        public void GenerateUnitOfWorkTests()
        {
            // Get entities from the domain project analysis
            var domainEntitiesPath = Path.Combine(Directory.GetCurrentDirectory(), "../CodeCraft.NET.Domain/Model");
            var entities = EntityAnalyzer.AnalyzeDomainEntities();
            
            _templateRenderer.Render(
                ConfigHelper.GetTemplatePath("UnitOfWorkTest"),
                ConfigHelper.GetUnitOfWorkTestPath(),
                CreateUnitOfWorkTestContext(entities));
        }

        /// <summary>
        /// Generates DbContext tests (shared across all entities)
        /// </summary>
        public void GenerateDbContextTests()
        {
            // Get entities from the domain project analysis
            var entities = EntityAnalyzer.AnalyzeDomainEntities();
            
            _templateRenderer.Render(
                ConfigHelper.GetTemplatePath("DbContextTest"),
                ConfigHelper.GetDbContextTestPath(),
                CreateDbContextTestContext(entities));
        }

        private object CreateRepositoryTestContext(EntityMetadata entity)
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
                InfrastructureProjectName = config.Shared.ProjectNames["Infrastructure"],
                TestInfrastructureProjectName = $"CodeCraft.NET.Test.Infrastructure",
                EntityNamespace = $"{config.Shared.ProjectNames["Domain"]}.Model",
                RepositoryNamespace = $"{config.Shared.ProjectNames["Infrastructure"]}.Persistence.Repositories",
                RepositoryInterfaceNamespace = $"{config.Shared.ProjectNames["Application"]}.Contracts.Persistence.Repositories",
                DbContextNamespace = $"{config.Shared.ProjectNames["Infrastructure"]}.Persistence"
            };
        }

        private object CreateUnitOfWorkTestContext(IEnumerable<EntityMetadata> entities)
        {
            var config = ConfigurationContext.Options;
            return new
            {
                entities,
                ApplicationProjectName = config.Shared.ProjectNames["Application"],
                DomainProjectName = config.Shared.ProjectNames["Domain"],
                InfrastructureProjectName = config.Shared.ProjectNames["Infrastructure"],
                TestInfrastructureProjectName = "CodeCraft.NET.Test.Infrastructure"
            };
        }

        private object CreateDbContextTestContext(IEnumerable<EntityMetadata> entities)
        {
            var config = ConfigurationContext.Options;
            return new
            {
                entities,
                ApplicationProjectName = config.Shared.ProjectNames["Application"],
                DomainProjectName = config.Shared.ProjectNames["Domain"],
                InfrastructureProjectName = config.Shared.ProjectNames["Infrastructure"],
                TestInfrastructureProjectName = "CodeCraft.NET.Test.Infrastructure"
            };
        }
    }
}