namespace CodeCraft.NET.DesktopAPI.Models
{
    /// <summary>
    /// Service result wrapper for consistent error handling across DesktopAPI services
    /// Provides a standardized way to handle success and failure scenarios
    /// </summary>
    /// <typeparam name="T">Type of the result value</typeparam>
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T Value { get; private set; } = default!;
        public string ErrorMessage { get; private set; } = string.Empty;
        public Exception? Exception { get; private set; }

        private ServiceResult() { }

        /// <summary>
        /// Creates a successful result with the provided value
        /// </summary>
        /// <param name="value">The successful result value</param>
        /// <returns>ServiceResult indicating success</returns>
        public static ServiceResult<T> Success(T value)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Value = value,
                ErrorMessage = string.Empty
            };
        }

        /// <summary>
        /// Creates a failure result with error message and optional exception
        /// </summary>
        /// <param name="errorMessage">Description of the error</param>
        /// <param name="exception">Optional exception that caused the failure</param>
        /// <returns>ServiceResult indicating failure</returns>
        public static ServiceResult<T> Failure(string errorMessage, Exception? exception = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Value = default!,
                ErrorMessage = errorMessage,
                Exception = exception
            };
        }

        /// <summary>
        /// Implicit conversion from T to ServiceResult<T> for convenience
        /// </summary>
        /// <param name="value">Value to wrap in success result</param>
        public static implicit operator ServiceResult<T>(T value)
        {
            return Success(value);
        }
    }
}