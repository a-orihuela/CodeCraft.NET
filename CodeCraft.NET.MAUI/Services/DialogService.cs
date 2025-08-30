using CodeCraft.NET.MAUI.Interfaces;
using MauiApp = Microsoft.Maui.Controls.Application;

namespace CodeCraft.NET.MAUI.Services
{
    /// <summary>
    /// Implementation of IDialogService for MAUI applications
    /// </summary>
    public class DialogService : IDialogService
    {
        /// <inheritdoc/>
        public async Task ShowErrorAsync(string title, string message)
        {
            if (MauiApp.Current?.MainPage != null)
            {
                await MauiApp.Current.MainPage.DisplayAlert(title, message, "OK");
            }
        }

        /// <inheritdoc/>
        public async Task ShowInfoAsync(string title, string message)
        {
            if (MauiApp.Current?.MainPage != null)
            {
                await MauiApp.Current.MainPage.DisplayAlert(title, message, "OK");
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ShowConfirmAsync(string title, string message)
        {
            if (MauiApp.Current?.MainPage != null)
            {
                return await MauiApp.Current.MainPage.DisplayAlert(title, message, "Yes", "No");
            }
            return false;
        }

        /// <inheritdoc/>
        public async Task<string?> ShowPromptAsync(string title, string message, string placeholder = "")
        {
            if (MauiApp.Current?.MainPage != null)
            {
                return await MauiApp.Current.MainPage.DisplayPromptAsync(title, message, placeholder: placeholder);
            }
            return null;
        }
    }
}