using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CodeCraft.NET.Infrastructure.Persistence.Seeders
{
    /// <summary>
    /// Interface for database seeding operations
    /// Ensures consistent and idempotent data initialization
    /// </summary>
    public interface IDataSeeder
    {
        /// <summary>
        /// Gets the seeder name for logging and identification
        /// </summary>
        string SeederName { get; }

        /// <summary>
        /// Gets the order in which this seeder should be executed
        /// Lower values are executed first
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Determines if the seeder should run based on current database state
        /// </summary>
        Task<bool> ShouldSeedAsync(DbContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the seeding operation
        /// Must be idempotent - safe to run multiple times
        /// </summary>
        Task SeedAsync(DbContext context, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Base class for all data seeders with common functionality
    /// </summary>
    public abstract class BaseSeeder : IDataSeeder
    {
        public abstract string SeederName { get; }
        public abstract int Order { get; }

        public abstract Task<bool> ShouldSeedAsync(DbContext context, CancellationToken cancellationToken = default);
        public abstract Task SeedAsync(DbContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Helper method to check if any records exist for a given entity type
        /// </summary>
        protected async Task<bool> HasAnyRecordsAsync<TEntity>(DbContext context, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            return await context.Set<TEntity>().AnyAsync(cancellationToken);
        }

        /// <summary>
        /// Helper method to get the count of records for a given entity type
        /// </summary>
        protected async Task<int> GetRecordCountAsync<TEntity>(DbContext context, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            return await context.Set<TEntity>().CountAsync(cancellationToken);
        }

        /// <summary>
        /// Helper method to safely add entity if it doesn't exist based on a condition
        /// </summary>
        protected async Task<TEntity> AddIfNotExistsAsync<TEntity>(
            DbContext context, 
            TEntity entity, 
            Expression<Func<TEntity, bool>> existsCondition,
            CancellationToken cancellationToken = default) 
            where TEntity : class
        {
            var exists = await context.Set<TEntity>().AnyAsync(existsCondition, cancellationToken);
            if (!exists)
            {
                await context.Set<TEntity>().AddAsync(entity, cancellationToken);
                return entity;
            }
            
            return await context.Set<TEntity>().FirstAsync(existsCondition, cancellationToken);
        }

        /// <summary>
        /// Helper method to log seeding operations
        /// </summary>
        protected void LogSeedingOperation(string operation, int count = 0)
        {
            var message = count > 0 
                ? $"[{SeederName}] {operation} - {count} records"
                : $"[{SeederName}] {operation}";
            Console.WriteLine(message);
        }
    }
}