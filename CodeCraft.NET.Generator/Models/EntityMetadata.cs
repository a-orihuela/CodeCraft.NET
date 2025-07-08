namespace CodeCraft.NET.Generator.Models
{
	public class EntityMetadata
	{
		public string Name { get; set; } = string.Empty;
		public string NamePlural { get; set; } = string.Empty;
		public List<PropertyMetadata> Properties { get; set; } = new List<PropertyMetadata>();
		public HashSet<string> Usings { get; set; } = new HashSet<string>();
	}
}
