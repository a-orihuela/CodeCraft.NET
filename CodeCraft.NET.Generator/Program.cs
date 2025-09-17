using CodeCraft.NET.Generator;
using CodeCraft.NET.Generator.Generators;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;
using CodeCraft.NET.Generator.Renderers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

try
{
	Console.WriteLine("CodeCraft.NET Generator Starting...");

	// Build configuration with profiles support
	var configuration = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("codecraft.config.json", optional: false, reloadOnChange: false)
		.AddEnvironmentVariables()
		.Build();

	// Setup DI container
	var services = new ServiceCollection();
	services.Configure<CodeCraftOptions>(configuration);
	services.AddSingleton<IConfiguration>(configuration);
	
	var serviceProvider = services.BuildServiceProvider();
	var options = serviceProvider.GetRequiredService<IOptions<CodeCraftOptions>>().Value;

	// Determine active profile
	var activeProfileName = GetActiveProfile(args);
	var activeProfile = options.GetActiveProfile(activeProfileName);
	
	Console.WriteLine($"Using profile: {activeProfileName}");
	Console.WriteLine($"Database Provider: {activeProfile.DatabaseProvider}");
	Console.WriteLine($"Components to generate:");
	Console.WriteLine($"  Infrastructure & CQRS: Always generated");
	Console.WriteLine($"  Services Layer: Always generated");
	Console.WriteLine($"  {(activeProfile.GenerateWebApi ? "✓" : "✗")} Web API Controllers: {activeProfile.GenerateWebApi}");
	Console.WriteLine($"  {(activeProfile.GenerateDesktopApi ? "✓" : "✗")} Desktop API Services: {activeProfile.GenerateDesktopApi}");
	Console.WriteLine($"  {(activeProfile.GenerateMaui ? "✓" : "✗")} MAUI Components: {activeProfile.GenerateMaui}");

	// Initialize the new config helper with our options
	ConfigurationContext.Initialize(options, activeProfile);

	// Show available commands
	if (args.Length > 0 && (args[0].Equals("help", StringComparison.OrdinalIgnoreCase) || args[0].Equals("--help", StringComparison.OrdinalIgnoreCase) || args[0].Equals("-h", StringComparison.OrdinalIgnoreCase)))
	{
		ShowHelp();
		return;
	}

	var rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
	var envPath = Path.Combine(rootPath, ".env");

	EnvLoader.LoadEnvFile(envPath);

	var entitiesMetadata = EntityAnalyzer.AnalyzeDomainEntities();
	if (entitiesMetadata.Count == 0)
	{
		Console.WriteLine("No entities found in Domain project.");
		Console.WriteLine("Add entities to CodeCraft.NET.Domain/Model/ and run the generator again.");
		return;
	}
	else
	{
		Console.WriteLine($"Found {entitiesMetadata.Count} entities:");
	}

	// Clean previously generated files (without touching Domain entities)
	CleanupManager.CleanGeneratedFilesOnly();

	var renderer = new ScribanTemplateRenderer();
	var cqrsGenerator = new CQRSGenerator(renderer);
	var repoGenerator = new RepositoryGenerator(renderer);
	var servicesGenerator = new ServicesGenerator(renderer);
	var controllerGenerator = new ControllerGenerator(renderer);
	var desktopApiGenerator = new DesktopApiGenerator(renderer);
	var dbContextGenerator = new DbContextGenerator(renderer);
	var infrastructureGenerator = new InfrastructureGenerator(renderer);
	var mauiGenerator = new MauiGenerator(renderer);

	Console.WriteLine("Generating code files...");

	// 1. Application
	foreach (var entity in entitiesMetadata)
	{
		cqrsGenerator.Generate(entity);
	}
	cqrsGenerator.GenerateMapping(entitiesMetadata);

	// 2. Infrastructure
	infrastructureGenerator.Generate(activeProfile.DatabaseProvider);
	dbContextGenerator.Generate(entitiesMetadata);
	repoGenerator.Generate(entitiesMetadata);

	// 3. Services Layer (always generated)
	foreach (var entity in entitiesMetadata)
	{
		servicesGenerator.Generate(entity);
	}
	servicesGenerator.GenerateServiceRegistration(entitiesMetadata);

	// 4. Web API (acts as facade over Services)
	if (activeProfile.GenerateWebApi)
	{
		foreach (var entity in entitiesMetadata)
		{
			controllerGenerator.Generate(entity);
		}
	}

	// 5. Desktop API (acts as facade over Services)
	if (activeProfile.GenerateDesktopApi)
	{
		foreach (var entity in entitiesMetadata)
		{
			desktopApiGenerator.Generate(entity);
		}
		desktopApiGenerator.GenerateServiceRegistration(entitiesMetadata);
	}

	// 6. MAUI
	if (activeProfile.GenerateMaui)
	{
		foreach (var entity in entitiesMetadata)
		{
			mauiGenerator.Generate(entity);
		}
		mauiGenerator.GenerateMAUIServiceRegistration(entitiesMetadata);
	}

	Console.WriteLine("Creating database migrations...");

	// 7. Generate migrations after DbContext is created
	if (activeProfile.ForceMigrations)
	{
		MigrationGenerator.GenerateAllMigrations();
	}
	else 
	{
		MigrationChecker.CheckPendingMigrations("ApplicationDbContext");
	}

	Console.WriteLine("Code generation completed successfully!");
	Console.WriteLine($"Generated for {activeProfile.DatabaseProvider} database provider using '{activeProfileName}' profile");
}
catch (Exception ex)
{
	Console.WriteLine($"Error during code generation: {ex.Message}");
	Console.WriteLine($"Stack trace: {ex.StackTrace}");
	Environment.Exit(1);
}

