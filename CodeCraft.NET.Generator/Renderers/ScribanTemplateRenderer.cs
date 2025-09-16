using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using Scriban;
using Scriban.Runtime;
using Scriban.Parsing;

namespace CodeCraft.NET.Generator.Renderers
{
	public class ScribanTemplateRenderer : ITemplateRenderer
	{
		public void Render(string templatePath, string outputPath, object model)
		{
			// Ensure outputPath is absolute
			if (!Path.IsPathRooted(outputPath))
			{
				var solutionRoot = ConfigurationContext.GetSolutionRoot();
				outputPath = Path.Combine(solutionRoot, outputPath);
			}
			
			var templateContent = EmbeddedResourceHelper.LoadTemplate(templatePath);
			var template = Template.Parse(templateContent);

			if (template.HasErrors)
				throw new InvalidOperationException($"Template parsing failed: {string.Join(", ", template.Messages.Select(m => m.Message))}");

			var context = CreateContext(model);
			var result = template.Render(context);
			Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
			File.WriteAllText(outputPath, result);
			
			Console.WriteLine($"   ✅ Generated: {Path.GetFileName(outputPath)}");
		}

		private static TemplateContext CreateContext(object model)
		{
			var config = ConfigurationContext.Options;
			var context = new TemplateContext();
			
			// Configure template loader for embedded resources
			context.TemplateLoader = new EmbeddedTemplateLoader();
			
			var scriptObject = new ScriptObject
			{
				{ "DomainProjectName", config.Shared.ProjectNames["Domain"] },
				{ "ApplicationProjectName", config.Shared.ProjectNames["Application"] },
				{ "InfrastructureProjectName", config.Shared.ProjectNames["Infrastructure"] },
				{ "ServerProjectName", config.Shared.ProjectNames["Server"] },
				{ "DesktopProjectName", config.Shared.ProjectNames["Desktop"] },
				{ "CrossProjectName", config.Shared.ProjectNames["Cross"] },
				{ "UnitOfWorkInterfaceName", config.Shared.Files["UnitOfWorkInterfaceName"] },
				{ "UnitOfWorkImplementationName", config.Shared.Files["UnitOfWorkImplementationName"] },
				{ "DatabaseProvider", ConfigurationContext.ActiveProfile.DatabaseProvider }
			};

			scriptObject.Import(model, renamer: member => member.Name);

			scriptObject.Import("camelcase", new Func<string, string>(CamelCase));
			scriptObject.Import("downcase", new Func<string, string>(s => s?.ToLowerInvariant() ?? ""));

			context.PushGlobal(scriptObject);
			context.MemberRenamer = member => member.Name;
			context.MemberFilter = member => true;
			context.LoopLimit = 1000;
			return context;
		}

		private static string CamelCase(string input)
		{
			if (string.IsNullOrEmpty(input)) return input;
			if (input.Length == 1) return input.ToLowerInvariant();
			return char.ToLowerInvariant(input[0]) + input.Substring(1);
		}
	}
	
	/// <summary>
	/// Custom template loader for embedded resources
	/// </summary>
	public class EmbeddedTemplateLoader : ITemplateLoader
	{
		public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName)
		{
			return templateName;
		}

		public string Load(TemplateContext context, SourceSpan callerSpan, string templatePath)
		{
			return EmbeddedResourceHelper.LoadTemplate(templatePath);
		}

		public ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath)
		{
			return new ValueTask<string>(Load(context, callerSpan, templatePath));
		}
	}
}