using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CodeCraft.NET.MAUI.Services.Localization;
using CodeCraft.NET.MAUI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CodeCraft.NET.MAUI.ViewModels
{
    /// <summary>
    /// ViewModel for language/culture selection
    /// </summary>
    public partial class LanguageSelectorViewModel : LocalizedViewModel
    {
        [ObservableProperty]
        private ObservableCollection<LanguageOption> availableLanguages = new();

        [ObservableProperty]
        private LanguageOption? selectedLanguage;

        [ObservableProperty]
        private bool isLanguageSelectionVisible;

        public LanguageSelectorViewModel(ILocalizationService localizationService) : base(localizationService)
        {
            System.Diagnostics.Debug.WriteLine("LanguageSelectorViewModel constructor called");
            LoadAvailableLanguages();
            SetCurrentLanguage();
            System.Diagnostics.Debug.WriteLine($"LanguageSelectorViewModel initialized with {AvailableLanguages.Count} languages");
        }

        private void LoadAvailableLanguages()
        {
            System.Diagnostics.Debug.WriteLine("Loading available languages...");
            AvailableLanguages.Clear();
            
            var cultures = _localizationService.GetAvailableCultures();
            System.Diagnostics.Debug.WriteLine($"Found {cultures.Count()} available cultures");
            
            foreach (var culture in cultures)
            {
                var option = new LanguageOption
                {
                    Culture = culture,
                    DisplayName = GetDisplayNameForCulture(culture),
                    NativeName = culture.NativeName,
                    FlagCode = GetFlagCodeForCulture(culture)
                };
                
                System.Diagnostics.Debug.WriteLine($"Added language: {option.DisplayName} ({option.FlagCode})");
                AvailableLanguages.Add(option);
            }
        }

        private void SetCurrentLanguage()
        {
            var currentCulture = _localizationService.CurrentCulture;
            System.Diagnostics.Debug.WriteLine($"Setting current language for culture: {currentCulture.Name}");
            
            SelectedLanguage = AvailableLanguages.FirstOrDefault(l => 
                l.Culture.Name == currentCulture.Name || 
                l.Culture.TwoLetterISOLanguageName == currentCulture.TwoLetterISOLanguageName);
                
            if (SelectedLanguage != null)
            {
                System.Diagnostics.Debug.WriteLine($"Selected language set to: {SelectedLanguage.DisplayName} ({SelectedLanguage.FlagCode})");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No matching language found for current culture");
            }
        }

        [RelayCommand]
        private void ToggleLanguageSelection()
        {
            System.Diagnostics.Debug.WriteLine($"Toggling language selection. Current state: {IsLanguageSelectionVisible}");
            IsLanguageSelectionVisible = !IsLanguageSelectionVisible;
            System.Diagnostics.Debug.WriteLine($"New state: {IsLanguageSelectionVisible}");
        }

        [RelayCommand]
        private async Task SelectLanguageAsync(LanguageOption? languageOption)
        {
            if (languageOption == null || languageOption == SelectedLanguage)
            {
                IsLanguageSelectionVisible = false;
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"Changing language to: {languageOption.Culture.Name}");

                // Change the application language
                _localizationService.SetCulture(languageOption.Culture);
                
                // Update selected language
                SelectedLanguage = languageOption;
                
                // Hide the selection
                IsLanguageSelectionVisible = false;

                // Save the preference for next app start
                await SaveLanguagePreferenceAsync(languageOption.Culture.Name);
                
                System.Diagnostics.Debug.WriteLine($"Language changed successfully to: {languageOption.Culture.Name}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error changing language: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private string GetDisplayNameForCulture(CultureInfo culture)
        {
            var displayName = culture.TwoLetterISOLanguageName switch
            {
                "en" => GetLocalizedString("English"),
                "es" => GetLocalizedString("Spanish"),
                _ => culture.DisplayName
            };
            System.Diagnostics.Debug.WriteLine($"Display name for {culture.Name}: {displayName}");
            return displayName;
        }

        private string GetFlagCodeForCulture(CultureInfo culture)
        {
            var flagCode = culture.TwoLetterISOLanguageName switch
            {
                "en" => "EN",
                "es" => "ES",  
                _ => "XX"
            };
            System.Diagnostics.Debug.WriteLine($"Flag code for {culture.Name}: {flagCode}");
            return flagCode;
        }

        private async Task SaveLanguagePreferenceAsync(string cultureName)
        {
            try
            {
                // Save to preferences
                await SecureStorage.SetAsync("AppLanguage", cultureName);
                System.Diagnostics.Debug.WriteLine($"Language preference saved: {cultureName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving language preference: {ex.Message}");
            }
        }

        public static async Task<string?> GetSavedLanguagePreferenceAsync()
        {
            try
            {
                return await SecureStorage.GetAsync("AppLanguage");
            }
            catch
            {
                return null;
            }
        }

        protected override void RefreshLocalizedProperties()
        {
            // Refresh the display names when culture changes
            foreach (var language in AvailableLanguages)
            {
                language.DisplayName = GetDisplayNameForCulture(language.Culture);
            }
            
            OnPropertyChanged(nameof(AvailableLanguages));
        }

        // Properties for binding in XAML
        public string LanguageText => GetLocalizedString("Language");
        public string SettingsText => GetLocalizedString("Settings");

        protected override void OnCultureChanged(object? sender, CultureInfo newCulture)
        {
            base.OnCultureChanged(sender, newCulture);
            
            // Update selected language
            SetCurrentLanguage();
            
            // Notify UI about text changes
            OnPropertyChanged(nameof(LanguageText), nameof(SettingsText));
        }
    }

    /// <summary>
    /// Represents a language option for selection
    /// </summary>
    public partial class LanguageOption : ObservableObject
    {
        [ObservableProperty]
        private CultureInfo culture = CultureInfo.InvariantCulture;

        [ObservableProperty]
        private string displayName = string.Empty;

        [ObservableProperty]
        private string nativeName = string.Empty;

        [ObservableProperty]
        private string flagCode = "XX";

        public override string ToString()
        {
            return DisplayName;
        }
    }
}