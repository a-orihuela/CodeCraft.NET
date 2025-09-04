// Custom partial class for InfrastructureServiceRegistration
// This file contains manual extensions and will NOT be overwritten by the generator

using CodeCraft.NET.Infrastructure.Persistence.Seeders;
using Microsoft.Extensions.DependencyInjection;

namespace CodeCraft.NET.Infrastructure
{
    public static partial class InfrastructureServiceRegistration
    {
        /// <summary>
        /// Adds custom services that extend the generated infrastructure services
        /// This method is called from the generated partial class
        /// </summary>
        static partial void AddCustomServices(IServiceCollection services)
        {
            AddCustomDomainServices(services);
            AddCustomDataSeeders(services);
        }

        /// <summary>
        /// Registers Domain Services for business logic validation
        /// </summary>
        private static void AddCustomDomainServices(IServiceCollection services)
        {

        }

        /// <summary>
        /// Registers database initialization and seeding services
        /// </summary>
        private static void AddCustomDataSeeders(IServiceCollection services)
        {
            services.AddScoped<DbInitializer>();
        }

        // Future custom services can be added here:
        // - Additional domain services
        // - Custom infrastructure services
        // - Integration services
        // - Third-party service configurations
    }
}