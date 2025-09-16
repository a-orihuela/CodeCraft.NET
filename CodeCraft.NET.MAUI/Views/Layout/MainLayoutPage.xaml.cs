using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using CodeCraft.NET.MAUI.Views.Layout;

namespace CodeCraft.NET.MAUI.Views.Layout
{
    /// <summary>
    /// Main layout page that orchestrates the responsive sidebar and content area
    /// Handles mobile/desktop layout transitions and sidebar state management
    /// </summary>
    public partial class MainLayoutPage : ContentPage
    {
        private readonly ILogger<MainLayoutPage> _logger;
        
        public static readonly BindableProperty SidebarWidthProperty =
            BindableProperty.Create(nameof(SidebarWidth), typeof(double), typeof(MainLayoutPage), 280.0);

        public static readonly BindableProperty IsMobileModeProperty =
            BindableProperty.Create(nameof(IsMobileMode), typeof(bool), typeof(MainLayoutPage), false);

        public static readonly BindableProperty IsMobileOverlayVisibleProperty =
            BindableProperty.Create(nameof(IsMobileOverlayVisible), typeof(bool), typeof(MainLayoutPage), false);

        public static readonly BindableProperty CurrentPageTitleProperty =
            BindableProperty.Create(nameof(CurrentPageTitle), typeof(string), typeof(MainLayoutPage), "Dashboard");

        public static readonly BindableProperty CurrentPageContentProperty =
            BindableProperty.Create(nameof(CurrentPageContent), typeof(ContentView), typeof(MainLayoutPage), null);

        public double SidebarWidth
        {
            get => (double)GetValue(SidebarWidthProperty);
            set => SetValue(SidebarWidthProperty, value);
        }

        public bool IsMobileMode
        {
            get => (bool)GetValue(IsMobileModeProperty);
            set => SetValue(IsMobileModeProperty, value);
        }

        public bool IsMobileOverlayVisible
        {
            get => (bool)GetValue(IsMobileOverlayVisibleProperty);
            set => SetValue(IsMobileOverlayVisibleProperty, value);
        }

        public string CurrentPageTitle
        {
            get => (string)GetValue(CurrentPageTitleProperty);
            set => SetValue(CurrentPageTitleProperty, value);
        }

        public ContentView CurrentPageContent
        {
            get => (ContentView)GetValue(CurrentPageContentProperty);
            set => SetValue(CurrentPageContentProperty, value);
        }

        public ICommand CloseSidebarCommand { get; private set; }

        // Design token references
        private double _sidebarWidthExpanded;
        private double _sidebarWidthCollapsed;
        private double _desktopMinWidth;

        public MainLayoutPage()
        {
            InitializeComponent();
            BindingContext = this;
            
            // Initialize commands
            CloseSidebarCommand = new Command(CloseSidebar);
            
            // Initialize design tokens
            LoadDesignTokens();
            
            // Set initial sidebar width
            SidebarWidth = _sidebarWidthExpanded;
            
            // Subscribe to events
            SizeChanged += OnSizeChanged;
            SidebarMenu.PropertyChanged += OnSidebarMenuPropertyChanged;
            
            // Set references between components
            AppHeader.SetSidebarReference(SidebarMenu);
            
            // Initialize page info
            AppHeader.UpdatePageInfo("Dashboard", "Overview and quick actions");
        }

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public MainLayoutPage(ILogger<MainLayoutPage> logger) : this()
        {
            _logger = logger;
        }

        /// <summary>
        /// Load design tokens from resources
        /// </summary>
        private void LoadDesignTokens()
        {
            try
            {
                _sidebarWidthExpanded = (double)Microsoft.Maui.Controls.Application.Current.Resources["SidebarWidthExpanded"];
                _sidebarWidthCollapsed = (double)Microsoft.Maui.Controls.Application.Current.Resources["SidebarWidthCollapsed"];
                _desktopMinWidth = (double)Microsoft.Maui.Controls.Application.Current.Resources["AppDesktopMinWidth"];
                
                _logger?.LogDebug("Design tokens loaded - Expanded: {Expanded}, Collapsed: {Collapsed}, MinWidth: {MinWidth}",
                    _sidebarWidthExpanded, _sidebarWidthCollapsed, _desktopMinWidth);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Failed to load design tokens, using defaults");
                _sidebarWidthExpanded = 280;
                _sidebarWidthCollapsed = 60;
                _desktopMinWidth = 1024;
            }
        }

        /// <summary>
        /// Handle size changes for responsive behavior
        /// </summary>
        private void OnSizeChanged(object sender, EventArgs e)
        {
            if (Width > 0)
            {
                var wasMobileMode = IsMobileMode;
                IsMobileMode = Width < _desktopMinWidth;
                
                UpdateLayoutForScreenSize();
                
                // Log layout changes
                if (wasMobileMode != IsMobileMode)
                {
                    _logger?.LogInformation("Layout mode changed - Width: {Width}, IsMobileMode: {IsMobileMode}", 
                        Width, IsMobileMode);
                }
            }
        }

