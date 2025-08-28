namespace CodeCraft.NET.Generator.Models
{
	public class DataBaseConfig
	{
		public string ConnectionString { get; set; } = string.Empty;
		public string SqliteConnectionString { get; set; } = string.Empty;
		public string MigrationsAssembly { get; set; } = string.Empty;
		public string DatabaseProvider { get; set; } = "SqlServer"; // SqlServer, SQLite
		
		public string GetConnectionStringForProvider(string provider)
		{
			return provider.Equals("SQLite", StringComparison.OrdinalIgnoreCase) 
				? SqliteConnectionString 
				: ConnectionString;
		}
	}
}
