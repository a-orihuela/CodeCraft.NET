namespace CodeCraft.NET.MAUI.Interfaces
{
    /// <summary>
    /// Provides dialog functionality for MAUI applications
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Shows an error dialog with the specified title and message
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="message">The error message</param>
        Task ShowErrorAsync(string title, string message);

        /// <summary>
        /// Shows an information dialog with the specified title and message
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="message">The information message</param>
        Task ShowInfoAsync(string title, string message);

        /// <summary>
        /// Shows a confirmation dialog with Yes/No buttons
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="message">The confirmation message</param>
        /// <returns>True if user clicked Yes, false if user clicked No</returns>
        Task<bool> ShowConfirmAsync(string title, string message);

        /// <summary>
        /// Shows a prompt dialog for user input
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="message">The prompt message</param>
        /// <param name="placeholder">Optional placeholder text</param>
        /// <returns>The user input, or null if cancelled</returns>
        Task<string?> ShowPromptAsync(string title, string message, string placeholder = "");
    }
}