namespace CodeCraft.NET.MAUI.Services.Generated
{
    /// <summary>
    /// MAUI-specific service result with UI-friendly messages
    /// </summary>
    public class MauiServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T Value { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public string UIMessage { get; private set; } = string.Empty;
        public Exception? Exception { get; private set; }

        private MauiServiceResult() { }

        public static MauiServiceResult<T> Success(T value, string uiMessage = "")
        {
            return new MauiServiceResult<T>
            {
                IsSuccess = true,
                Value = value,
                UIMessage = uiMessage
            };
        }

        public static MauiServiceResult<T> Failure(string errorMessage, string uiMessage = "", Exception? exception = null)
        {
            return new MauiServiceResult<T>
            {
                IsSuccess = false,
                Value = default(T)!,
                ErrorMessage = errorMessage,
                UIMessage = string.IsNullOrEmpty(uiMessage) ? errorMessage : uiMessage,
                Exception = exception
            };
        }
    }
}