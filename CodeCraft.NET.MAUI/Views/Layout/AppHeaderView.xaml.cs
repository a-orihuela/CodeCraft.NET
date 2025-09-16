using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using CodeCraft.NET.MAUI.Views.Layout;

namespace CodeCraft.NET.MAUI.Views.Layout
{
    /// <summary>
    /// App header view with responsive behavior for mobile and desktop layouts
    /// Handles page titles, search functionality, and user profile menu
    /// </summary>
    public partial class AppHeaderView : ContentView
    {
        private readonly ILogger<AppHeaderView> _logger;
        private SidebarMenuView _sidebarMenuView;

        public static readonly BindableProperty CurrentPageTitleProperty =
            BindableProperty.Create(nameof(CurrentPageTitle), typeof(string), typeof(AppHeaderView), "Dashboard");

        public static readonly BindableProperty CurrentPageSubtitleProperty =
            BindableProperty.Create(nameof(CurrentPageSubtitle), typeof(string), typeof(AppHeaderView), "Overview and quick actions");

        public static readonly BindableProperty HasSubtitleProperty =
            BindableProperty.Create(nameof(HasSubtitle), typeof(bool), typeof(AppHeaderView), true);

        public static readonly BindableProperty IsMobileModeProperty =
            BindableProperty.Create(nameof(IsMobileMode), typeof(bool), typeof(AppHeaderView), false);

        public static readonly BindableProperty IsDesktopModeProperty =
            BindableProperty.Create(nameof(IsDesktopMode), typeof(bool), typeof(AppHeaderView), true);

        public static readonly BindableProperty IsProfileMenuOpenProperty =
            BindableProperty.Create(nameof(IsProfileMenuOpen), typeof(bool), typeof(AppHeaderView), false);

        public static readonly BindableProperty CurrentUserNameProperty =
            BindableProperty.Create(nameof(CurrentUserName), typeof(string), typeof(AppHeaderView), "Demo User");

        public static readonly BindableProperty CurrentUserEmailProperty =
            BindableProperty.Create(nameof(CurrentUserEmail), typeof(string), typeof(AppHeaderView), "demo@ketoplanner.app");

        public static readonly BindableProperty CurrentUserRoleProperty =
            BindableProperty.Create(nameof(CurrentUserRole), typeof(string), typeof(AppHeaderView), "Premium User");

        public static readonly BindableProperty UserInitialsProperty =
            BindableProperty.Create(nameof(UserInitials), typeof(string), typeof(AppHeaderView), "DU");

        public static readonly BindableProperty ThemeToggleTextProperty =
            BindableProperty.Create(nameof(ThemeToggleText), typeof(string), typeof(AppHeaderView), "?? Dark Mode");

        public string CurrentPageTitle
        {
            get => (string)GetValue(CurrentPageTitleProperty);
            set => SetValue(CurrentPageTitleProperty, value);
        }

        public string CurrentPageSubtitle
        {
            get => (string)GetValue(CurrentPageSubtitleProperty);
            set => SetValue(CurrentPageSubtitleProperty, value);
        }

        public bool HasSubtitle
        {
            get => (bool)GetValue(HasSubtitleProperty);
            set => SetValue(HasSubtitleProperty, value);
        }

        public bool IsMobileMode
        {
            get => (bool)GetValue(IsMobileModeProperty);
            set => SetValue(IsMobileModeProperty, value);
        }

        public bool IsDesktopMode
        {
            get => (bool)GetValue(IsDesktopModeProperty);
            set => SetValue(IsDesktopModeProperty, value);
        }

        public bool IsProfileMenuOpen
        {
            get => (bool)GetValue(IsProfileMenuOpenProperty);
            set => SetValue(IsProfileMenuOpenProperty, value);
        }

        public string CurrentUserName
        {
            get => (string)GetValue(CurrentUserNameProperty);
            set => SetValue(CurrentUserNameProperty, value);
        }

        public string CurrentUserEmail
        {
            get => (string)GetValue(CurrentUserEmailProperty);
            set => SetValue(CurrentUserEmailProperty, value);
        }

        public string CurrentUserRole
        {
            get => (string)GetValue(CurrentUserRoleProperty);
            set => SetValue(CurrentUserRoleProperty, value);
        }

        public string UserInitials
        {
            get => (string)GetValue(UserInitialsProperty);
            set => SetValue(UserInitialsProperty, value);
        }

        public string ThemeToggleText
        {
            get => (string)GetValue(ThemeToggleTextProperty);
            set => SetValue(ThemeToggleTextProperty, value);
        }

        public ICommand ToggleSidebarCommand { get; private set; }
        public ICommand OpenSearchCommand { get; private set; }
        public ICommand OpenProfileMenuCommand { get; private set; }
        public ICommand CloseProfileMenuCommand { get; private set; }
        public ICommand NavigateToProfileCommand { get; private set; }
        public ICommand NavigateToSettingsCommand { get; private set; }
        public ICommand ToggleThemeCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        public AppHeaderView()
        {
            InitializeComponent();
            BindingContext = this;
            
            // Initialize commands
            InitializeCommands();
            
            // Subscribe to size changes for responsive behavior
            SizeChanged += OnSizeChanged;
            
            // Initialize theme toggle text based on current theme
            UpdateThemeToggleText();
        }

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public AppHeaderView(ILogger<AppHeaderView> logger) : this()
        {
            _logger = logger;
        }

