using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using CodeCraft.NET.MAUI.Views.Controls;

namespace CodeCraft.NET.MAUI.Views.Layout
{
    /// <summary>
    /// Sidebar navigation menu view with responsive expansion/collapse behavior
    /// Supports both mobile hamburger menu and desktop sidebar layouts
    /// Now with accordion behavior - only one section expanded at a time
    /// </summary>
    public partial class SidebarMenuView : ContentView
    {
        private readonly ILogger<SidebarMenuView> _logger;
        
        // Track all expansion views for accordion behavior
        private List<ExpansionView> _allExpansionViews = new List<ExpansionView>();

        public static readonly BindableProperty IsExpandedProperty =
            BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(SidebarMenuView), true, BindingMode.TwoWay);

        public static readonly BindableProperty IsMobileModeProperty =
            BindableProperty.Create(nameof(IsMobileMode), typeof(bool), typeof(SidebarMenuView), false, BindingMode.TwoWay);

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public bool IsMobileMode
        {
            get => (bool)GetValue(IsMobileModeProperty);
            set => SetValue(IsMobileModeProperty, value);
        }

        public ICommand ToggleExpansionCommand { get; private set; }

        // Navigation commands
        public ICommand NavigateToDashboardCommand { get; private set; }
        
        public SidebarMenuView()
        {
            InitializeComponent();
            BindingContext = this;
            
            // Initialize commands
            InitializeCommands();
            
            // Configure expandable menu sections
            ConfigureMenuSections();
            
            // Subscribe to size changes for responsive behavior
            SizeChanged += OnSizeChanged;
            
            _logger?.LogInformation("SidebarMenuView initialized with expandable sections");
        }

        /// <summary>
        /// Constructor with dependency injection for logger
        /// </summary>
        public SidebarMenuView(ILogger<SidebarMenuView> logger) : this()
        {
            _logger = logger;
        }

        private void InitializeCommands()
        {
            ToggleExpansionCommand = new Command(ToggleExpansion);
            NavigateToDashboardCommand = new Command(async () => await NavigateToDashboard());
        }

        /// <summary>
        /// Configure the expandable menu sections with their respective menu items
        /// </summary>
        private void ConfigureMenuSections()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("?? ConfigureMenuSections START - Full Implementation");
                _logger?.LogInformation("Starting menu sections configuration...");

                var dashboardSection = this.FindByName<ExpansionView>("DashboardSection");

                _allExpansionViews.Clear();
                if (dashboardSection != null) _allExpansionViews.Add(dashboardSection);

                // Subscribe to expansion events for accordion behavior
                foreach (var section in _allExpansionViews)
                {
                    section.ExpansionChanged += OnSectionExpansionChanged;
                    System.Diagnostics.Debug.WriteLine($"?? Subscribed to {section.Title} expansion events");
                }

