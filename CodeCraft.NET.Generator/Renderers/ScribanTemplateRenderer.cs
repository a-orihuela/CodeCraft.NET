using CodeCraft.NET.Generator.Contracts;
using CodeCraft.NET.Generator.Helpers;
using Scriban;
using Scriban.Runtime;

namespace CodeCraft.NET.Generator.Renderers
{
	public class ScribanTemplateRenderer : ITemplateRenderer
	{
		public void Render(string templatePath, string outputPath, object model)
		{
			var templateContent = EmbeddedResourceHelper.LoadTemplate(templatePath);
			var template = Template.Parse(templateContent);

			if (template.HasErrors)
				throw new InvalidOperationException($"Template parsing failed: {string.Join(", ", template.Messages.Select(m => m.Message))}");

			var context = CreateContext(model);
			var result = template.Render(context);
			Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
			File.WriteAllText(outputPath, result);
		}

		private static TemplateContext? CreateContext(object model) 
		{
			var context = new TemplateContext();
			var scriptObject = new ScriptObject
			{
				{ "DomainProjectName", CodeCraftGenSettings.DomainProjectName },
				{ "ApplicationProjectName", CodeCraftGenSettings.ApplicationProjectName },
				{ "InfrastructureProjectName", CodeCraftGenSettings.InfrastructureProjectName },
				{ "ServerProjectName", CodeCraftGenSettings.ServerProjectName },
				{ "CodeCraftNETCrossName", CodeCraftGenSettings.CodeCraftNETCrossName },
				{ "UnitOfWorkInterfaceName", CodeCraftGenSettings.UnitOfWorkInterfaceName },
				{ "UnitOfWorkImplementationName", CodeCraftGenSettings.UnitOfWorkImplementationName },
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
}
