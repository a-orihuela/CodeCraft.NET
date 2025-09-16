using Microsoft.Extensions.Logging;

namespace CodeCraft.NET.MAUI.Helpers
{
    /// <summary>
    /// UI State Manager for handling common UI states across the application
    /// </summary>
    public static class UIStateManager
    {
        private static ILogger? _logger;

        /// <summary>
        /// Initialize with logger
        /// </summary>
        public static void Initialize(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Show loading indicator with optional message
        /// </summary>
        public static async Task ShowLoadingAsync(string message = "Loading...")
        {
            _logger?.LogInformation("Showing loading: {Message}", message);
            // TODO: Implement global loading indicator
            await Task.Delay(100);
        }

        /// <summary>
        /// Hide loading indicator
        /// </summary>
        public static async Task HideLoadingAsync()
        {
            _logger?.LogInformation("Hiding loading indicator");
            // TODO: Implement global loading indicator
            await Task.Delay(100);
        }

        /// <summary>
        /// Show error message to user
        /// </summary>
        public static async Task ShowErrorAsync(string message, string title = "Error")
        {
            _logger?.LogWarning("Showing error to user: {Title} - {Message}", title, message);
            
            if (Microsoft.Maui.Controls.Application.Current?.MainPage != null)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert(title, message, "OK");
            }
        }

        /// <summary>
        /// Show success message to user
        /// </summary>
        public static async Task ShowSuccessAsync(string message, string title = "Success")
        {
            _logger?.LogInformation("Showing success to user: {Title} - {Message}", title, message);
            
            if (Microsoft.Maui.Controls.Application.Current?.MainPage != null)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert(title, message, "OK");
            }
        }

        /// <summary>
        /// Show confirmation dialog
        /// </summary>
        public static async Task<bool> ShowConfirmationAsync(string message, string title = "Confirm", string accept = "Yes", string cancel = "No")
        {
            _logger?.LogInformation("Showing confirmation dialog: {Title} - {Message}", title, message);
            
            if (Microsoft.Maui.Controls.Application.Current?.MainPage != null)
            {
                return await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
            }
            return false;
        }

        /// <summary>
        /// Show loading overlay with cancellation support
        /// </summary>
        public static async Task<T> ExecuteWithLoadingAsync<T>(Func<Task<T>> operation, string message = "Loading...")
        {
            await ShowLoadingAsync(message);
            try
            {
                return await operation();
            }
            finally
            {
                await HideLoadingAsync();
            }
        }

        /// <summary>
        /// Show loading overlay for void operations
        /// </summary>
        public static async Task ExecuteWithLoadingAsync(Func<Task> operation, string message = "Loading...")
        {
            await ShowLoadingAsync(message);
            try
            {
                await operation();
            }
            finally
            {
                await HideLoadingAsync();
            }
        }
    }
}