using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Generators
{
	public class MauiGenerator
	{
		private readonly ITemplateRenderer _templateRenderer;

		public MauiGenerator(ITemplateRenderer templateRenderer)
		{
			_templateRenderer = templateRenderer;
		}

		public void Generate(IEnumerable<EntityMetadata> entities)
		{
			Console.WriteLine("Generating MAUI components...");
			
			// Filter out entities that shouldn't be generated based on configuration
			var filteredEntities = FilterEntitiesForGeneration(entities);
			
			foreach (var entity in filteredEntities)
			{
				Console.WriteLine($"   Generating MAUI files for {entity.Name}...");
				
				// 1. Generate ViewModels (always regenerated - they are partial)
				GenerateViewModels(entity);
				
				// 2. Generate Pages Generated (always regenerated)
				GeneratePagesGenerated(entity);
				
				// 3. Generate Pages Code-Behind (always regenerated)
				GeneratePagesCodeBehind(entity);
			}
			
			// 4. Generate common services
			GenerateServiceRegistration(filteredEntities);
			GenerateShellRouting(filteredEntities);
		}

		private IEnumerable<EntityMetadata> FilterEntitiesForGeneration(IEnumerable<EntityMetadata> entities)
		{
			// Exclude known non-entity folders like Examples, Custom, Orchestration, etc.
			var excludedNames = new[] { 
				"Examples", "Custom", "Base", "Helpers", "Extensions", 
				"Orchestration", "Common", "Shared" 
			};
			
			return entities.Where(entity => 
				!excludedNames.Any(excluded => 
					entity.Name.Equals(excluded, StringComparison.OrdinalIgnoreCase)));
		}

		private void GenerateViewModels(EntityMetadata entity)
		{
			var context = CreateTemplateContext(entity);
			
			// Main ViewModels (always regenerated)
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiListViewModel"),
				GetMauiListViewModelPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiCreateViewModel"),
				GetMauiCreateViewModelPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiEditViewModel"),
				GetMauiEditViewModelPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiDetailViewModel"),
				GetMauiDetailViewModelPath(entity.Name),
				context);
		}

		private void GeneratePagesGenerated(EntityMetadata entity)
		{
			var context = CreateTemplateContext(entity);
			
			// Generated Pages XAML (always regenerated)
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiListPageGenerated"),
				GetMauiListPageGeneratedPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiCreatePageGenerated"),
				GetMauiCreatePageGeneratedPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiEditPageGenerated"),
				GetMauiEditPageGeneratedPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiDetailPageGenerated"),
				GetMauiDetailPageGeneratedPath(entity.Name),
				context);
		}

		private void GeneratePagesCodeBehind(EntityMetadata entity)
		{
			var context = CreateTemplateContext(entity);
			
			// Generated Pages Code-Behind (always regenerated)
			_templateRenderer.Render(
				"CodeCraft.NET.Generator.Templates.MAUI.Pages.ListPageCodeBehind.scriban",
				GetMauiListPageGeneratedCodeBehindPath(entity.Name),
				context);

			_templateRenderer.Render(
				"CodeCraft.NET.Generator.Templates.MAUI.Pages.CreatePageCodeBehind.scriban",
				GetMauiCreatePageGeneratedCodeBehindPath(entity.Name),
				context);

			_templateRenderer.Render(
				"CodeCraft.NET.Generator.Templates.MAUI.Pages.EditPageCodeBehind.scriban",
				GetMauiEditPageGeneratedCodeBehindPath(entity.Name),
				context);

			_templateRenderer.Render(
				"CodeCraft.NET.Generator.Templates.MAUI.Pages.DetailPageCodeBehind.scriban",
				GetMauiDetailPageGeneratedCodeBehindPath(entity.Name),
				context);
		}

		private void GenerateServiceRegistration(IEnumerable<EntityMetadata> entities)
		{
			var context = new { entities };
			
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiServiceRegistration"),
				GetMauiServiceRegistrationPath(),
				context);
		}

		private void GenerateShellRouting(IEnumerable<EntityMetadata> entities)
		{
			var context = new { entities };
			
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath("MauiShellRouting"),
				"CodeCraft.NET.MAUI/ShellRouting.Generated.cs",
				context);
		}

		private object CreateTemplateContext(EntityMetadata entity)
		{
			var config = ConfigurationContext.Options;
			return new
			{
				entity.Name,
				name = entity.Name.ToLowerInvariant(),
				entity.NamePlural,
				entity.Properties,
				entity.Usings,
				MauiProjectName = "CodeCraft.NET.MAUI",
				DesktopProjectName = config.Shared.ProjectNames["Desktop"],
				ApplicationProjectName = config.Shared.ProjectNames["Application"],
				DomainProjectName = config.Shared.ProjectNames["Domain"]
			};
		}

		// Helper methods for paths
		private string GetMauiListViewModelPath(string entityName) =>
			string.Format(ConfigurationContext.Options.Shared.Files["MauiListViewModel"], entityName);

		private string GetMauiCreateViewModelPath(string entityName) =>
			string.Format(ConfigurationContext.Options.Shared.Files["MauiCreateViewModel"], entityName);

		private string GetMauiEditViewModelPath(string entityName) =>
			string.Format(ConfigurationContext.Options.Shared.Files["MauiEditViewModel"], entityName);

		private string GetMauiDetailViewModelPath(string entityName) =>
			string.Format(ConfigurationContext.Options.Shared.Files["MauiDetailViewModel"], entityName);

		private string GetMauiListPageGeneratedPath(string entityName) =>
			string.Format(ConfigurationContext.Options.Shared.Files["MauiListPageGenerated"], entityName);

		private string GetMauiCreatePageGeneratedPath(string entityName) =>
			string.Format(ConfigurationContext.Options.Shared.Files["MauiCreatePageGenerated"], entityName);

		private string GetMauiEditPageGeneratedPath(string entityName) =>
			string.Format(ConfigurationContext.Options.Shared.Files["MauiEditPageGenerated"], entityName);

		private string GetMauiDetailPageGeneratedPath(string entityName) =>
			string.Format(ConfigurationContext.Options.Shared.Files["MauiDetailPageGenerated"], entityName);

		// Code-behind paths
		private string GetMauiListPageGeneratedCodeBehindPath(string entityName) =>
			$"CodeCraft.NET.MAUI/Views/{entityName}/{entityName}ListPage.Generated.xaml.cs";

		private string GetMauiCreatePageGeneratedCodeBehindPath(string entityName) =>
			$"CodeCraft.NET.MAUI/Views/{entityName}/{entityName}CreatePage.Generated.xaml.cs";

		private string GetMauiEditPageGeneratedCodeBehindPath(string entityName) =>
			$"CodeCraft.NET.MAUI/Views/{entityName}/{entityName}EditPage.Generated.xaml.cs";

		private string GetMauiDetailPageGeneratedCodeBehindPath(string entityName) =>
			$"CodeCraft.NET.MAUI/Views/{entityName}/{entityName}DetailPage.Generated.xaml.cs";

		private string GetMauiServiceRegistrationPath() =>
			ConfigurationContext.Options.Shared.Files["MauiServiceRegistration"];
	}
}