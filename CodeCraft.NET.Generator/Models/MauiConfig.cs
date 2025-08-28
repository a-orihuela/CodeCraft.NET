namespace CodeCraft.NET.Generator.Models
{
	public class MauiConfig
	{
		public string GenerationStrategy { get; set; } = "BaseAndCustom";
		public bool PreserveCustomFiles { get; set; } = true;
		public string CustomFilesSuffix { get; set; } = string.Empty;
		public string GeneratedFilesSuffix { get; set; } = ".Generated";
		public string[] GenerateOnlyIfNotExists { get; set; } = Array.Empty<string>();
	}
}