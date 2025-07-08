using CodeCraft.NET.Generator.Helpers;

namespace CodeCraft.NET.Generator.Models
{
	public static class TemplateLocator
	{
		private static string BuildTemplatePath(params string[] parts)
		{
			return string.Join(".", parts);
		}

		public static List<TemplateDefinition> GetCommandTemplates() => new()
		{
			// Create templates
			new TemplateDefinition {
				Type = CodeCraftGenSettings.CreateFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.CommandsFolder,
					CodeCraftGenSettings.CreateFolder, CodeCraftGenSettings.CreateTemplate),
				Suffix = CodeCraftGenSettings.CreateSuffix
			},
			new TemplateDefinition {
				Type = CodeCraftGenSettings.CreateFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.CommandsFolder,
					CodeCraftGenSettings.CreateFolder, CodeCraftGenSettings.CreateHandlerTemplate),
				Suffix = CodeCraftGenSettings.CreateHandlerSuffix
			},
			new TemplateDefinition {
				Type = CodeCraftGenSettings.CreateFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.CommandsFolder,
					CodeCraftGenSettings.CreateFolder, CodeCraftGenSettings.CreateValidatorTemplate),
				Suffix = CodeCraftGenSettings.CreateValidatorSuffix
			},

			// Update templates
			new TemplateDefinition {
				Type = CodeCraftGenSettings.UpdateFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.CommandsFolder,
					CodeCraftGenSettings.UpdateFolder, CodeCraftGenSettings.UpdateTemplate),
				Suffix = CodeCraftGenSettings.UpdateSuffix
			},
			new TemplateDefinition {
				Type = CodeCraftGenSettings.UpdateFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.CommandsFolder,
					CodeCraftGenSettings.UpdateFolder, CodeCraftGenSettings.UpdateHandlerTemplate),
				Suffix = CodeCraftGenSettings.UpdateHandlerSuffix
			},
			new TemplateDefinition {
				Type = CodeCraftGenSettings.UpdateFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.CommandsFolder,
					CodeCraftGenSettings.UpdateFolder, CodeCraftGenSettings.UpdateValidatorTemplate),
				Suffix = CodeCraftGenSettings.UpdateValidatorSuffix
			},

			// Delete templates
			new TemplateDefinition {
				Type = CodeCraftGenSettings.DeleteFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.CommandsFolder,
					CodeCraftGenSettings.DeleteFolder, CodeCraftGenSettings.DeleteTemplate),
				Suffix = CodeCraftGenSettings.DeleteSuffix
			},
			new TemplateDefinition {
				Type = CodeCraftGenSettings.DeleteFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.CommandsFolder,
					CodeCraftGenSettings.DeleteFolder, CodeCraftGenSettings.DeleteHandlerTemplate),
				Suffix = CodeCraftGenSettings.DeleteHandlerSuffix
			},
		};

		public static List<TemplateDefinition> GetQueryTemplates() => new()
		{
			new TemplateDefinition {
				Type = CodeCraftGenSettings.QueriesFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.QueriesFolder,
					CodeCraftGenSettings.GetByIdQueryTemplate),
				Suffix = CodeCraftGenSettings.GetByIdQuerySuffix
			},
			new TemplateDefinition {
				Type = CodeCraftGenSettings.QueriesFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.QueriesFolder,
					CodeCraftGenSettings.GetByIdHandlerTemplate),
				Suffix = CodeCraftGenSettings.GetByIdHandlerSuffix
			},
			new TemplateDefinition {
				Type = CodeCraftGenSettings.QueriesFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.QueriesFolder,
					CodeCraftGenSettings.GetWithRelatedQueryTemplate),
				Suffix = CodeCraftGenSettings.GetWithRelatedQuerySuffix
			},
			new TemplateDefinition {
				Type = CodeCraftGenSettings.QueriesFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.FeaturesFolder, CodeCraftGenSettings.QueriesFolder,
					CodeCraftGenSettings.GetWithRelatedHandlerTemplate),
				Suffix = CodeCraftGenSettings.GetWithRelatedHandlerSuffix
			}
		};

		public static List<TemplateDefinition> GetSpecificationTemplates() => new()
		{
			new TemplateDefinition {
				Type = CodeCraftGenSettings.SpecificationsFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.SpecificationsFolder, CodeCraftGenSettings.SpecificationTemplate),
				Suffix = CodeCraftGenSettings.SpecificationSuffix
			},
			new TemplateDefinition {
				Type = CodeCraftGenSettings.SpecificationsFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.SpecificationsFolder, CodeCraftGenSettings.SpecificationParamsTemplate),
				Suffix = CodeCraftGenSettings.SpecificationParamsSuffix
			},
			new TemplateDefinition {
				Type = CodeCraftGenSettings.SpecificationsFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.SpecificationsFolder, CodeCraftGenSettings.WithRelatedTemplate),
				Suffix = CodeCraftGenSettings.WithRelatedSuffix
			},
			new TemplateDefinition {
				Type = CodeCraftGenSettings.SpecificationsFolder,
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.CQRSFolder,
					CodeCraftGenSettings.SpecificationsFolder, CodeCraftGenSettings.WithRelatedSpecificationTemplate),
				Suffix = CodeCraftGenSettings.WithRelatedSpecificationSuffix
			}
		};

		public static List<TemplateDefinition> GetRepositoryTemplates() => new()
		{
			new TemplateDefinition {
				Type = "RepositoryInterface",
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.RepositoriesFolder,
					CodeCraftGenSettings.RepositoryInterfaceTemplate),
				Suffix = CodeCraftGenSettings.RepositoryInterfaceSuffix
			},
			new TemplateDefinition {
				Type = "RepositoryImplementation",
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.RepositoriesFolder,
					CodeCraftGenSettings.RepositoryImplementationTemplate),
				Suffix = CodeCraftGenSettings.RepositoryImplementationSuffix
			},
		};

		public static List<TemplateDefinition> GetUnitOfWorkTemplates() => new()
		{
			new TemplateDefinition {
				Type = "UnitOfWorkInterface",
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.RepositoriesFolder,
					CodeCraftGenSettings.UnitOfWorkInterfaceTemplate),
				Suffix = CodeCraftGenSettings.UnitOfWorkInterfaceFileName
			},
			new TemplateDefinition {
				Type = "UnitOfWorkImplementation",
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.RepositoriesFolder,
					CodeCraftGenSettings.UnitOfWorkImplementationTemplate),
				Suffix = CodeCraftGenSettings.UnitOfWorkImplementationFileName
			}
		};

		public static List<TemplateDefinition> GetMappingTemplates() => new()
		{
			new TemplateDefinition {
				Type = "MappingProfile",
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.MappingFolder,
					CodeCraftGenSettings.MappingProfileFileName),
				Suffix = "MappingProfile.cs"
			}
		};

		public static List<TemplateDefinition> GetControllerTemplates() => new()
		{
			new TemplateDefinition {
				Type = "Controller",
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.ControllersFolder,
					CodeCraftGenSettings.ControllerTemplate),
				Suffix = CodeCraftGenSettings.ControllerSuffix
			},
			new TemplateDefinition {
				Type = "HttpRequest",
				Path = BuildTemplatePath(CodeCraftGenSettings.TemplatesFolder, CodeCraftGenSettings.HttpRequestsFolder,
					CodeCraftGenSettings.HttpRequestTemplate),
				Suffix = CodeCraftGenSettings.HttpRequestSuffix
			}
		};
	}
}