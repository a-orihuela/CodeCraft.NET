namespace CodeCraft.NET.MAUI.Interfaces
{
    /// <summary>
    /// Provides navigation functionality for MAUI Shell applications
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Navigates to the specified route
        /// </summary>
        /// <param name="route">The route to navigate to</param>
        /// <param name="parameter">Optional navigation parameter</param>
        Task NavigateToAsync(string route, object? parameter = null);

        /// <summary>
        /// Navigates back to the previous page
        /// </summary>
        Task GoBackAsync();

        /// <summary>
        /// Navigates to the root page
        /// </summary>
        Task PopToRootAsync();
    }
}