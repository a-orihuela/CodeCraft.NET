using CodeCraft.NET.DesktopAPI;
using Microsoft.Extensions.Logging;

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
                fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
            });

        // TODO: Add Blazor WebView later when needed
        // builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Logging.AddDebug();
        // TODO: Add BlazorWebView developer tools later
        // builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        // Register DesktopAPI services 
        var connectionString = "Data Source=CodeCraftDb.db";
        builder.Services.AddDesktopApiServices(connectionString);

        // Register MAUI services (auto-generated + custom)
        builder.Services.AddCustomMauiServices(); // Custom services and protected components

        return builder.Build();
    }
}