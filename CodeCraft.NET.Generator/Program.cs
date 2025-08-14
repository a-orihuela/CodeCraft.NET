using CodeCraft.NET.Generator;
using CodeCraft.NET.Generator.Generators;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Renderers;

try
{
	Console.WriteLine("🚀 CodeCraft.NET Generator Starting...");

	// Check for command line arguments
	if (args.Length > 0 && args[0].Equals("cleanAll", StringComparison.OrdinalIgnoreCase))
	{
		Console.WriteLine("🧹 Clean mode activated - Removing all generated files...");
		CleanupManager.CleanAll();
		Console.WriteLine("✅ Template cleaned successfully!");
		Console.WriteLine("💡 You can now use the template as a clean base or add your own entities to the Domain project.");
		return;
	}

	// Check for command line arguments
	if (args.Length > 0 && args[0].Equals("clean", StringComparison.OrdinalIgnoreCase))
	{
		Console.WriteLine("🧹 Clean mode activated - Removing all generated files...");
		CleanupManager.CleanGeneratedFilesOnly();
		Console.WriteLine("✅ Template cleaned successfully!");
		Console.WriteLine("💡 You can now use the template as a clean base or add your own entities to the Domain project.");
		return;
	}

	// Show available commands
	if (args.Length > 0 && (args[0].Equals("help", StringComparison.OrdinalIgnoreCase) || args[0].Equals("--help", StringComparison.OrdinalIgnoreCase) || args[0].Equals("-h", StringComparison.OrdinalIgnoreCase)))
	{
		ShowHelp();
		return;
	}

	// Check for force migration flag
	bool forceMigration = args.Contains("--force-migration") || args.Contains("-f");
	if (forceMigration)
	{
		Console.WriteLine("🔧 Force migration mode activated");
	}

	var rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
	var envPath = Path.Combine(rootPath, ".env");

	EnvLoader.LoadEnvFile(envPath);

	var entitiesMetadata = EntityAnalyzer.AnalyzeDomainEntities();
	
	Console.WriteLine($"📊 Found {entitiesMetadata.Count} entities:");
	foreach (var entity in entitiesMetadata)
	{
		Console.WriteLine($"   - {entity.Name} ({entity.Properties.Count} properties)");
	}

	if (entitiesMetadata.Count == 0)
	{
		Console.WriteLine("⚠️  No entities found in Domain project.");
		Console.WriteLine("💡 Add entities to CodeCraft.NET.Domain/Model/ and run the generator again.");
		Console.WriteLine("💡 Or run 'dotnet run clean' to clean all generated files.");
		return;
	}

	// Clean previously generated files (without touching Domain entities)
	CleanupManager.CleanGeneratedFilesOnly();

	var renderer = new ScribanTemplateRenderer();
	var cqrsGenerator = new CQRSGenerator(renderer);
	var repoGenerator = new RepositoryGenerator(renderer);
	var controllerGenerator = new ControllerGenerator(renderer);
	var dbContextGenerator = new DbContextGenerator(renderer);

	Console.WriteLine("📝 Generating code files...");

	// 1. Generate DbContext first (before anything else that depends on it)
	dbContextGenerator.Generate(entitiesMetadata);

	// 2. Generate CQRS and other components
	foreach (var entity in entitiesMetadata)
	{
		Console.WriteLine($"   📝 Generating files for {entity.Name}...");
		cqrsGenerator.Generate(entity);
		controllerGenerator.Generate(entity);
	}

	cqrsGenerator.GenerateMapping(entitiesMetadata);
	repoGenerator.Generate(entitiesMetadata);

	Console.WriteLine("🗄️ Creating database migrations...");

	// 3. Generate migrations after DbContext is created
	if (forceMigration)
	{
		Console.WriteLine("   🔧 Forcing migration creation...");
		MigrationGenerator.ForceGenerateMigration();
	}
	else
	{
		MigrationGenerator.GenerateAllMigrations();
	}

	if (!forceMigration)
	{
		// 4. Check for pending migrations
		MigrationChecker.CheckPendingMigrations("ApplicationDbContext");
	}

	Console.WriteLine("✅ Code generation completed successfully!");
}
catch (Exception ex)
{
	Console.WriteLine($"❌ Error during code generation: {ex.Message}");
	Console.WriteLine($"Stack trace: {ex.StackTrace}");
	Environment.Exit(1);
}

static void ShowHelp()
{
	Console.WriteLine("🔧 CodeCraft.NET Generator - Usage:");
	Console.WriteLine();
	Console.WriteLine("Commands:");
	Console.WriteLine("  dotnet run                       - Generate code for all entities in Domain project");
	Console.WriteLine("  dotnet run clean                 - Clean generated files (keep Domain entities)");
	Console.WriteLine("  dotnet run cleanAll              - Clean all generated files and example entities");
	Console.WriteLine("  dotnet run help                  - Show this help message");
	Console.WriteLine();
	Console.WriteLine("Options:");
	Console.WriteLine("  --force-migration, -f            - Force migration creation even if no changes detected");
	Console.WriteLine();
	Console.WriteLine("Examples:");
	Console.WriteLine("  dotnet run                       # Generate CRUD for all entities");
	Console.WriteLine("  dotnet run --force-migration     # Generate with forced migration creation");
	Console.WriteLine("  dotnet run clean                 # Remove generated files, keep your entities");
	Console.WriteLine("  dotnet run cleanAll              # Complete reset, remove everything");
	Console.WriteLine();
	Console.WriteLine("💡 Migration Behavior:");
	Console.WriteLine("  - By default, migrations are created only when model changes are detected");
	Console.WriteLine("  - Use --force-migration to always create a migration");
	Console.WriteLine("  - Recent migrations (within 2 minutes) are automatically skipped");
	Console.WriteLine();
	Console.WriteLine("💡 Quick Start:");
	Console.WriteLine("  1. Add your entities to CodeCraft.NET.Domain/Model/");
	Console.WriteLine("  2. Run 'dotnet run' to generate all CRUD operations");
	Console.WriteLine("  3. Run 'dotnet run clean' to clean generated files only");
	Console.WriteLine("  4. Run 'dotnet run cleanAll' to reset template completely");
}