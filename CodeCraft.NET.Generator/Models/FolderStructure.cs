namespace CodeCraft.NET.Generator.Models
{
	public class FolderStructure
	{
		// Templates
		public string TemplatesFolder { get; set; }
		public string CQRSFolder { get; set; }
		public string FeaturesFolder { get; set; }
		public string CommandsFolder { get; set; }
		public string CreateFolder { get; set; }
		public string UpdateFolder { get; set; }
		public string DeleteFolder { get; set; }
		public string QueriesFolder { get; set; }
		public string SpecificationsFolder { get; set; }
		public string MappingFolder { get; set; }
		public string RepositoriesFolder { get; set; }
		public string ControllersFolder { get; set; }
		public string HttpRequestsFolder { get; set; }
		public string DbContext {  get; set; }

		// Application structure
		public string ContractsFolder { get; set; }
		public string PersistenceFolder { get; set; }

		// Tests
		public string TestsFolder { get; set; }
		public string ApiRequestsFolder { get; set; }

		// File naming
		public string MappingProfileFileName { get; set; }
		public string UnitOfWorkInterfaceFileName { get; set; }
		public string UnitOfWorkInterfaceName { get; set; }
		public string UnitOfWorkImplementationFileName { get; set; }
		public string UnitOfWorkImplementationName { get; set; }
	}
}
