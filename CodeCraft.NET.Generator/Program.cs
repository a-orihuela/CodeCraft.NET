using CodeCraft.NET.Generator;
using CodeCraft.NET.Generator.Generators;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Renderers;
using CodeCraft.NET.Cross.Services; // Agregar esta referencia

try
{
	Console.WriteLine("🚀 CodeCraft.NET Generator Starting...");

	// 1. Ensure Docker services are running
	await DockerManager.EnsureDatabaseIsRunningAsync();

	var rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
	var envPath = Path.Combine(rootPath, ".env");

	EnvLoader.LoadEnvFile(envPath);

	var entitiesMetadata = EntityAnalyzer.AnalyzeDomainEntities();

	PrepareEscenario.DeleteOldFiles();

	var renderer = new ScribanTemplateRenderer();
	var cqrsGenerator = new CQRSGenerator(renderer);
	var repoGenerator = new RepositoryGenerator(renderer);
	var controllerGenerator = new ControllerGenerator(renderer);
	var dbContextGenerator = new DbContextGenerator(renderer);

	Console.WriteLine("📝 Generating code files...");

	// 2. Generate DbContext first (before anything else that depends on it)
	dbContextGenerator.Generate(entitiesMetadata);

	// 3. Generate CQRS and other components
	foreach (var entity in entitiesMetadata)
	{
		cqrsGenerator.Generate(entity);
		controllerGenerator.Generate(entity);
	}

	cqrsGenerator.GenerateMapping(entitiesMetadata);
	repoGenerator.Generate(entitiesMetadata);

	Console.WriteLine("🗄️ Creating database migrations...");

	// 4. Generate migrations after DbContext is created
	MigrationGenerator.GenerateAllMigrations();

	// 5. Check for pending migrations
	MigrationChecker.CheckPendingMigrations("ApplicationDbContext");

	Console.WriteLine("✅ Code generation completed successfully!");
}
catch (Exception ex)
{
	Console.WriteLine($"❌ Error during code generation: {ex.Message}");
	Environment.Exit(1);
}