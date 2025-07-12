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
var CQRSgenerator = new CQRSGenerator(renderer);
var repoGenerator = new RepositoryGenerator(renderer);
var controllerGenerator = new ControllerGenerator(renderer);
var dbContextGenerator = new DbContextGenerator(renderer);

foreach (var entity in entitiesMetadata)
{
	CQRSgenerator.Generate(entity);
	controllerGenerator.Generate(entity);
}
CQRSgenerator.GenerateMapping(entitiesMetadata);
repoGenerator.Generate(entitiesMetadata);

// 1. Generate DbContext first (before anything else that depends on it)
dbContextGenerator.Generate(entitiesMetadata);

// 2. Generate migrations after DbContext is created
MigrationGenerator.GenerateAllMigrations();
MigrationChecker.CheckPendingMigrations(
	PathHelper.InfrastructureRoot,
	PathHelper.ServerRoot,
	"UnifiedDbContext");
