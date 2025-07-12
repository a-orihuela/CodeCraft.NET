namespace CodeCraft.NET.Generator.Models
{
	public class TemplateFiles
	{
		// Command templates
		public string CreateTemplate { get; set; }
		public string CreateHandlerTemplate { get; set; }
		public string CreateValidatorTemplate { get; set; }
		public string UpdateTemplate { get; set; }
		public string UpdateHandlerTemplate { get; set; }
		public string UpdateValidatorTemplate { get; set; }
		public string DeleteTemplate { get; set; }
		public string DeleteHandlerTemplate { get; set; }

		// Query templates
		public string GetByIdQueryTemplate { get; set; }
		public string GetByIdHandlerTemplate { get; set; }
		public string GetWithRelatedQueryTemplate { get; set; }
		public string GetWithRelatedHandlerTemplate { get; set; }

		// Specification templates
		public string SpecificationTemplate { get; set; }
		public string SpecificationParamsTemplate { get; set; }
		public string WithRelatedTemplate { get; set; }
		public string WithRelatedSpecificationTemplate { get; set; }

		// Repository templates
		public string RepositoryInterfaceTemplate { get; set; }
		public string RepositoryImplementationTemplate { get; set; }
		public string UnitOfWorkInterfaceTemplate { get; set; }
		public string UnitOfWorkImplementationTemplate { get; set; }

		// Controller templates
		public string ControllerTemplate { get; set; }
		public string HttpRequestTemplate { get; set; }

		//DbContext templates
		public string DbContextTemplate { get; set; }
		public string DbContextFactoryTemplate { get; set; }


		// Output file suffixes
		public string CreateSuffix { get; set; }
		public string CreateHandlerSuffix { get; set; }
		public string CreateValidatorSuffix { get; set; }
		public string UpdateSuffix { get; set; }
		public string UpdateHandlerSuffix { get; set; }
		public string UpdateValidatorSuffix { get; set; }
		public string DeleteSuffix { get; set; }
		public string DeleteHandlerSuffix { get; set; }
		public string GetByIdQuerySuffix { get; set; }
		public string GetByIdHandlerSuffix { get; set; }
		public string GetWithRelatedQuerySuffix { get; set; }
		public string GetWithRelatedHandlerSuffix { get; set; }
		public string SpecificationSuffix { get; set; }
		public string SpecificationParamsSuffix { get; set; }
		public string WithRelatedSuffix { get; set; }
		public string WithRelatedSpecificationSuffix { get; set; }
		public string RepositoryInterfaceSuffix { get; set; }
		public string RepositoryImplementationSuffix { get; set; }
		public string ControllerSuffix { get; set; }
		public string HttpRequestSuffix { get; set; }
	}
}
