using CodeCraft.NET.MAUI.ViewModels;

namespace CodeCraft.NET.MAUI.Views
{
    public partial class LocalizationTestPage : ContentPage
    {
        public LocalizationTestPage()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("LocalizationTestPage constructor called");
            InitializeLanguageSelector();
        }

        private void InitializeLanguageSelector()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Starting InitializeLanguageSelector");
                
                // Get the services from the application - using fully qualified namespace
                if (Microsoft.Maui.Controls.Application.Current is App app && app.Services != null)
                {
                    System.Diagnostics.Debug.WriteLine("App and Services found");
                    
                    var languageSelectorViewModel = app.Services.GetService<LanguageSelectorViewModel>();
                    if (languageSelectorViewModel != null)
                    {
                        System.Diagnostics.Debug.WriteLine("LanguageSelectorViewModel obtained from DI");
                        
                        LanguageSelector.BindingContext = languageSelectorViewModel;
                        System.Diagnostics.Debug.WriteLine("LanguageSelector BindingContext set successfully");
                        
                        // Verify the binding context was set
                        if (LanguageSelector.BindingContext == languageSelectorViewModel)
                        {
                            System.Diagnostics.Debug.WriteLine("BindingContext verification successful");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("ERROR: BindingContext verification failed");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("ERROR: LanguageSelectorViewModel not found in DI container");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: Application.Current or Services is null");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in InitializeLanguageSelector: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            System.Diagnostics.Debug.WriteLine("LocalizationTestPage OnAppearing called");
            
            // Double-check the binding context
            if (LanguageSelector.BindingContext is LanguageSelectorViewModel vm)
            {
                System.Diagnostics.Debug.WriteLine($"BindingContext confirmed: {vm.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"Available languages count: {vm.AvailableLanguages.Count}");
                System.Diagnostics.Debug.WriteLine($"Selected language: {vm.SelectedLanguage?.FlagCode ?? "null"}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ERROR: BindingContext is not LanguageSelectorViewModel");
            }
        }
    }
}