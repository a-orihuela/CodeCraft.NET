using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CodeCraft.NET.Infrastructure;
using CodeCraft.NET.DesktopAPI;
using CodeCraft.NET.MAUI.Services.Localization;
using System.Globalization;

namespace CodeCraft.NET.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Add logging
		builder.Services.AddLogging(configure => configure.AddDebug());

		// Add localization services
		builder.Services.AddLocalization();
		builder.Services.AddSingleton<ILocalizationService, LocalizationService>();

		// Database configuration for MAUI (SQLite with proper location)
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "CodeCraftDb.db");
		var connectionString = $"Data Source={dbPath}";

		// Log database location for debugging
#if DEBUG
		System.Diagnostics.Debug.WriteLine($"SQLite Database Location: {dbPath}");
#endif

		// Create configuration
		var configuration = new ConfigurationBuilder().Build();

		// Add Infrastructure services (for data access) - SQLite only
		builder.Services.AddInfrastructureServices(configuration, connectionString);

		// Add MAUI-specific services
		builder.Services.AddMauiServices();
		
		// Add custom MAUI services (examples and extensions)
		builder.Services.AddCustomMauiServices();

		// Register App with services
		builder.Services.AddSingleton<App>();

		var app = builder.Build();

		// Set the services in the App instance
		var appInstance = app.Services.GetRequiredService<App>();
		if (appInstance is App typedApp)
		{
			typedApp.GetType().GetProperty("Services")?.SetValue(typedApp, app.Services);
		}

		// Initialize localization with saved preference
		_ = Task.Run(async () => await InitializeLocalizationAsync(app.Services));

		// Register Shell routes
		ShellRouting.RegisterRoutes();

		return app;
	}

	private static async Task InitializeLocalizationAsync(IServiceProvider services)
	{
		try
		{
			var localizationService = services.GetService<ILocalizationService>();
			if (localizationService != null)
			{
				System.Diagnostics.Debug.WriteLine("LocalizationService found in DI container");
				
				// Try to load saved language preference
				var savedLanguage = await CodeCraft.NET.MAUI.ViewModels.LanguageSelectorViewModel.GetSavedLanguagePreferenceAsync();
				
				if (!string.IsNullOrEmpty(savedLanguage) && localizationService.IsCultureSupported(savedLanguage))
				{
					System.Diagnostics.Debug.WriteLine($"Setting saved language: {savedLanguage}");
					localizationService.SetCulture(savedLanguage);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"No valid saved language found, using default culture: {localizationService.CurrentCulture.Name}");
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("LocalizationService NOT found in DI container");
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error initializing localization: {ex.Message}");
			System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
		}
	}
}