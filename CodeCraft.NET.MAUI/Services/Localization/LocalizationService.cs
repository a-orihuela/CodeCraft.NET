using System.Globalization;

namespace CodeCraft.NET.MAUI.Services.Localization
{
    /// <summary>
    /// Implementation of localization service for MAUI applications
    /// </summary>
    public class LocalizationService : ILocalizationService
    {
        private CultureInfo _currentCulture;
        private readonly List<CultureInfo> _availableCultures;

        public event EventHandler<CultureInfo>? CultureChanged;

        public LocalizationService()
        {
            // Initialize available cultures
            _availableCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"), // English (United States)
                new CultureInfo("es-ES")  // Spanish (Spain)
            };

            // Set default culture
            _currentCulture = CultureInfo.CurrentCulture;
            
            // If current culture is not supported, default to English
            if (!_availableCultures.Any(c => c.TwoLetterISOLanguageName == _currentCulture.TwoLetterISOLanguageName))
            {
                _currentCulture = _availableCultures.First(); // Default to English
            }
            else
            {
                // Find the exact match or closest match
                _currentCulture = _availableCultures.FirstOrDefault(c => 
                    c.Name == _currentCulture.Name) ?? 
                    _availableCultures.FirstOrDefault(c => 
                    c.TwoLetterISOLanguageName == _currentCulture.TwoLetterISOLanguageName) ?? 
                    _availableCultures.First();
            }

            // Apply the culture
            ApplyCulture(_currentCulture);
        }

        public CultureInfo CurrentCulture => _currentCulture;

        public string GetString(string key)
        {
            try
            {
                // Simple fallback implementation - return the key or a basic translation
                return GetBasicTranslation(key);
            }
            catch
            {
                return key; // Return key if any error occurs
            }
        }

        public string GetString(string key, params object[] args)
        {
            try
            {
                var format = GetString(key);
                return string.Format(format, args);
            }
            catch
            {
                return key; // Return key if any error occurs
            }
        }

        private string GetBasicTranslation(string key)
        {
            // Basic Spanish translations without special characters
            if (_currentCulture.TwoLetterISOLanguageName == "es")
            {
                return key switch
                {
                    "Save" => "Guardar",
                    "Cancel" => "Cancelar", 
                    "Delete" => "Eliminar",
                    "Edit" => "Editar",
                    "Settings" => "Configuracion",
                    "Language" => "Idioma",
                    "Dashboard" => "Panel Principal",
                    "Monday" => "Lunes",
                    "Tuesday" => "Martes",
                    "Wednesday" => "Miercoles",
                    "Thursday" => "Jueves",
                    "Friday" => "Viernes",
                    "Saturday" => "Sabado",
                    "Sunday" => "Domingo",
                    "English" => "English",
                    "Spanish" => "Espanol",
                    _ => key
                };
            }
            
            // Default English
            return key;
        }

        public void SetCulture(CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            // Check if culture is supported
            var supportedCulture = _availableCultures.FirstOrDefault(c => 
                c.Name == culture.Name) ?? 
                _availableCultures.FirstOrDefault(c => 
                c.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName);

            if (supportedCulture == null)
            {
                throw new NotSupportedException($"Culture '{culture.Name}' is not supported. Available cultures: {string.Join(", ", _availableCultures.Select(c => c.Name))}");
            }

            _currentCulture = supportedCulture;
            ApplyCulture(_currentCulture);
            
            // Raise culture changed event
            CultureChanged?.Invoke(this, _currentCulture);
        }

        public void SetCulture(string cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
                throw new ArgumentException("Culture name cannot be null or empty", nameof(cultureName));

            try
            {
                var culture = new CultureInfo(cultureName);
                SetCulture(culture);
            }
            catch (CultureNotFoundException ex)
            {
                throw new ArgumentException($"Invalid culture name: {cultureName}", nameof(cultureName), ex);
            }
        }

        public IEnumerable<CultureInfo> GetAvailableCultures()
        {
            return _availableCultures.AsReadOnly();
        }

        /// <summary>
        /// Checks if a culture is supported
        /// </summary>
        /// <param name="cultureName">The culture name to check</param>
        /// <returns>True if supported, false otherwise</returns>
        public bool IsCultureSupported(string cultureName)
        {
            try
            {
                var culture = new CultureInfo(cultureName);
                return _availableCultures.Any(c => 
                    c.Name == culture.Name || 
                    c.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Applies the culture to the current thread and app resources
        /// </summary>
        /// <param name="culture">The culture to apply</param>
        private void ApplyCulture(CultureInfo culture)
        {
            // Set thread culture for formatting (dates, numbers, etc.)
            Thread.CurrentThread.CurrentCulture = culture;
            
            // Set UI culture for resource loading
            Thread.CurrentThread.CurrentUICulture = culture;
            
            // Set culture for the current culture info
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        /// <summary>
        /// Gets the display name for a culture in the current language
        /// </summary>
        /// <param name="culture">The culture to get the display name for</param>
        /// <returns>The display name</returns>
        public string GetCultureDisplayName(CultureInfo culture)
        {
            return culture.TwoLetterISOLanguageName switch
            {
                "en" => GetString("English"),
                "es" => GetString("Spanish"),
                _ => culture.DisplayName
            };
        }
    }
}