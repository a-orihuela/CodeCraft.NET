using CodeCraft.NET.MAUI.Services.Localization;

namespace CodeCraft.NET.MAUI.Extensions.Localization
{
    /// <summary>
    /// Markup extension for localizing strings in XAML
    /// Usage: {local:Localize Key=ResourceKey}
    /// </summary>
    [ContentProperty(nameof(Key))]
    public class LocalizeExtension : IMarkupExtension<string>
    {
        public string Key { get; set; } = string.Empty;

        public string ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(Key))
                return Key;

            // Get the localization service from DI
            var localizationService = GetLocalizationService();
            return localizationService?.GetString(Key) ?? Key;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        private static ILocalizationService? GetLocalizationService()
        {
            try
            {
                // Try to get the service from the current application
                if (Microsoft.Maui.Controls.Application.Current is App app && app.Services != null)
                {
                    return app.Services.GetService<ILocalizationService>();
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Alternative markup extension that can be used with parameters
    /// Usage: {local:LocalizeFormat Key=ResourceKey, Arg1=Value1, Arg2=Value2}
    /// </summary>
    [ContentProperty(nameof(Key))]
    public class LocalizeFormatExtension : IMarkupExtension<string>
    {
        public string Key { get; set; } = string.Empty;
        public object? Arg1 { get; set; }
        public object? Arg2 { get; set; }
        public object? Arg3 { get; set; }
        public object? Arg4 { get; set; }
        public object? Arg5 { get; set; }

        public string ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(Key))
                return Key;

            var localizationService = GetLocalizationService();
            if (localizationService == null)
                return Key;

            // Collect non-null arguments
            var args = new List<object?>();
            if (Arg1 != null) args.Add(Arg1);
            if (Arg2 != null) args.Add(Arg2);
            if (Arg3 != null) args.Add(Arg3);
            if (Arg4 != null) args.Add(Arg4);
            if (Arg5 != null) args.Add(Arg5);

            return args.Count > 0 
                ? localizationService.GetString(Key, args.ToArray()) 
                : localizationService.GetString(Key);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        private static ILocalizationService? GetLocalizationService()
        {
            try
            {
                if (Microsoft.Maui.Controls.Application.Current is App app && app.Services != null)
                {
                    return app.Services.GetService<ILocalizationService>();
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}