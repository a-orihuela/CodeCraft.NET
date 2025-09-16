using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace CodeCraft.NET.MAUI.ViewModels.Base
{
    /// <summary>
    /// Base ViewModel for CRUD operations
    /// Provides common functionality for Create, Read, Update, Delete scenarios
    /// </summary>
    public abstract partial class BaseCrudViewModel<TDto, TCreate, TUpdate> : ObservableObject
        where TDto : class
        where TCreate : class
        where TUpdate : class
    {
        protected readonly ILogger _logger;

        #region Observable Properties

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _isSaving;

        [ObservableProperty]
        private bool _hasError;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _hasUnsavedChanges;

        [ObservableProperty]
        private string _validationSummary = string.Empty;

        [ObservableProperty]
        private bool _hasValidationErrors;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private bool _isEnabled = true;

        #endregion

        #region Commands

        [RelayCommand]
        protected abstract Task SaveAsync();

        [RelayCommand]
        protected abstract Task CancelAsync();

        [RelayCommand]
        protected virtual Task ClearErrorAsync()
        {
            HasError = false;
            ErrorMessage = string.Empty;
            return Task.CompletedTask;
        }

        [RelayCommand]
        protected virtual Task ResetAsync()
        {
            ClearErrorState();
            ClearValidationErrors();
            MarkAsSaved();
            return Task.CompletedTask;
        }

        #endregion

        protected BaseCrudViewModel(ILogger logger)
        {
            _logger = logger;
        }

        #region Protected Helper Methods

        /// <summary>
        /// Set loading state with optional message
        /// </summary>
        protected virtual void SetLoadingState(bool isLoading, string message = "")
        {
            IsLoading = isLoading;
            IsEnabled = !isLoading;
            
            if (isLoading && !string.IsNullOrEmpty(message))
            {
                _logger.LogInformation(message);
            }
        }

        /// <summary>
        /// Set saving state with optional message
        /// </summary>
        protected virtual void SetSavingState(bool isSaving, string message = "")
        {
            IsSaving = isSaving;
            IsEnabled = !isSaving;
            
            if (isSaving && !string.IsNullOrEmpty(message))
            {
                _logger.LogInformation(message);
            }
        }

        /// <summary>
        /// Set error state with message
        /// </summary>
        protected virtual void SetErrorState(string errorMessage, Exception? exception = null)
        {
            HasError = true;
            ErrorMessage = errorMessage;
            IsEnabled = true;
            
            if (exception != null)
            {
                _logger.LogError(exception, "ViewModel error: {ErrorMessage}", errorMessage);
            }
            else
            {
                _logger.LogWarning("ViewModel error: {ErrorMessage}", errorMessage);
            }
        }

        /// <summary>
        /// Clear error state
        /// </summary>
        protected virtual void ClearErrorState()
        {
            HasError = false;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// Set validation errors
        /// </summary>
        protected virtual void SetValidationErrors(IEnumerable<string> errors)
        {
            var errorList = errors.ToList();
            HasValidationErrors = errorList.Any();
            ValidationSummary = string.Join(Environment.NewLine, errorList);
        }

        /// <summary>
        /// Clear validation errors
        /// </summary>
        protected virtual void ClearValidationErrors()
        {
            HasValidationErrors = false;
            ValidationSummary = string.Empty;
        }

        /// <summary>
        /// Mark as having unsaved changes
        /// </summary>
        protected virtual void MarkAsChanged()
        {
            HasUnsavedChanges = true;
        }

        /// <summary>
        /// Mark as saved (no pending changes)
        /// </summary>
        protected virtual void MarkAsSaved()
        {
            HasUnsavedChanges = false;
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Validate the current state
        /// </summary>
        protected abstract Task<bool> ValidateAsync();

        /// <summary>
        /// Initialize the ViewModel with data
        /// </summary>
        public abstract Task InitializeAsync();

        #endregion
    }
}