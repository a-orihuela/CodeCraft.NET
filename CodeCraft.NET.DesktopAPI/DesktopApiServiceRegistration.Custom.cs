using Microsoft.Extensions.DependencyInjection;

namespace CodeCraft.NET.DesktopAPI
{
    public static partial class DesktopApiServiceRegistration
    {
        /// <summary>
        /// Registers advanced orchestration services for business operations.
        /// Services are located in Custom folder to prevent generator overwriting.
        /// This demonstrates the extensible architecture pattern.
        /// </summary>
        public static IServiceCollection AddOrchestrationServices(this IServiceCollection services)
        {
            // Chart services and orchestration
            
            return services;
        }
    }
}