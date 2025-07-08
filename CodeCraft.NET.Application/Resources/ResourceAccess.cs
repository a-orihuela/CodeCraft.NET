using System.Resources;

namespace CodeCraft.NET.Application.Resources
{
	public static class ResourceAccess
	{
		public static string Messages(string value, ResourceManager resourceManager = null)
		{
			resourceManager ??= Resources.Messages.ResourceManager;
			return resourceManager.GetString(value);
		}
	}
}
