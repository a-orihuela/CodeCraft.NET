namespace CodeCraft.NET.MAUI.Validation.Generated
{
    /// <summary>
    /// Validation result for UI feedback
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> ErrorMessages { get; set; } = new();
        public string ErrorSummary => string.Join(Environment.NewLine, ErrorMessages);
        public string FirstError => ErrorMessages.FirstOrDefault() ?? string.Empty;
    }
}