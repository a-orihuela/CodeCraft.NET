// Base ApplicationDbContextFactory
// This factory is used for design-time operations like migrations
// This will be updated automatically when running the generator

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CodeCraft.NET.Infrastructure.Persistence.Factories
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Default connection string for design-time operations
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CodeCraftDb;Trusted_Connection=true;MultipleActiveResultSets=true;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}