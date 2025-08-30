namespace CodeCraft.NET.MAUI
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private async void OnViewStylesDemoClicked(object? sender, EventArgs e)
		{
			try
			{
				// Navigate to the styles demo page
				await Shell.Current.GoToAsync("StyleDemoPage");
			}
			catch
			{
				// Fallback: show alert if navigation fails
				await DisplayAlert("Navigation",
					"Styles Demo page will be shown here. " +
					"Navigation system is being set up.",
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
				"• Check ButtonStyles.xaml for button variations\n" +
				"• Check ComponentStyles.xaml for cards and layouts",
				"Got it!");
		}
	}
}
