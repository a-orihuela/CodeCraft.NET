using CodeCraft.NET.Generator;
using CodeCraft.NET.Generator.Generators;
using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Renderers;

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

// 1. Generate DbContext first (before anything else that depends on it)
dbContextGenerator.Generate(entitiesMetadata);

// 2. Generate CQRS and other components
foreach (var entity in entitiesMetadata)
{
	cqrsGenerator.Generate(entity);
	controllerGenerator.Generate(entity);
}

cqrsGenerator.GenerateMapping(entitiesMetadata);
repoGenerator.Generate(entitiesMetadata);

// 3. Generate migrations after DbContext is created
MigrationGenerator.GenerateAllMigrations();

// 4. Check for pending migrations
MigrationChecker.CheckPendingMigrations(
	ConfigHelper.GetInfrastructureRoot(),
	ConfigHelper.GetServerRoot(),
	"ApplicationDbContext");

Console.WriteLine("Code generation completed successfully!");