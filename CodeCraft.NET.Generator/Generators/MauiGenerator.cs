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
			
			foreach (var entity in entities)
			{
				Console.WriteLine($"   Generating MAUI files for {entity.Name}...");
				
				// 1. Generate ViewModels (always regenerated - they are partial)
				GenerateViewModels(entity);
				
				// 2. Generate Pages Generated (always regenerated)
				GeneratePagesGenerated(entity);
			}
			
			// 3. Generate common services
			GenerateServiceRegistration(entities);
			GenerateShellRouting(entities);
		}

		private void GenerateViewModels(EntityMetadata entity)
		{
			var context = CreateTemplateContext(entity);
			
			// Main ViewModels (always regenerated)
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiListViewModel)),
				GetMauiListViewModelPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiCreateViewModel)),
				GetMauiCreateViewModelPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiEditViewModel)),
				GetMauiEditViewModelPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiDetailViewModel)),
				GetMauiDetailViewModelPath(entity.Name),
				context);
		}

		private void GeneratePagesGenerated(EntityMetadata entity)
		{
			var context = CreateTemplateContext(entity);
			
			// Generated Pages (always regenerated)
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiListPageGenerated)),
				GetMauiListPageGeneratedPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiCreatePageGenerated)),
				GetMauiCreatePageGeneratedPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiEditPageGenerated)),
				GetMauiEditPageGeneratedPath(entity.Name),
				context);

			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiDetailPageGenerated)),
				GetMauiDetailPageGeneratedPath(entity.Name),
				context);
		}

		private void GenerateServiceRegistration(IEnumerable<EntityMetadata> entities)
		{
			var context = new { entities };
			
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiServiceRegistration)),
				GetMauiServiceRegistrationPath(),
				context);
		}

		private void GenerateShellRouting(IEnumerable<EntityMetadata> entities)
		{
			var context = new { entities };
			
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiShellRouting)),
				"CodeCraft.NET.MAUI/AppShell.Generated.cs",
				context);
		}

		private object CreateTemplateContext(EntityMetadata entity)
		{
			var config = CodeCraftConfig.Instance;
			return new
			{
				entity.Name,
				name = entity.Name.ToLowerInvariant(),
				entity.NamePlural,
				entity.Properties,
				entity.Usings,
				MauiProjectName = "CodeCraft.NET.MAUI",
				DesktopProjectName = config.ProjectNames.Desktop,
				ApplicationProjectName = config.ProjectNames.Application,
				DomainProjectName = config.ProjectNames.Domain
			};
		}

		// Helper methods for paths
		private string GetMauiListViewModelPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiListViewModel, entityName);

		private string GetMauiCreateViewModelPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiCreateViewModel, entityName);

		private string GetMauiEditViewModelPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiEditViewModel, entityName);

		private string GetMauiDetailViewModelPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiDetailViewModel, entityName);

		private string GetMauiListPageGeneratedPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiListPageGenerated, entityName);

		private string GetMauiCreatePageGeneratedPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiCreatePageGenerated, entityName);

		private string GetMauiEditPageGeneratedPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiEditPageGenerated, entityName);

		private string GetMauiDetailPageGeneratedPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiDetailPageGenerated, entityName);

		private string GetMauiServiceRegistrationPath() =>
			CodeCraftConfig.Instance.Files.MauiServiceRegistration;
	}
}