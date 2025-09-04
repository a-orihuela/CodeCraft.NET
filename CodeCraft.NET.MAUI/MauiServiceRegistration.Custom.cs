// Custom extensions for MAUI service registration
// This file contains manual extensions and will NOT be overwritten by the generator

using Microsoft.Extensions.DependencyInjection;
using CodeCraft.NET.MAUI.Views.Examples;

namespace CodeCraft.NET.MAUI
{
    public static partial class MauiServiceRegistration
    {
        /// <summary>
        /// Registers custom MAUI services and example components
        /// </summary>
        public static IServiceCollection AddCustomMauiServices(this IServiceCollection services)
        {
            // Register DesktopAPI services for direct consumption
            //services.AddScoped<FoodItemService>();
            
            // Register example ViewModels and Pages
            services.AddTransient<DesktopApiExamplePage>();
            
            // Register demo pages
            services.AddTransient<TokenDemoPage>();
            
            // Future custom services can be added here:
            // - Custom navigation services
            // - Platform-specific integrations
            // - Custom UI components

            return services;
        }
    }
}