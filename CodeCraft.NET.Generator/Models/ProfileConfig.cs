namespace CodeCraft.NET.Generator.Models
{
	public class ProfileConfig
	{
		public string DatabaseProvider { get; set; } = "SQLite";
		public string ConnectionString { get; set; } = "Data Source=CodeCraftDb.db";
		public bool GenerateWebApi { get; set; } = true;
		public bool GenerateDesktopApi { get; set; } = true;
		public bool GenerateMaui { get; set; } = true;
		public bool OverwriteExisting { get; set; } = false;
		public bool ForceMigrations { get; set; } = false;
	}
}