        /// <summary>
        /// Handle sidebar menu property changes
        /// </summary>
        private void OnSidebarMenuPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SidebarMenuView.IsExpanded))
            {
                UpdateSidebarWidth();
            }
            else if (e.PropertyName == nameof(SidebarMenuView.IsMobileMode))
            {
                UpdateLayoutForScreenSize();
            }
        }

        /// <summary>
        /// Update layout based on current screen size
        /// </summary>
        private void UpdateLayoutForScreenSize()
        {
            if (IsMobileMode)
            {
                // Mobile: Sidebar overlays content
                SidebarColumn.Width = new GridLength(SidebarMenu.IsExpanded ? _sidebarWidthExpanded : 0);
                IsMobileOverlayVisible = SidebarMenu.IsExpanded;
                
                // Ensure sidebar is in mobile mode
                if (!SidebarMenu.IsMobileMode)
                {
                    SidebarMenu.SetExpanded(false);
                }
            }
            else
            {
                // Desktop: Sidebar is always visible
                UpdateSidebarWidth();
                IsMobileOverlayVisible = false;
            }
        }

        /// <summary>
        /// Update sidebar width based on expansion state
        /// </summary>
        private void UpdateSidebarWidth()
        {
            if (IsMobileMode)
            {
                // Mobile: Full width when expanded, hidden when collapsed
                SidebarWidth = SidebarMenu.IsExpanded ? _sidebarWidthExpanded : 0;
                SidebarColumn.Width = new GridLength(SidebarWidth);
                IsMobileOverlayVisible = SidebarMenu.IsExpanded;
            }
            else
            {
                // Desktop: Expanded or collapsed width
                SidebarWidth = SidebarMenu.IsExpanded ? _sidebarWidthExpanded : _sidebarWidthCollapsed;
                SidebarColumn.Width = new GridLength(SidebarWidth);
                IsMobileOverlayVisible = false;
            }

            _logger?.LogDebug("Sidebar width updated - Width: {Width}, IsExpanded: {IsExpanded}, IsMobile: {IsMobile}",
                SidebarWidth, SidebarMenu.IsExpanded, IsMobileMode);
        }

        /// <summary>
        /// Close sidebar when overlay is tapped (mobile mode)
        /// </summary>
        private void CloseSidebar()
        {
            if (IsMobileMode)
            {
                SidebarMenu.SetExpanded(false);
                _logger?.LogInformation("Sidebar closed via overlay tap");
            }
        }

        /// <summary>
        /// Navigate to a specific page and update content area
        /// </summary>
        public async Task NavigateToPageAsync(string pageName, ContentView pageContent = null)
        {
            try
            {
                // Update current page tracking
                CurrentPageTitle = pageName;
                
                // Update header title based on page
                var (title, subtitle) = GetPageTitleInfo(pageName);
                AppHeader.UpdatePageInfo(title, subtitle);

                // Load page content if provided
                if (pageContent != null)
                {
                    CurrentPageContent = pageContent;
                    ContentArea.Content = pageContent;
                }

                _logger?.LogInformation("Navigated to page: {PageName}", pageName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to page: {PageName}", pageName);
            }
        }

        /// <summary>
        /// Get title and subtitle information for a page
        /// </summary>
        private (string title, string subtitle) GetPageTitleInfo(string pageName)
        {
            return pageName.ToLowerInvariant() switch
            {
                "dashboard" => ("Dashboard", "Overview and quick actions"),
                "configuration" => ("Personal Profile Configuration", "Configure your personal data for accurate calorie calculations"),
                "generateplan" => ("Generate Plan", "Create new meal plan"),
                "planhistory" => ("Plan History", "View previous plans"),
                "foodcatalog" => ("Food Catalog", "Browse allowed foods"),
                "nutritionrules" => ("Nutrition Rules", "Configure diet rules"),
                "settings" => ("Settings", "Application configuration"),
                "profile" => ("Profile", "User account settings"),
                _ => (pageName, "")
            };
        }

        /// <summary>
        /// Show a temporary notification or status message
        /// </summary>
        public async Task ShowNotificationAsync(string message, string type = "info")
        {
            try
            {
                // For now, use a simple alert
                // In a real app, you might want a custom notification system
                await DisplayAlert("Notification", message, "OK");
                
                _logger?.LogInformation("Notification shown - Type: {Type}, Message: {Message}", type, message);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to show notification");
            }
        }

        /// <summary>
        /// Update user information in header
        /// </summary>
        public void UpdateUserInfo(string name, string email, string role = "User")
        {
            AppHeader.UpdateUserInfo(name, email, role);
            _logger?.LogDebug("User info updated in main layout");
        }

        /// <summary>
        /// Set the minimum width for desktop mode (configurable)
        /// </summary>
        public void SetDesktopMinWidth(double width)
        {
            _desktopMinWidth = Math.Max(width, 800); // Ensure minimum usable width
            UpdateLayoutForScreenSize();
            
            _logger?.LogInformation("Desktop minimum width updated to: {Width}px", _desktopMinWidth);
        }
    }
}