static string GetActiveProfile(string[] args)
{
	// Check for --profile argument
	for (int i = 0; i < args.Length - 1; i++)
	{
		if (args[i].Equals("--profile", StringComparison.OrdinalIgnoreCase))
		{
			return args[i + 1];
		}
	}
	
	// Check for environment variable
	var envProfile = Environment.GetEnvironmentVariable("CODECRAFT_PROFILE");
	if (!string.IsNullOrEmpty(envProfile))
	{
		return envProfile;
	}
	
	// Default profile
	return "dev";
}

static void ShowHelp()
{
	Console.WriteLine("CodeCraft.NET Generator - Usage:");
	Console.WriteLine();
	Console.WriteLine("DEFAULT BEHAVIOR:");
	Console.WriteLine("  dotnet run                       - Generate using 'dev' profile (SQLite + all components)");
	Console.WriteLine();
	Console.WriteLine("Commands:");
	Console.WriteLine("  dotnet run                       - Generate code for all entities using default profile");
	Console.WriteLine("  dotnet run -- --profile dev      - Generate using 'dev' profile");
	Console.WriteLine("  dotnet run -- --profile ci       - Generate using 'ci' profile");
	Console.WriteLine("  dotnet run help                  - Show this help message");
	Console.WriteLine();
	Console.WriteLine("Profile Selection:");
	Console.WriteLine("  --profile <name>                 - Use specific profile from codecraft.config.json");
	Console.WriteLine("  CODECRAFT_PROFILE env var        - Set default profile via environment variable");
	Console.WriteLine();
	Console.WriteLine("Available Profiles (default config):");
	Console.WriteLine("  dev        - SQLite, all components, no overwrites");
	Console.WriteLine("  ci         - In-memory SQLite, Web API only, force overwrites");
	Console.WriteLine("  production - SQLite, all components, no overwrites");
	Console.WriteLine();
	Console.WriteLine("Examples:");
	Console.WriteLine("  dotnet run                       # Use default 'dev' profile");
	Console.WriteLine("  dotnet run -- --profile ci       # Use CI profile for automated builds");
	Console.WriteLine();
	Console.WriteLine("ARCHITECTURE:");
	Console.WriteLine("  Services Layer  - Business logic and orchestration (always generated)");
	Console.WriteLine("  Web API        - HTTP facade over Services layer");
	Console.WriteLine("  Desktop API    - Direct facade over Services layer");
	Console.WriteLine();
	Console.WriteLine("CUSTOMIZATION:");
	Console.WriteLine("  Edit codecraft.config.json to:");
	Console.WriteLine("  - Add custom profiles");
	Console.WriteLine("  - Modify database providers");
	Console.WriteLine("  - Configure component generation per profile");
	Console.WriteLine("  - Set profile-specific connection strings");
	Console.WriteLine();
	Console.WriteLine("Profile Configuration Benefits:");
	Console.WriteLine("  - No hardcoded constants in Program.cs");
	Console.WriteLine("  - Environment-specific settings");
	Console.WriteLine("  - Consistent CI/CD builds");
	Console.WriteLine("  - Easy database provider switching");
	Console.WriteLine();
	Console.WriteLine("Database Provider Notes:");
	Console.WriteLine("  - SQLite: Perfect for MAUI/Desktop apps, single file database");
	Console.WriteLine("  - In-memory SQLite: Ideal for testing and CI environments");
	Console.WriteLine();
	Console.WriteLine("MAUI Generation Notes:");
	Console.WriteLine("  - ViewModels: Base files always regenerated, .Custom.cs files preserved");
	Console.WriteLine("  - Views: .Generated.xaml always regenerated, custom .xaml files preserved");
	Console.WriteLine();
	Console.WriteLine("Quick Start:");
	Console.WriteLine("  1. Add your entities to CodeCraft.NET.Domain/Model/");
	Console.WriteLine("  2. Run 'dotnet run' (uses dev profile by default)");
	Console.WriteLine("  3. For CI builds: 'dotnet run -- --profile ci'");
	Console.WriteLine("  4. Customize profiles in codecraft.config.json as needed");
}