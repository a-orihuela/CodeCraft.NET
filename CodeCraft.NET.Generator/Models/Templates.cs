namespace CodeCraft.NET.Generator.Models
{
	public class Templates
	{
		public string Controller { get; set; } = string.Empty;
		public string CommandCreate { get; set; } = string.Empty;
		public string CommandCreateHandler { get; set; } = string.Empty;
		public string CommandCreateValidator { get; set; } = string.Empty;
		public string CommandDelete { get; set; } = string.Empty;
		public string CommandDeleteHandler { get; set; } = string.Empty;
		public string CommandUpdate { get; set; } = string.Empty;
		public string CommandUpdateHandler { get; set; } = string.Empty;
		public string CommandUpdateValidator { get; set; } = string.Empty;
		public string QueryGetById { get; set; } = string.Empty;
		public string QueryGetByIdHandler { get; set; } = string.Empty;
		public string QueryGetWithRelated { get; set; } = string.Empty;
		public string QueryGetWithRelatedHandler { get; set; } = string.Empty;
		public string Specification { get; set; } = string.Empty;
		public string SpecificationParams { get; set; } = string.Empty;
		public string WithRelated { get; set; } = string.Empty;
		public string WithRelatedSpecification { get; set; } = string.Empty;
		public string HttpRequest { get; set; } = string.Empty;
		public string MappingProfile { get; set; } = string.Empty;
		public string DbContext { get; set; } = string.Empty;
		public string DbContextFactory { get; set; } = string.Empty;
		public string RepositoryInterface { get; set; } = string.Empty;
		public string RepositoryImplementation { get; set; } = string.Empty;
		public string UnitOfWorkInterface { get; set; } = string.Empty;
		public string UnitOfWorkImplementation { get; set; } = string.Empty;
		public string DesktopService { get; set; } = string.Empty;
		public string DesktopServiceRegistration { get; set; } = string.Empty;
		public string InfrastructureServiceRegistration { get; set; } = string.Empty;
	}
}
