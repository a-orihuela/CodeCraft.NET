using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
    public class InfrastructureGenerator
    {
        private readonly ITemplateRenderer _templateRenderer;

        public InfrastructureGenerator(ITemplateRenderer templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }

        public void Generate(string databaseProvider)
        {
            // Ensure correct NuGet packages are installed
            NuGetPackageManager.EnsureEntityFrameworkPackage(databaseProvider);
            
            // Generate the service registration file
            GenerateInfrastructureServiceRegistration(databaseProvider);
            
            // Restore packages if changes were made
            NuGetPackageManager.RestorePackages();
        }

        private void GenerateInfrastructureServiceRegistration(string databaseProvider)
        {
            var templatePath = ConfigHelper.GetTemplatePath("InfrastructureServiceRegistration");
            var outputPath = ConfigHelper.GetInfrastructureServiceRegistrationPath();

            var config = ConfigurationContext.Options;
            var context = new
            {
                DatabaseProvider = databaseProvider,
                InfrastructureProjectName = config.Shared.ProjectNames["Infrastructure"]
            };

            _templateRenderer.Render(templatePath, outputPath, context);
        }
    }
}