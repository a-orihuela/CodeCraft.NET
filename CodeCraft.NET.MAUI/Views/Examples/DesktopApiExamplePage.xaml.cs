using Microsoft.Maui.Controls;

namespace CodeCraft.NET.MAUI.Views.Examples;

/// <summary>
/// Page demonstrating DesktopAPI integration without HTTP
/// Shows how MAUI can consume business services directly through dependency injection
/// </summary>
public partial class DesktopApiExamplePage : ContentPage
{
    public DesktopApiExamplePage()
    {
        InitializeComponent();
        
        // Set the BindingContext after the services are resolved
        Loaded += OnPageLoaded;
    }

    private void OnPageLoaded(object? sender, EventArgs e)
    {
        try
        {
            // Get the services from the application - using fully qualified namespace
            if (Microsoft.Maui.Controls.Application.Current is App app && app.Services != null)
            {
              
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error setting BindingContext: {ex.Message}");
            
            // Show error to user
            DisplayAlert("Initialization Error", 
                $"Failed to initialize page: {ex.Message}", 
                "OK");
        }
    }
}