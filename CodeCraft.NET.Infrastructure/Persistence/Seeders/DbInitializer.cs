using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CodeCraft.NET.Infrastructure.Persistence.Seeders
{
    /// <summary>
    /// Database initializer that handles migrations and seeding
    /// Provides idempotent initialization for any environment
    /// </summary>
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DbInitializer> _logger;
        private readonly List<IDataSeeder> _seeders;

        public DbInitializer(ApplicationDbContext context, ILogger<DbInitializer> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            // Register all seeders in order
            _seeders = new List<IDataSeeder>
            {
            };
        }

        /// <summary>
        /// Initializes database with migrations and seeding
        /// Safe to call multiple times - fully idempotent
        /// </summary>
        /// <param name="migrateDatabase">Whether to apply pending migrations</param>
        /// <param name="seedData">Whether to seed initial data</param>
        public async Task InitializeAsync(bool migrateDatabase = true, bool seedData = true, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Starting database initialization");

                if (migrateDatabase)
                {
                    await ApplyMigrationsAsync(cancellationToken);
                }

                if (seedData)
                {
                    await SeedDataAsync(cancellationToken);
                }

                _logger.LogInformation("Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during database initialization");
                throw;
            }
        }

        /// <summary>
        /// Applies all pending migrations to the database
        /// </summary>
        private async Task ApplyMigrationsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking for pending migrations");
            
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync(cancellationToken);
            var pendingCount = pendingMigrations.Count();

            if (pendingCount > 0)
            {
                _logger.LogInformation("Applying {Count} pending migrations", pendingCount);
                foreach (var migration in pendingMigrations)
                {
                    _logger.LogDebug("Pending migration: {Migration}", migration);
                }
                
                await _context.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Migrations applied successfully");
            }
            else
            {
                _logger.LogInformation("Database is up to date - no pending migrations");
            }
        }

        /// <summary>
        /// Executes all registered seeders in order
        /// </summary>
        private async Task SeedDataAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting data seeding process");

            var executedSeeders = 0;
            var skippedSeeders = 0;

            // Execute seeders in order
            var orderedSeeders = _seeders.OrderBy(s => s.Order).ToList();
            
            foreach (var seeder in orderedSeeders)
            {
                try
                {
                    _logger.LogDebug("Checking seeder: {SeederName} (Order: {Order})", seeder.SeederName, seeder.Order);
                    
                    var shouldSeed = await seeder.ShouldSeedAsync(_context, cancellationToken);
                    
                    if (shouldSeed)
                    {
                        _logger.LogInformation("Executing seeder: {SeederName}", seeder.SeederName);
                        await seeder.SeedAsync(_context, cancellationToken);
                        executedSeeders++;
                        _logger.LogInformation("Seeder completed: {SeederName}", seeder.SeederName);
                    }
                    else
                    {
                        _logger.LogDebug("Skipping seeder (conditions not met): {SeederName}", seeder.SeederName);
                        skippedSeeders++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing seeder: {SeederName}", seeder.SeederName);
                    throw;
                }
            }

            _logger.LogInformation("Data seeding completed. Executed: {Executed}, Skipped: {Skipped}", 
                executedSeeders, skippedSeeders);
        }

        /// <summary>
        /// Gets detailed information about database and seeding state
        /// Useful for diagnostics and health checks
        /// </summary>
        public async Task<DatabaseStatus> GetStatusAsync(CancellationToken cancellationToken = default)
        {
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
            var pendingMigrations = canConnect ? await _context.Database.GetPendingMigrationsAsync(cancellationToken) : Enumerable.Empty<string>();
            var appliedMigrations = canConnect ? await _context.Database.GetAppliedMigrationsAsync(cancellationToken) : Enumerable.Empty<string>();

            var status = new DatabaseStatus
            {
                CanConnect = canConnect,
                HasPendingMigrations = pendingMigrations.Any(),
                PendingMigrations = pendingMigrations.ToList(),
                AppliedMigrations = appliedMigrations.ToList(),
                SeederStatuses = new List<SeederStatus>()
            };

            if (canConnect)
            {
                foreach (var seeder in _seeders.OrderBy(s => s.Order))
                {
                    try
                    {
                        var shouldSeed = await seeder.ShouldSeedAsync(_context, cancellationToken);
                        status.SeederStatuses.Add(new SeederStatus
                        {
                            Name = seeder.SeederName,
                            Order = seeder.Order,
                            ShouldSeed = shouldSeed,
                            Error = null
                        });
                    }
                    catch (Exception ex)
                    {
                        status.SeederStatuses.Add(new SeederStatus
                        {
                            Name = seeder.SeederName,
                            Order = seeder.Order,
                            ShouldSeed = false,
                            Error = ex.Message
                        });
                    }
                }
            }

            return status;
        }
    }

    /// <summary>
    /// Represents the current status of the database and seeding operations
    /// </summary>
    public class DatabaseStatus
    {
        public bool CanConnect { get; set; }
        public bool HasPendingMigrations { get; set; }
        public List<string> PendingMigrations { get; set; } = new();
        public List<string> AppliedMigrations { get; set; } = new();
        public List<SeederStatus> SeederStatuses { get; set; } = new();

        public bool IsFullyInitialized => CanConnect && !HasPendingMigrations && SeederStatuses.All(s => !s.ShouldSeed && s.Error == null);
    }

    /// <summary>
    /// Represents the status of an individual seeder
    /// </summary>
    public class SeederStatus
    {
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public bool ShouldSeed { get; set; }
        public string? Error { get; set; }
    }
}