        private void InitializeCommands()
        {
            ToggleSidebarCommand = new Command(ToggleSidebar);
            OpenSearchCommand = new Command(async () => await OpenSearch());
            OpenProfileMenuCommand = new Command(OpenProfileMenu);
            CloseProfileMenuCommand = new Command(CloseProfileMenu);
            NavigateToProfileCommand = new Command(async () => await NavigateToProfile());
            NavigateToSettingsCommand = new Command(async () => await NavigateToSettings());
            ToggleThemeCommand = new Command(ToggleTheme);
            LogoutCommand = new Command(async () => await Logout());
        }

        /// <summary>
        /// Set reference to sidebar for toggle functionality
        /// </summary>
        public void SetSidebarReference(SidebarMenuView sidebarMenuView)
        {
            _sidebarMenuView = sidebarMenuView;
        }

        /// <summary>
        /// Toggle sidebar visibility (mobile hamburger menu)
        /// </summary>
        private void ToggleSidebar()
        {
            try
            {
                _sidebarMenuView?.ToggleSidebar();
                _logger?.LogInformation("Sidebar toggle requested from header");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to toggle sidebar from header");
            }
        }

        /// <summary>
        /// Open search functionality
        /// </summary>
        private async Task OpenSearch()
        {
            try
            {
                // Navigate to search page or show search modal
                await Shell.Current.GoToAsync("//Search");
                _logger?.LogInformation("Search opened");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to open search");
            }
        }

        /// <summary>
        /// Toggle profile dropdown menu
        /// </summary>
        private void OpenProfileMenu()
        {
            IsProfileMenuOpen = true;
            _logger?.LogInformation("Profile menu opened");
        }

        /// <summary>
        /// Close profile dropdown menu
        /// </summary>
        private void CloseProfileMenu()
        {
            IsProfileMenuOpen = false;
            _logger?.LogInformation("Profile menu closed");
        }

        /// <summary>
        /// Navigate to user profile
        /// </summary>
        private async Task NavigateToProfile()
        {
            try
            {
                IsProfileMenuOpen = false;
                await Shell.Current.GoToAsync("//Profile");
                _logger?.LogInformation("Navigated to profile");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to profile");
            }
        }

        /// <summary>
        /// Navigate to settings
        /// </summary>
        private async Task NavigateToSettings()
        {
            try
            {
                IsProfileMenuOpen = false;
                await Shell.Current.GoToAsync("//Settings");
                _logger?.LogInformation("Navigated to settings");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to settings");
            }
        }

        /// <summary>
        /// Toggle between light and dark theme
        /// </summary>
        private void ToggleTheme()
        {
            try
            {
                // Toggle the app theme
                var currentTheme = Microsoft.Maui.Controls.Application.Current.UserAppTheme;
                Microsoft.Maui.Controls.Application.Current.UserAppTheme = currentTheme == AppTheme.Dark 
                    ? AppTheme.Light 
                    : AppTheme.Dark;

                UpdateThemeToggleText();
                IsProfileMenuOpen = false;
                
                _logger?.LogInformation("Theme toggled to: {Theme}", Microsoft.Maui.Controls.Application.Current.UserAppTheme);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to toggle theme");
            }
        }

        /// <summary>
        /// Handle user logout
        /// </summary>
        private async Task Logout()
        {
            try
            {
                IsProfileMenuOpen = false;
                
                // Show confirmation dialog
                bool confirmed = await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert(
                    "Sign Out", 
                    "Are you sure you want to sign out?", 
                    "Sign Out", 
                    "Cancel");

                if (confirmed)
                {
                    // Clear user data and navigate to login/welcome
                    await Shell.Current.GoToAsync("//Welcome");
                    _logger?.LogInformation("User signed out");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to logout");
            }
        }

        /// <summary>
        /// Update page title and subtitle
        /// </summary>
        public void UpdatePageInfo(string title, string subtitle = null)
        {
            CurrentPageTitle = title;
            CurrentPageSubtitle = subtitle;
            HasSubtitle = !string.IsNullOrEmpty(subtitle);
            
            _logger?.LogDebug("Page info updated - Title: {Title}, Subtitle: {Subtitle}", title, subtitle);
        }

        /// <summary>
        /// Update user information
        /// </summary>
        public void UpdateUserInfo(string name, string email, string role = null)
        {
            CurrentUserName = name;
            CurrentUserEmail = email;
            CurrentUserRole = role ?? "User";
            
            // Generate initials from name
            UserInitials = GenerateInitials(name);
            
            _logger?.LogDebug("User info updated - Name: {Name}, Email: {Email}", name, email);
        }

        /// <summary>
        /// Handle size changes for responsive behavior
        /// </summary>
        private void OnSizeChanged(object sender, EventArgs e)
        {
            if (Width > 0)
            {
                var desktopMinWidth = (double)Microsoft.Maui.Controls.Application.Current.Resources["AppDesktopMinWidth"];
                
                IsMobileMode = Width < desktopMinWidth;
                IsDesktopMode = !IsMobileMode;

                _logger?.LogDebug("Header size changed - Width: {Width}, IsMobileMode: {IsMobileMode}", 
                    Width, IsMobileMode);
            }
        }

        /// <summary>
        /// Update theme toggle text based on current theme
        /// </summary>
        private void UpdateThemeToggleText()
        {
            ThemeToggleText = Microsoft.Maui.Controls.Application.Current.UserAppTheme == AppTheme.Dark 
                ? "?? Light Mode" 
                : "?? Dark Mode";
        }

        /// <summary>
        /// Generate initials from user name
        /// </summary>
        private string GenerateInitials(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "U";

            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
                return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
            
            return (parts[0][0].ToString() + parts[parts.Length - 1][0].ToString()).ToUpper();
        }
    }
}