// Base infrastructure service registration
// This file is the foundation for dependency injection setup
// Auto-generated files will extend this configuration

using CodeCraft.NET.Application.Contracts.Persistence.Base;
using CodeCraft.NET.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeCraft.NET.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, string appConnection)
        {
            AddDbContext(services, configuration, appConnection);
            AddRepositories(services);

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            // Register generic repository adapter
            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepositoryAdapter<>));
            
            // Note: Specific repositories and UnitOfWork will be registered automatically
            // after running the code generator (dotnet run --project YourProject.Generator)
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration, string appConnection)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    appConnection,
                    x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                )
            );
        }
    }
}