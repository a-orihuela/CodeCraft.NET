using CommunityToolkit.Mvvm.ComponentModel;
using CodeCraft.NET.MAUI.Services.Localization;
using System.Globalization;

namespace CodeCraft.NET.MAUI.ViewModels.Base
{
    /// <summary>
    /// Base class for ViewModels that need localization support
    /// </summary>
    public partial class LocalizedViewModel : ObservableObject
    {
        protected readonly ILocalizationService _localizationService;

        public LocalizedViewModel(ILocalizationService localizationService)
        {
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _localizationService.CultureChanged += OnCultureChanged;
        }

        /// <summary>
        /// Gets a localized string by key
        /// </summary>
        /// <param name="key">The resource key</param>
        /// <returns>The localized string</returns>
        protected string GetLocalizedString(string key)
        {
            return _localizationService.GetString(key);
        }

        /// <summary>
        /// Gets a localized string by key with parameters
        /// </summary>
        /// <param name="key">The resource key</param>
        /// <param name="args">The parameters to format the string</param>
        /// <returns>The formatted localized string</returns>
        protected string GetLocalizedString(string key, params object[] args)
        {
            return _localizationService.GetString(key, args);
        }

        /// <summary>
        /// Gets the current culture
        /// </summary>
        public CultureInfo CurrentCulture => _localizationService.CurrentCulture;

        /// <summary>
        /// Called when the culture changes. Override to refresh localized properties.
        /// </summary>
        protected virtual void OnCultureChanged(object? sender, CultureInfo newCulture)
        {
            OnPropertyChanged(nameof(CurrentCulture));
            // Derived classes should override this to refresh their localized properties
            RefreshLocalizedProperties();
        }

        /// <summary>
        /// Override this method to refresh localized properties when culture changes
        /// </summary>
        protected virtual void RefreshLocalizedProperties()
        {
            // Base implementation does nothing
            // Derived classes should override to notify property changes for localized properties
        }

        /// <summary>
        /// Helper method to notify property changed for multiple properties
        /// </summary>
        /// <param name="propertyNames">The property names to notify</param>
        protected void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                OnPropertyChanged(propertyName);
            }
        }

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }
    }
}