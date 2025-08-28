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
				
				// 3. Generate Pages Custom (only if they don't exist)
				GeneratePagesCustomIfNotExists(entity);
			}
			
			// 4. Generate common services
			GenerateCommonServices(entities);
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

			// Custom ViewModels (only if they don't exist)
			GenerateCustomViewModelIfNotExists(entity, "List");
			GenerateCustomViewModelIfNotExists(entity, "Create");
			GenerateCustomViewModelIfNotExists(entity, "Edit");
			GenerateCustomViewModelIfNotExists(entity, "Detail");
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

		private void GeneratePagesCustomIfNotExists(EntityMetadata entity)
		{
			var context = CreateTemplateContext(entity);
			
			// Custom Pages (only if they don't exist)
			GenerateIfNotExists(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiListPage)),
				GetMauiListPagePath(entity.Name),
				context,
				$"{entity.Name}ListPage.xaml");

			GenerateIfNotExists(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiCreatePage)),
				GetMauiCreatePagePath(entity.Name),
				context,
				$"{entity.Name}CreatePage.xaml");

			GenerateIfNotExists(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiEditPage)),
				GetMauiEditPagePath(entity.Name),
				context,
				$"{entity.Name}EditPage.xaml");

			GenerateIfNotExists(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiDetailPage)),
				GetMauiDetailPagePath(entity.Name),
				context,
				$"{entity.Name}DetailPage.xaml");
		}

		private void GenerateCustomViewModelIfNotExists(EntityMetadata entity, string viewType)
		{
			var customPath = GetMauiViewModelCustomPath(entity.Name, viewType);
			var config = CodeCraftConfig.Instance;
			var fullPath = Path.Combine(config.GetSolutionRoot(), customPath);
			
			if (!File.Exists(fullPath))
			{
				// ? USE SCRIBAN TEMPLATE instead of hardcoded string
				var context = CreateCustomViewModelContext(entity, viewType);
				_templateRenderer.Render(
					ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiCustomViewModel)),
					customPath,
					context);
				Console.WriteLine($"   Generated: ViewModels/Custom/{entity.Name}/{entity.Name}{viewType}ViewModel.Custom.cs");
			}
			else
			{
				Console.WriteLine($"   Preserved: ViewModels/Custom/{entity.Name}/{entity.Name}{viewType}ViewModel.Custom.cs - Custom file exists");
			}
		}

		private void GenerateIfNotExists(string templatePath, string outputPath, object context, string displayName)
		{
			var config = CodeCraftConfig.Instance;
			var fullPath = Path.Combine(config.GetSolutionRoot(), outputPath);
			
			if (!File.Exists(fullPath))
			{
				_templateRenderer.Render(templatePath, outputPath, context);
				Console.WriteLine($"   Generated: Views/Custom/{displayName}");
			}
			else
			{
				Console.WriteLine($"   Preserved: Views/Custom/{displayName} - Custom file exists");
			}
		}

		private void GenerateCommonServices(IEnumerable<EntityMetadata> entities)
		{
			var context = new { entities };
			
			_templateRenderer.Render(
				ConfigHelper.GetTemplatePath(nameof(CodeCraftConfig.Instance.Templates.MauiCommonServices)),
				"CodeCraft.NET.MAUI/Services/CommonServices.cs",
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

		private object CreateCustomViewModelContext(EntityMetadata entity, string viewType)
		{
			var config = CodeCraftConfig.Instance;
			return new
			{
				entity.Name,
				name = entity.Name.ToLowerInvariant(),
				entity.NamePlural,
				entity.Properties,
				entity.Usings,
				ViewType = viewType,
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

		private string GetMauiViewModelCustomPath(string entityName, string viewType) =>
			$"CodeCraft.NET.MAUI/ViewModels/Custom/{entityName}/{entityName}{viewType}ViewModel.Custom.cs";

		private string GetMauiListPageGeneratedPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiListPageGenerated, entityName);

		private string GetMauiCreatePageGeneratedPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiCreatePageGenerated, entityName);

		private string GetMauiEditPageGeneratedPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiEditPageGenerated, entityName);

		private string GetMauiDetailPageGeneratedPath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiDetailPageGenerated, entityName);

		private string GetMauiListPagePath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiListPage, entityName);

		private string GetMauiCreatePagePath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiCreatePage, entityName);

		private string GetMauiEditPagePath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiEditPage, entityName);

		private string GetMauiDetailPagePath(string entityName) =>
			string.Format(CodeCraftConfig.Instance.Files.MauiDetailPage, entityName);

		private string GetMauiServiceRegistrationPath() =>
			CodeCraftConfig.Instance.Files.MauiServiceRegistration;
	}
}