using CodeCraft.NET.MAUI.ViewModels;
using CodeCraft.NET.MAUI.Views.Examples;
using Microsoft.Maui.Controls;

namespace CodeCraft.NET.MAUI
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			
			// Initialize language selector if services are available
			InitializeLanguageSelector();
		}

		private void InitializeLanguageSelector()
		{
			try
			{
				// Get the services from the application - using fully qualified namespace
				if (Microsoft.Maui.Controls.Application.Current is App app && app.Services != null)
				{
					var languageSelectorViewModel = app.Services.GetService(typeof(LanguageSelectorViewModel)) as LanguageSelectorViewModel;
					if (languageSelectorViewModel != null)
					{
						LanguageSelector.BindingContext = languageSelectorViewModel;
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error initializing language selector: {ex.Message}");
			}
		}

		private async void OnViewStylesDemoClicked(object? sender, EventArgs e)
		{
			try
			{
				// Navigate to the design tokens demo page
				await Navigation.PushAsync(new TokenDemoPage());
			}
			catch (Exception ex)
			{
				// Show error if navigation fails
				await DisplayAlert("Navigation Error",
					$"Could not navigate to Design Tokens Demo: {ex.Message}",
					"OK");
			}
		}

		private async void OnViewDocumentationClicked(object? sender, EventArgs e)
		{
			// Show documentation info
			await DisplayAlert("Documentation",
				"The complete styles documentation is available in:\n\n" +
				"• Resources/Styles/README.md\n" +
				"• Check Colors.xaml for color palette\n" +
				"• Check UnifiedStyles.xaml for all component styles\n" +
				"• Check Tokens.xaml for design tokens\n" +
				"• Check TokenComponents.xaml for token-based components\n" +
				"• TokenDemoPage.xaml for complete demonstration",
				"Got it!");
		}
	}
}
