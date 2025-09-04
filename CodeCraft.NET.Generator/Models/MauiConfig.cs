namespace CodeCraft.NET.Generator.Models
{
	public class MauiConfig
	{
		public string GenerationStrategy { get; set; } = "BaseAndCustom";
		public bool PreserveCustomFiles { get; set; } = true;
		public string CustomFilesSuffix { get; set; } = "";
		public string GeneratedFilesSuffix { get; set; } = ".Generated";
		public List<string> GenerateOnlyIfNotExists { get; set; } = new();
		public string[] ExcludedFolders { get; set; } = Array.Empty<string>();
	}
}
