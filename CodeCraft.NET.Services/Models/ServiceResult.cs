namespace CodeCraft.NET.Services.Models
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string? ErrorMessage { get; private set; }
        public List<string> ValidationErrors { get; private set; } = new();

        private ServiceResult(bool isSuccess, T? data, string? errorMessage, List<string>? validationErrors = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
            ValidationErrors = validationErrors ?? new List<string>();
        }

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>(true, data, null);
        }

        public static ServiceResult<T> Failure(string errorMessage)
        {
            return new ServiceResult<T>(false, default, errorMessage);
        }

        public static ServiceResult<T> ValidationFailure(List<string> validationErrors)
        {
            return new ServiceResult<T>(false, default, "Validation failed", validationErrors);
        }
    }
}