// Custom extensions for DesktopAPI service registration
// This file contains manual extensions and will NOT be overwritten by the generator

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
            return services;
        }
    }
}