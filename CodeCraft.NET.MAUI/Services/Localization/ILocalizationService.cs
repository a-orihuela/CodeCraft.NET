using System.Globalization;

namespace CodeCraft.NET.MAUI.Services.Localization
{
    /// <summary>
    /// Interface for localization services
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Gets the current culture
        /// </summary>
        CultureInfo CurrentCulture { get; }

        /// <summary>
        /// Gets a localized string by key
        /// </summary>
        /// <param name="key">The resource key</param>
        /// <returns>The localized string</returns>
        string GetString(string key);

        /// <summary>
        /// Gets a localized string by key with parameters
        /// </summary>
        /// <param name="key">The resource key</param>
        /// <param name="args">The parameters to format the string</param>
        /// <returns>The formatted localized string</returns>
        string GetString(string key, params object[] args);

        /// <summary>
        /// Changes the application language
        /// </summary>
        /// <param name="culture">The culture to set</param>
        void SetCulture(CultureInfo culture);

        /// <summary>
        /// Changes the application language by culture name
        /// </summary>
        /// <param name="cultureName">The culture name (e.g., "en-US", "es-ES")</param>
        void SetCulture(string cultureName);

        /// <summary>
        /// Gets all available cultures
        /// </summary>
        /// <returns>List of available cultures</returns>
        IEnumerable<CultureInfo> GetAvailableCultures();

        /// <summary>
        /// Checks if a culture is supported
        /// </summary>
        /// <param name="cultureName">The culture name to check</param>
        /// <returns>True if supported, false otherwise</returns>
        bool IsCultureSupported(string cultureName);

        /// <summary>
        /// Gets the display name for a culture in the current language
        /// </summary>
        /// <param name="culture">The culture to get the display name for</param>
        /// <returns>The display name</returns>
        string GetCultureDisplayName(CultureInfo culture);

        /// <summary>
        /// Event raised when the culture changes
        /// </summary>
        event EventHandler<CultureInfo> CultureChanged;
    }
}