                // DASHBOARD SECTION (single item - direct navigation)
                if (dashboardSection != null)
                {
                    System.Diagnostics.Debug.WriteLine($"?? BEFORE - Dashboard IsSingleItem: {dashboardSection.IsSingleItem}");
                    System.Diagnostics.Debug.WriteLine($"?? Configuring Dashboard section as single item");
                    dashboardSection.ColorIndex = 1;
                    dashboardSection.IsSingleItem = true; // This will hide the + icon
                    dashboardSection.SingleItemCommand = NavigateToDashboardCommand; // Direct navigation command
                    dashboardSection.MenuItems = new ObservableCollection<MenuItemData>(); // Empty collection for single items
                    System.Diagnostics.Debug.WriteLine($"?? AFTER - Dashboard IsSingleItem: {dashboardSection.IsSingleItem}");
                    System.Diagnostics.Debug.WriteLine($"? Dashboard section configured - IsSingleItem: {dashboardSection.IsSingleItem}, ExpandIcon: '{dashboardSection.ExpandIcon}'");
                    System.Diagnostics.Debug.WriteLine($"? Dashboard SingleItemCommand: {dashboardSection.SingleItemCommand != null}");
                    
                    // Force UI update after a short delay to ensure property changes are processed
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        System.Diagnostics.Debug.WriteLine($"?? Force updating Dashboard UI - IsSingleItem: {dashboardSection.IsSingleItem}");
                        var iconLabel = dashboardSection.FindByName<Label>("ExpandIconLabel");
                        var contentContainer = dashboardSection.FindByName<StackLayout>("ContentContainer");
                        
                        if (iconLabel != null)
                        {
                            iconLabel.Text = dashboardSection.ExpandIcon;
                            iconLabel.IsVisible = !dashboardSection.IsSingleItem;
                            System.Diagnostics.Debug.WriteLine($"?? Dashboard icon updated - Text: '{iconLabel.Text}', Visible: {iconLabel.IsVisible}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"? Could not find ExpandIconLabel in Dashboard");
                        }
                        
                        if (contentContainer != null)
                        {
                            contentContainer.IsVisible = dashboardSection.IsContentVisible;
                            System.Diagnostics.Debug.WriteLine($"?? Dashboard content visibility: {contentContainer.IsVisible}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"? Could not find ContentContainer in Dashboard");
                        }
                    });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"? DASHBOARD SECTION NOT FOUND!");
                }

                _logger?.LogInformation("Menu sections configured successfully - Full implementation complete");
                System.Diagnostics.Debug.WriteLine("?? ConfigureMenuSections END - All sections ready");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"?? ERROR in ConfigureMenuSections: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"?? Stack trace: {ex.StackTrace}");
                _logger?.LogError(ex, "Failed to configure menu sections");
            }
        }

        /// <summary>
        /// Toggle sidebar expansion state
        /// </summary>
        private void ToggleExpansion()
        {
            IsExpanded = !IsExpanded;
            _logger?.LogInformation("Sidebar toggled - IsExpanded: {IsExpanded}", IsExpanded);
        }

        /// <summary>
        /// Navigation command implementations
        /// </summary>
        private async Task NavigateToDashboard()
        {
            try
            {
                _logger?.LogInformation("Navigating to Dashboard");
                System.Diagnostics.Debug.WriteLine("?? NavigateToDashboard called - Finding parent PrincipalPage");
                
                // Find the parent PrincipalPage and call its navigation method
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    System.Diagnostics.Debug.WriteLine("?? Found PrincipalPage, calling LoadContentForSection('dashboard')");
                    principalPage.LoadContentForSection("dashboard");
                    System.Diagnostics.Debug.WriteLine("? Dashboard navigation completed");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("? Could not find parent PrincipalPage");
                    await Shell.Current.DisplayAlert("Navigation", "Could not find parent page", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Dashboard");
                System.Diagnostics.Debug.WriteLine($"? Dashboard navigation failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Helper method to find parent of specific type
        /// </summary>
        private T FindParentOfType<T>(Element element) where T : Element
        {
            var current = element.Parent;
            while (current != null)
            {
                if (current is T parent)
                    return parent;
                current = current.Parent;
            }
            return null;
        }

        // Add missing OnSizeChanged method
        private void OnSizeChanged(object sender, EventArgs e)
        {
            if (Width > 0)
            {
                // Use desktop min width token to determine mobile mode
                try
                {
                    var desktopMinWidth = (double)Microsoft.Maui.Controls.Application.Current.Resources["AppDesktopMinWidth"];
                    
                    var wasMobileMode = IsMobileMode;
                    IsMobileMode = Width < desktopMinWidth;
                    
                    // Auto-collapse when entering mobile mode
                    if (IsMobileMode && !wasMobileMode)
                    {
                        IsExpanded = false;
                    }
                    // Auto-expand when entering desktop mode
                    else if (!IsMobileMode && wasMobileMode)
                    {
                        IsExpanded = true;
                    }

                    _logger?.LogDebug("Size changed - Width: {Width}, IsMobileMode: {IsMobileMode}, IsExpanded: {IsExpanded}", 
                        Width, IsMobileMode, IsExpanded);
                }
                catch
                {
                    // Fallback if resource not found
                    IsMobileMode = Width < 1024;
                }
            }
        }

        /// <summary>
        /// Public method to toggle sidebar from external components (e.g., header hamburger button)
        /// </summary>
        public void ToggleSidebar()
        {
            ToggleExpansion();
        }

        /// <summary>
        /// Public method to set expanded state
        /// </summary>
        public void SetExpanded(bool expanded)
        {
            IsExpanded = expanded;
        }

        /// <summary>
        /// Event handler for expansion changes in submenu sections (for accordion behavior)
        /// </summary>
        private void OnSectionExpansionChanged(object sender, ExpansionViewExpansionChangedEventArgs e)
        {
            if (sender is ExpansionView expandedSection && e.IsExpanded)
            {
                System.Diagnostics.Debug.WriteLine($"?? Accordion: {expandedSection.Title} is expanding, collapsing others");
                
                // Collapse all other sections
                foreach (var section in _allExpansionViews)
                {
                    if (section != expandedSection && section.IsExpanded)
                    {
                        System.Diagnostics.Debug.WriteLine($"?? Accordion: Collapsing {section.Title}");
                        section.IsExpanded = false;
                    }
                }
            }
        }
    }
}