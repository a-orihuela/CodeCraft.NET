namespace CodeCraft.NET.Generator.Models
{
	public class Files
	{
		public string Controller { get; set; } = string.Empty;
		public string DesktopService { get; set; } = string.Empty;
		public string Service { get; set; } = string.Empty;
		public string DesktopServiceRegistration { get; set; } = string.Empty;
		public string ServicesServiceRegistration { get; set; } = string.Empty;
		public string InfrastructureServiceRegistration { get; set; } = string.Empty;
		
		// MAUI Files
		public string MauiListPageGenerated { get; set; } = string.Empty;
		public string MauiListPage { get; set; } = string.Empty;
		public string MauiCreatePageGenerated { get; set; } = string.Empty;
		public string MauiCreatePage { get; set; } = string.Empty;
		public string MauiEditPageGenerated { get; set; } = string.Empty;
		public string MauiEditPage { get; set; } = string.Empty;
		public string MauiDetailPageGenerated { get; set; } = string.Empty;
		public string MauiDetailPage { get; set; } = string.Empty;
		public string MauiListViewModel { get; set; } = string.Empty;
		public string MauiListViewModelCustom { get; set; } = string.Empty;
		public string MauiCreateViewModel { get; set; } = string.Empty;
		public string MauiCreateViewModelCustom { get; set; } = string.Empty;
		public string MauiEditViewModel { get; set; } = string.Empty;
		public string MauiEditViewModelCustom { get; set; } = string.Empty;
		public string MauiDetailViewModel { get; set; } = string.Empty;
		public string MauiDetailViewModelCustom { get; set; } = string.Empty;
		public string MauiServiceRegistration { get; set; } = string.Empty;
		public string MauiProgram { get; set; } = string.Empty;
		public string EntityDtos { get; set; } = string.Empty;
		public string EntityWithRelatedDto { get; set; } = string.Empty;
		
		public string HttpRequest { get; set; } = string.Empty;
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
		public string MappingProfile { get; set; } = string.Empty;
		public string DbContext { get; set; } = string.Empty;
		public string DbContextFactory { get; set; } = string.Empty;
		public string RepositoryInterface { get; set; } = string.Empty;
		public string RepositoryImplementation { get; set; } = string.Empty;
		public string UnitOfWorkInterface { get; set; } = string.Empty;
		public string UnitOfWorkImplementation { get; set; } = string.Empty;
		public string UnitOfWorkInterfaceName { get; set; } = string.Empty;
		public string UnitOfWorkImplementationName { get; set; } = string.Empty;
		
		// Test Files
		public string ServiceTest { get; set; } = string.Empty;
		public string CommandCreateHandlerTest { get; set; } = string.Empty;
		public string CommandUpdateHandlerTest { get; set; } = string.Empty;
		public string CommandDeleteHandlerTest { get; set; } = string.Empty;
		public string QueryGetByIdHandlerTest { get; set; } = string.Empty;
		public string QueryGetWithRelatedHandlerTest { get; set; } = string.Empty;
		public string RepositoryTest { get; set; } = string.Empty;
		public string BaseServiceTest { get; set; } = string.Empty;
		public string BaseRepositoryTest { get; set; } = string.Empty;
		public string TestDbContext { get; set; } = string.Empty;
	}
}
