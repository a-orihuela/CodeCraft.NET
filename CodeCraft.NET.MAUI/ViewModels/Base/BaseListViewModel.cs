using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace CodeCraft.NET.MAUI.ViewModels.Base
{
    /// <summary>
    /// Base ViewModel for List operations
    /// Provides common functionality for listing, searching, and managing collections
    /// </summary>
    public abstract partial class BaseListViewModel<T> : ObservableObject where T : class
    {
        protected readonly ILogger _logger;

        #region Observable Properties

        [ObservableProperty]
        private ObservableCollection<T> _items = new();

        [ObservableProperty]
        private ObservableCollection<T> _filteredItems = new();

        [ObservableProperty]
        private T? _selectedItem;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private bool _hasError;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private bool _isEmpty;

        [ObservableProperty]
        private string _emptyMessage = "No items found";

        [ObservableProperty]
        private int _totalCount;

        [ObservableProperty]
        private bool _isEnabled = true;

        [ObservableProperty]
        private string _title = string.Empty;

        #endregion

        #region Commands

        [RelayCommand]
        protected abstract Task LoadItemsAsync();

        [RelayCommand]
        protected virtual async Task RefreshAsync()
        {
            IsRefreshing = true;
            try
            {
                await LoadItemsAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        protected abstract Task CreateItemAsync();

        [RelayCommand]
        protected abstract Task EditItemAsync(T? item);

        [RelayCommand]
        protected abstract Task DeleteItemAsync(T? item);

        [RelayCommand]
        protected abstract Task ViewItemAsync(T? item);

        [RelayCommand]
        protected virtual Task SearchAsync()
        {
            ApplyFilter();
            return Task.CompletedTask;
        }

        [RelayCommand]
        protected virtual Task ClearSearchAsync()
        {
            SearchText = string.Empty;
            ApplyFilter();
            return Task.CompletedTask;
        }

        [RelayCommand]
        protected virtual Task ClearErrorAsync()
        {
            HasError = false;
            ErrorMessage = string.Empty;
            return Task.CompletedTask;
        }

        [RelayCommand]
        protected virtual Task SelectItemAsync(T? item)
        {
            SelectedItem = item;
            return Task.CompletedTask;
        }

        #endregion

        protected BaseListViewModel(ILogger logger)
        {
            _logger = logger;
            PropertyChanged += OnPropertyChanged;
        }

        #region Abstract Methods

        /// <summary>
        /// Filter predicate for search functionality
        /// </summary>
        protected abstract bool FilterPredicate(T item, string searchText);

        /// <summary>
        /// Initialize the ViewModel with data
        /// </summary>
        public abstract Task InitializeAsync();

        #endregion

        #region Protected Helper Methods

        /// <summary>
        /// Set the items collection and update UI state
        /// </summary>
        protected virtual void SetItems(IEnumerable<T> items)
        {
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
            
            TotalCount = Items.Count;
            ApplyFilter();
            UpdateEmptyState();
        }

        /// <summary>
        /// Apply search filter to items
        /// </summary>
        protected virtual void ApplyFilter()
        {
            FilteredItems.Clear();
            
            var itemsToShow = string.IsNullOrWhiteSpace(SearchText) 
                ? Items 
                : Items.Where(item => FilterPredicate(item, SearchText));
                
            foreach (var item in itemsToShow)
            {
                FilteredItems.Add(item);
            }
            
            UpdateEmptyState();
        }

        /// <summary>
        /// Update empty state based on filtered items
        /// </summary>
        protected virtual void UpdateEmptyState()
        {
            IsEmpty = FilteredItems.Count == 0;
            
            if (IsEmpty)
            {
                EmptyMessage = string.IsNullOrWhiteSpace(SearchText) 
                    ? "No items found" 
                    : $"No items match '{SearchText}'";
            }
        }

        #endregion

        #region Private Methods

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchText))
            {
                ApplyFilter();
            }
        }

        #endregion
    }
}