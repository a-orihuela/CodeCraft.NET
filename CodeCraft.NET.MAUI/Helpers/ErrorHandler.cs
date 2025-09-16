using Microsoft.Extensions.Logging;

namespace CodeCraft.NET.MAUI.Helpers
{
    /// <summary>
    /// Error handler for managing exceptions and user feedback
    /// </summary>
    public static class ErrorHandler
    {
        private static ILogger? _logger;

        /// <summary>
        /// Initialize error handler with logger
        /// </summary>
        public static void Initialize(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handle exception with user-friendly message
        /// </summary>
        public static async Task HandleExceptionAsync(Exception exception, string userMessage = "An unexpected error occurred")
        {
            _logger?.LogError(exception, "Unhandled exception: {Message}", exception.Message);
            
            await UIStateManager.ShowErrorAsync(userMessage);
        }

        /// <summary>
        /// Handle service errors with appropriate user feedback
        /// </summary>
        public static async Task HandleServiceErrorAsync(string errorMessage, string? userMessage = null)
        {
            _logger?.LogWarning("Service error: {ErrorMessage}", errorMessage);
            
            var displayMessage = userMessage ?? GetUserFriendlyMessage(errorMessage);
            await UIStateManager.ShowErrorAsync(displayMessage);
        }

        /// <summary>
        /// Handle and log error, returning user-friendly message without showing dialog
        /// </summary>
        public static string HandleAndGetMessage(Exception exception, string fallbackMessage = "An error occurred")
        {
            _logger?.LogError(exception, "Error handled: {Message}", exception.Message);
            return GetUserFriendlyMessage(exception.Message) ?? fallbackMessage;
        }

        /// <summary>
        /// Convert technical error messages to user-friendly ones
        /// </summary>
        public static string GetUserFriendlyMessage(string technicalMessage)
        {
            return technicalMessage.ToLowerInvariant() switch
            {
                var msg when msg.Contains("network") => "Please check your internet connection and try again.",
                var msg when msg.Contains("timeout") => "The operation took too long. Please try again.",
                var msg when msg.Contains("not found") => "The requested item could not be found.",
                var msg when msg.Contains("unauthorized") => "You don't have permission to perform this action.",
                var msg when msg.Contains("validation") => "Please check your input and try again.",
                var msg when msg.Contains("connection") => "Unable to connect to the server. Please try again later.",
                var msg when msg.Contains("server") => "Server error occurred. Please try again later.",
                var msg when msg.Contains("duplicate") => "This item already exists.",
                var msg when msg.Contains("required") => "Please fill in all required fields.",
                _ => "An unexpected error occurred. Please try again later."
            };
        }

        /// <summary>
        /// Execute operation with automatic error handling
        /// </summary>
        public static async Task<bool> TryExecuteAsync(Func<Task> operation, string? errorMessage = null)
        {
            try
            {
                await operation();
                return true;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, errorMessage ?? "Operation failed");
                return false;
            }
        }

        /// <summary>
        /// Execute operation with automatic error handling and return result
        /// </summary>
        public static async Task<(bool Success, T? Result)> TryExecuteAsync<T>(Func<Task<T>> operation, string? errorMessage = null)
        {
            try
            {
                var result = await operation();
                return (true, result);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, errorMessage ?? "Operation failed");
                return (false, default(T));
            }
        }
    }
}