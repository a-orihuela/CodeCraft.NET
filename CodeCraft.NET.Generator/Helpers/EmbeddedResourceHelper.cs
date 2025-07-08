using System.Reflection;

namespace CodeCraft.NET.Generator.Helpers
{
	public static class EmbeddedResourceHelper
	{
		public static string LoadTemplate(string resourcePath)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resourceKey = resourcePath.Replace("\\", ".").Replace("/", ".");
			var allResources = assembly.GetManifestResourceNames();
			var fullResourceName = allResources.FirstOrDefault(r => r.EndsWith(resourceKey));

			if (fullResourceName == null)
			{
				var available = string.Join(Environment.NewLine, allResources);
				throw new FileNotFoundException($"Embedded template not found: {resourcePath}\nSearched for: *{resourceKey}\nAvailable resources:\n{available}");
			}

			using var stream = assembly.GetManifestResourceStream(fullResourceName);
			using var reader = new StreamReader(stream!);
			return reader.ReadToEnd();
		}
	}
}
