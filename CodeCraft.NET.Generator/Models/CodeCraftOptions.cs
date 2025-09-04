namespace CodeCraft.NET.Generator.Models
{
	public class CodeCraftOptions
	{
		public const string SectionName = "CodeCraft";

		public SharedConfig Shared { get; set; } = new();
		public Dictionary<string, ProfileConfig> Profiles { get; set; } = new();
		public string DefaultProfile { get; set; } = "dev";

		public ProfileConfig GetActiveProfile(string? profileName = null)
		{
			var profile = profileName ?? DefaultProfile;
			return Profiles.TryGetValue(profile, out var config) ? config : Profiles[DefaultProfile];
		}
		public List<string> GenerateOnlyIfNotExists { get; set; } = new();
	}
}




