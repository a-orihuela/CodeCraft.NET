namespace CodeCraft.NET.Generator.Models
{
	public class CodeCraftConfig
	{
		// Solution settings
		public string SolutionFileName { get; set; }
		public string SolutionRootOverride { get; set; }
		public string DomainProjectName { get; set; }
		public string ApplicationProjectName { get; set; }
		public string InfrastructureProjectName { get; set; }
		public string ServerProjectName { get; set; }
		public string CodeCraftNETGeneratorName { get; set; }
		public string CodeCraftNETCrossName { get; set; }

		// Folder structure
		public FolderStructure Folders { get; set; } = new FolderStructure();

		// Template files
		public TemplateFiles Templates { get; set; } = new TemplateFiles();

		// Database contexts
		public List<string> DbContexts { get; set; } =
		[
			"ApplicationDbContext",
			"BasicIdentityDbContext"
		];
	}
}