using CodeCraft.NET.Generator.Helpers;

namespace CodeCraft.NET.Generator
{
	public static class CleanupManager
	{
		/// <summary>
		/// Cleans only generated files without touching Domain entities
		/// </summary>
		public static void CleanGeneratedFilesOnly()
		{
			var config = ConfigurationContext.Options;
			int filesDeleted = 0;
			int directoriesDeleted = 0;

			// Clean CQRS Features (Application layer)
			filesDeleted += CleanCQRSFeatures();

			// Clean Repositories (Application contracts)
			filesDeleted += CleanRepositoryContracts();

			// Clean Repository implementations (Infrastructure)
			filesDeleted += CleanRepositoryImplementations();

			// Clean Unit of Work files
			filesDeleted += CleanUnitOfWorkFiles();

			// Clean Mapping files
			filesDeleted += CleanMappingFiles();

			// Clean DbContext files
			filesDeleted += CleanDbContextFiles();

			// Clean Controllers (WebAPI)
			filesDeleted += CleanControllers();

			// Clean Desktop Services (DesktopAPI)
			filesDeleted += CleanDesktopServices();

			// Clean API Request files
			filesDeleted += CleanApiRequestFiles();

			// Clean Specifications
			filesDeleted += CleanSpecifications();

			// Clean migrations
			directoriesDeleted += CleanMigrations();

			// Clean empty directories
			directoriesDeleted += CleanEmptyDirectories();
		}

		private static int CleanCQRSFeatures()
		{
			int count = 0;
			try
			{
				var config = ConfigurationContext.Options;
				// Get CQRS Features root directory directly
				var featuresPath = $"{config.Shared.ProjectNames["Application"]}/CQRS/Features";
				var fullFeaturesPath = Path.Combine(ConfigurationContext.GetSolutionRoot(), featuresPath);

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

		private static int CleanRepositoryContracts()
		{
			int count = 0;
			try
			{
				var repoPath = ConfigHelper.GetRepositoryInterfacePath("Sample");
				var repositoriesDir = Path.GetDirectoryName(repoPath);
				
				if (!string.IsNullOrEmpty(repositoriesDir))
				{
					var fullPath = Path.Combine(ConfigurationContext.GetSolutionRoot(), repositoriesDir);
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

		private static int CleanRepositoryImplementations()
		{
			int count = 0;
			try
			{
				var repoImplPath = ConfigHelper.GetRepositoryImplementationPath("Sample");
				var repositoriesDir = Path.GetDirectoryName(repoImplPath);
				
				if (!string.IsNullOrEmpty(repositoriesDir))
				{
					var fullPath = Path.Combine(ConfigurationContext.GetSolutionRoot(), repositoriesDir);
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

		private static int CleanUnitOfWorkFiles()
		{
			int count = 0;
			try
			{
				// Clean interface
				var interfacePath = Path.Combine(ConfigurationContext.GetSolutionRoot(), ConfigHelper.GetUnitOfWorkInterfacePath());
				if (File.Exists(interfacePath))
				{
					File.Delete(interfacePath);
					count++;
				}

				// Clean implementation
				var implementationPath = Path.Combine(ConfigurationContext.GetSolutionRoot(), ConfigHelper.GetUnitOfWorkImplementationPath());
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

		private static int CleanMappingFiles()
		{
			int count = 0;
			try
			{
				var mappingPath = Path.Combine(ConfigurationContext.GetSolutionRoot(), ConfigHelper.GetMappingProfilePath());
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

		private static int CleanDbContextFiles()
		{
			int count = 0;
			try
			{
				// Clean DbContext
				var dbContextPath = Path.Combine(ConfigurationContext.GetSolutionRoot(), ConfigHelper.GetDbContextPath());
				if (File.Exists(dbContextPath))
				{
					File.Delete(dbContextPath);
					count++;
				}

				// Clean DbContext Factory
				var factoryPath = Path.Combine(ConfigurationContext.GetSolutionRoot(), ConfigHelper.GetDbContextFactoryPath());
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

		private static int CleanControllers()
		{
			int count = 0;
			try
			{
				var controllerPath = ConfigHelper.GetControllerPath("Sample");
				var controllersDir = Path.GetDirectoryName(controllerPath);
				
				if (!string.IsNullOrEmpty(controllersDir))
				{
					var fullPath = Path.Combine(ConfigurationContext.GetSolutionRoot(), controllersDir);
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

		private static int CleanDesktopServices()
		{
			int count = 0;
			try
			{
				var servicePath = ConfigHelper.GetDesktopServicePath("Sample");
				var servicesDir = Path.GetDirectoryName(servicePath);
				
				if (!string.IsNullOrEmpty(servicesDir))
				{
					var fullPath = Path.Combine(ConfigurationContext.GetSolutionRoot(), servicesDir);
					if (Directory.Exists(fullPath))
					{
						// Get all service files but exclude protected directories
						var protectedDirs = new[] { "Custom", "Orchestration", "Common", "Shared", "Examples" };
						var serviceFiles = Directory.GetFiles(fullPath, "*Service.cs", SearchOption.AllDirectories)
							.Where(file => !protectedDirs.Any(protectedDir => 
								file.Contains($"{Path.DirectorySeparatorChar}{protectedDir}{Path.DirectorySeparatorChar}") ||
								file.Contains($"/{protectedDir}/")))
							.ToArray();
						
						count = serviceFiles.Length;
						
						foreach (var file in serviceFiles)
						{
							File.Delete(file);
						}
						
						Console.WriteLine($"   Cleaned {count} desktop services (protected directories: {string.Join(", ", protectedDirs)})");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   [WARN] Cannot delete Desktop Services: {ex.Message}");
			}
			return count;
		}

		private static int CleanApiRequestFiles()
		{
			int count = 0;
			try
			{
				var requestPath = ConfigHelper.GetHttpRequestPath("Sample");
				var requestsDir = Path.GetDirectoryName(requestPath);
				
				if (!string.IsNullOrEmpty(requestsDir))
				{
					var fullPath = Path.Combine(ConfigurationContext.GetSolutionRoot(), requestsDir);
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

		private static int CleanSpecifications()
		{
			int count = 0;
			try
			{
				var config = ConfigurationContext.Options;
				// Get Specifications root directory
				var samplePath = string.Format(config.Shared.Files["Specification"], "Sample", "Sample");
				var specificationsPath = Path.GetDirectoryName(Path.GetDirectoryName(samplePath)); // Go up 2 levels
				
				if (!string.IsNullOrEmpty(specificationsPath))
				{
					var fullPath = Path.Combine(ConfigurationContext.GetSolutionRoot(), specificationsPath.Replace('\\', '/'));
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

		private static int CleanMigrations()
		{
			int count = 0;
			try
			{
				var config = ConfigurationContext.Options;
				var infrastructurePath = ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Infrastructure"]);
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

		private static int CleanEmptyDirectories()
		{
			int count = 0;
			try
			{
				var config = ConfigurationContext.Options;
				var projectPaths = new[]
				{
					ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Application"]),
					ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Infrastructure"]),
					ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Server"]),
					ConfigurationContext.GetSolutionRelativePath(config.Shared.ProjectNames["Desktop"])
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