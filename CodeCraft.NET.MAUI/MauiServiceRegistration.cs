using Microsoft.Extensions.Logging;
using CodeCraft.NET.MAUI.Views.Layout;
using CodeCraft.NET.MAUI.Helpers;

namespace CodeCraft.NET.MAUI
{
    public static class MauiServiceRegistration
    {
        /// <summary>
        /// Registers custom MAUI services and example components
        /// </summary>
        public static IServiceCollection AddCustomMauiServices(this IServiceCollection services)
        {
            // Initialize static helpers
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("UIStateManager");
            UIStateManager.Initialize(logger);
            ErrorHandler.Initialize(logger);
            
            // Layout components
            services.AddTransient<MainLayoutPage>();
            services.AddTransient<SidebarMenuView>();
            services.AddTransient<AppHeaderView>();

            return services;
        }
    }
}