using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator
{
	public static class CleanupManager
	{
		/// <summary>
		/// Cleans all generated files including example entities from Domain project
		/// </summary>
		public static void CleanAll()
		{
			CleanGeneratedFiles(includeDomainEntities: true);
		}

		/// <summary>
		/// Cleans only generated files without touching Domain entities
		/// </summary>
		public static void CleanGeneratedFilesOnly()
		{
			CleanGeneratedFiles(includeDomainEntities: false);
		}

		private static void CleanGeneratedFiles(bool includeDomainEntities)
		{
			var config = CodeCraftConfig.Instance;
			int filesDeleted = 0;
			int directoriesDeleted = 0;

			if (includeDomainEntities)
			{
				Console.WriteLine("???  Cleaning generated files and example entities...");
				// Clean example entities from Domain
				filesDeleted += CleanExampleEntities(config);
			}

			// Clean CQRS Features (Application layer)
			filesDeleted += CleanCQRSFeatures(config);

			// Clean Repositories (Application contracts)
			filesDeleted += CleanRepositoryContracts(config);

			// Clean Repository implementations (Infrastructure)
			filesDeleted += CleanRepositoryImplementations(config);

			// Clean Unit of Work files
			filesDeleted += CleanUnitOfWorkFiles(config);

			// Clean Mapping files
			filesDeleted += CleanMappingFiles(config);

			// Clean DbContext files
			filesDeleted += CleanDbContextFiles(config);

			// Clean Controllers (WebAPI)
			filesDeleted += CleanControllers(config);

			// Clean Desktop Services (DesktopAPI)
			filesDeleted += CleanDesktopServices(config);

			// Clean API Request files
			filesDeleted += CleanApiRequestFiles(config);

			// Clean Specifications
			filesDeleted += CleanSpecifications(config);

			// Clean migrations
			directoriesDeleted += CleanMigrations(config);

			// Clean empty directories
			directoriesDeleted += CleanEmptyDirectories(config);

			if (includeDomainEntities)
			{
				Console.WriteLine($"?? Cleanup completed:");
				Console.WriteLine($"   ?? Files deleted: {filesDeleted}");
				Console.WriteLine($"   ?? Directories cleaned: {directoriesDeleted}");
			}
		}

		private static int CleanExampleEntities(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				var domainPath = config.GetSolutionRelativePath(config.ProjectNames.Domain);
				var modelPath = Path.Combine(domainPath, "Model");
				
				if (Directory.Exists(modelPath))
				{
					// Delete example entities like Product.cs
					var exampleEntities = new[] { "Product.cs", "User.cs", "Customer.cs", "Order.cs" };
					
					foreach (var entityFile in exampleEntities)
					{
						var filePath = Path.Combine(modelPath, entityFile);
						if (File.Exists(filePath))
						{
							File.Delete(filePath);
							count++;
							Console.WriteLine($"   ? Deleted example entity: {entityFile}");
						}
					}

					// Also clean the Model directory if it's empty
					if (Directory.GetFiles(modelPath).Length == 0 && Directory.GetDirectories(modelPath).Length == 0)
					{
						Directory.Delete(modelPath);
						Console.WriteLine($"   ? Deleted empty Model directory");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   ??  Could not clean example entities: {ex.Message}");
			}
			return count;
		}

		private static int CleanCQRSFeatures(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				// Get CQRS Features root directory directly
				var featuresPath = "CodeCraft.NET.Application/CQRS/Features";
				var fullFeaturesPath = Path.Combine(config.GetSolutionRoot(), featuresPath);

				if (Directory.Exists(fullFeaturesPath))
				{
					count = Directory.GetFiles(fullFeaturesPath, "*", SearchOption.AllDirectories).Length;
					Directory.Delete(fullFeaturesPath, recursive: true);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete CQRS Features: {ex.Message}");
			}
			return count;
		}

		private static int CleanRepositoryContracts(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				var repoPath = ConfigHelper.GetRepositoryInterfacePath("Sample");
				var repositoriesDir = Path.GetDirectoryName(repoPath);
				
				if (!string.IsNullOrEmpty(repositoriesDir))
				{
					var fullPath = Path.Combine(config.GetSolutionRoot(), repositoriesDir);
					if (Directory.Exists(fullPath))
					{
						// Delete only repository files, not the entire directory
						foreach (var file in Directory.GetFiles(fullPath, "*Repository.cs", SearchOption.AllDirectories))
						{
							File.Delete(file);
							count++;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete Repository interfaces: {ex.Message}");
			}
			return count;
		}

		private static int CleanRepositoryImplementations(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				var repoImplPath = ConfigHelper.GetRepositoryImplementationPath("Sample");
				var repositoriesDir = Path.GetDirectoryName(repoImplPath);
				
				if (!string.IsNullOrEmpty(repositoriesDir))
				{
					var fullPath = Path.Combine(config.GetSolutionRoot(), repositoriesDir);
					if (Directory.Exists(fullPath))
					{
						var repoFiles = Directory.GetFiles(fullPath, "*Repository.cs", SearchOption.AllDirectories);
						count = repoFiles.Length;
						
						foreach (var file in repoFiles)
						{
							File.Delete(file);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete Repository implementations: {ex.Message}");
			}
			return count;
		}

		private static int CleanUnitOfWorkFiles(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				// Clean interface
				var interfacePath = Path.Combine(config.GetSolutionRoot(), ConfigHelper.GetUnitOfWorkInterfacePath());
				if (File.Exists(interfacePath))
				{
					File.Delete(interfacePath);
					count++;
				}

				// Clean implementation
				var implementationPath = Path.Combine(config.GetSolutionRoot(), ConfigHelper.GetUnitOfWorkImplementationPath());
				if (File.Exists(implementationPath))
				{
					File.Delete(implementationPath);
					count++;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete Unit of Work files: {ex.Message}");
			}
			return count;
		}

		private static int CleanMappingFiles(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				var mappingPath = Path.Combine(config.GetSolutionRoot(), ConfigHelper.GetMappingProfilePath());
				if (File.Exists(mappingPath))
				{
					File.Delete(mappingPath);
					count++;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete Mapping Profile: {ex.Message}");
			}
			return count;
		}

		private static int CleanDbContextFiles(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				// Clean DbContext
				var dbContextPath = Path.Combine(config.GetSolutionRoot(), ConfigHelper.GetDbContextPath());
				if (File.Exists(dbContextPath))
				{
					File.Delete(dbContextPath);
					count++;
				}

				// Clean DbContext Factory
				var factoryPath = Path.Combine(config.GetSolutionRoot(), ConfigHelper.GetDbContextFactoryPath());
				if (File.Exists(factoryPath))
				{
					File.Delete(factoryPath);
					count++;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete DbContext files: {ex.Message}");
			}
			return count;
		}

		private static int CleanControllers(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				var controllerPath = ConfigHelper.GetControllerPath("Sample");
				var controllersDir = Path.GetDirectoryName(controllerPath);
				
				if (!string.IsNullOrEmpty(controllersDir))
				{
					var fullPath = Path.Combine(config.GetSolutionRoot(), controllersDir);
					if (Directory.Exists(fullPath))
					{
						var controllerFiles = Directory.GetFiles(fullPath, "*Controller.cs", SearchOption.AllDirectories);
						count = controllerFiles.Length;
						
						foreach (var file in controllerFiles)
						{
							File.Delete(file);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete Controllers: {ex.Message}");
			}
			return count;
		}

		private static int CleanDesktopServices(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				var servicePath = ConfigHelper.GetDesktopServicePath("Sample");
				var servicesDir = Path.GetDirectoryName(servicePath);
				
				if (!string.IsNullOrEmpty(servicesDir))
				{
					var fullPath = Path.Combine(config.GetSolutionRoot(), servicesDir);
					if (Directory.Exists(fullPath))
					{
						var serviceFiles = Directory.GetFiles(fullPath, "*Service.cs", SearchOption.AllDirectories);
						count = serviceFiles.Length;
						
						foreach (var file in serviceFiles)
						{
							File.Delete(file);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete Desktop Services: {ex.Message}");
			}
			return count;
		}

		private static int CleanApiRequestFiles(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				var requestPath = ConfigHelper.GetHttpRequestPath("Sample");
				var requestsDir = Path.GetDirectoryName(requestPath);
				
				if (!string.IsNullOrEmpty(requestsDir))
				{
					var fullPath = Path.Combine(config.GetSolutionRoot(), requestsDir);
					if (Directory.Exists(fullPath))
					{
						var requestFiles = Directory.GetFiles(fullPath, "*Requests.http", SearchOption.AllDirectories);
						count = requestFiles.Length;
						
						foreach (var file in requestFiles)
						{
							File.Delete(file);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete HTTP request files: {ex.Message}");
			}
			return count;
		}

		private static int CleanSpecifications(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				// Get Specifications root directory
				var samplePath = string.Format(config.Files.Specification, "Sample", "Sample");
				var specificationsPath = Path.GetDirectoryName(Path.GetDirectoryName(samplePath)); // Go up 2 levels
				
				if (!string.IsNullOrEmpty(specificationsPath))
				{
					var fullPath = Path.Combine(config.GetSolutionRoot(), specificationsPath.Replace('\\', '/'));
					if (Directory.Exists(fullPath))
					{
						count = Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories).Length;
						Directory.Delete(fullPath, recursive: true);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete Specifications: {ex.Message}");
			}
			return count;
		}

		private static int CleanMigrations(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				var infrastructurePath = config.GetSolutionRelativePath(config.ProjectNames.Infrastructure);
				var migrationsPath = Path.Combine(infrastructurePath, "Migrations");
				
				if (Directory.Exists(migrationsPath))
				{
					var migrationFiles = Directory.GetFiles(migrationsPath, "*.cs", SearchOption.TopDirectoryOnly)
						.Where(f => Path.GetFileName(f).StartsWith("AutoGen_") || Path.GetFileName(f).Contains("ApplicationDbContext"))
						.ToArray();
					
					foreach (var file in migrationFiles)
					{
						File.Delete(file);
						count++;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete migration files: {ex.Message}");
			}
			return count;
		}

		private static int CleanEmptyDirectories(CodeCraftConfig config)
		{
			int count = 0;
			try
			{
				var projectPaths = new[]
				{
					config.GetSolutionRelativePath(config.ProjectNames.Application),
					config.GetSolutionRelativePath(config.ProjectNames.Infrastructure),
					config.GetSolutionRelativePath(config.ProjectNames.Server),
					config.GetSolutionRelativePath(config.ProjectNames.Desktop)
				};

				foreach (var projectPath in projectPaths)
				{
					if (Directory.Exists(projectPath))
					{
						count += DeleteEmptyDirectoriesRecursive(projectPath);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot clean empty directories: {ex.Message}");
			}
			return count;
		}

		private static int DeleteEmptyDirectoriesRecursive(string startLocation)
		{
			int count = 0;
			foreach (var directory in Directory.GetDirectories(startLocation))
			{
				count += DeleteEmptyDirectoriesRecursive(directory);
				if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
				{
					try
					{
						Directory.Delete(directory, false);
						count++;
					}
					catch
					{
						// Ignore if we can't delete
					}
				}
			}
			return count;
		}
	}
}