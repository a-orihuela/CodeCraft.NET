using CodeCraft.NET.MAUI.Interfaces;

namespace CodeCraft.NET.MAUI.Services
{
    /// <summary>
    /// Implementation of INavigationService for MAUI Shell applications
    /// </summary>
    public class NavigationService : INavigationService
    {
        /// <inheritdoc/>
        public async Task NavigateToAsync(string route, object? parameter = null)
        {
            var navigationParameter = parameter != null 
                ? new Dictionary<string, object> { { "Parameter", parameter } }
                : null;
                
            await Shell.Current.GoToAsync(route, navigationParameter);
        }

        /// <inheritdoc/>
        public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        /// <inheritdoc/>
        public async Task PopToRootAsync()
        {
            await Shell.Current.GoToAsync("//");
        }
    }
}