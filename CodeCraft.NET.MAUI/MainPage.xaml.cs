using Microsoft.Extensions.Logging;

namespace CodeCraft.NET.MAUI;

public partial class MainPage : ContentPage
{
    private readonly ILogger<MainPage> _logger;
    int count = 0;

    public MainPage(ILogger<MainPage> logger)
    {
        _logger = logger;
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (sender is Button button)
        {
            if (count == 1)
                button.Text = $"Clicked {count} time";
            else
                button.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(button.Text);
        }
        
        _logger.LogInformation("Counter button clicked {Count} times", count);
    }

    private async void OnGoToDetailsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//details");
    }
}
