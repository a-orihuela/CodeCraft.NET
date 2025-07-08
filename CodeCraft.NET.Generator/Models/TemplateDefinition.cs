namespace CodeCraft.NET.Generator.Models
{
	public class TemplateDefinition
	{
		public string Type { get; set; } = string.Empty;   // E.g. "Create", "Update"
		public string Path { get; set; } = string.Empty;   // Template file path
		public string Suffix { get; set; } = string.Empty; // Output file suffix
	}
}
