namespace CodeCraft.NET.Generator.Models
{
	public class PropertyMetadata
	{
		public string Name { get; set; } = string.Empty;
		public string Type { get; set; } = string.Empty;
		public bool IsNavigation { get; set; } = false;

		public string TypeName
		{
			get
			{
				if (Type.StartsWith("Nullable<"))
					return Type.Substring(9, Type.Length - 10); // Extrae "DateTime" de "Nullable<DateTime>"
				return Type;
			}
		}
	}
}
