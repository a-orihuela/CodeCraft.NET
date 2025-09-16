using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        public ICommand NavigateToConfigurationCommand { get; private set; }
        public ICommand NavigateToGeneratePlanCommand { get; private set; }
        public ICommand NavigateToPlanHistoryCommand { get; private set; }
        public ICommand NavigateToFoodCatalogCommand { get; private set; }
        public ICommand NavigateToNutritionRulesCommand { get; private set; }
        public ICommand NavigateToSettingsCommand { get; private set; }
        public ICommand NavigateToProfileCommand { get; private set; }
        public ICommand NavigateToHomeCommand { get; private set; }
        public ICommand NavigateToDesktopApiDemoCommand { get; private set; }
        public ICommand NavigateToDesignTokensDemoCommand { get; private set; }
        public ICommand NavigateToRealChartsDemoCommand { get; private set; }
        public ICommand NavigateToAnalyticsDashboardCommand { get; private set; }
        public ICommand NavigateToResponsiveLayoutDemoCommand { get; private set; }
        public ICommand NavigateToClientComparisonCommand { get; private set; }

        // New specific navigation commands for generated pages
        public ICommand NavigateToUserManagementCommand { get; private set; }
        public ICommand NavigateToUserProfileListCommand { get; private set; }
        public ICommand NavigateToGeneratedPlansCommand { get; private set; }
        public ICommand NavigateToPlanDetailsCommand { get; private set; }
        public ICommand NavigateToWeeklyPlansCommand { get; private set; }
        public ICommand NavigateToDailyPlansCommand { get; private set; }
        public ICommand NavigateToMealsCommand { get; private set; }
        public ICommand NavigateToFoodPortionsCommand { get; private set; }
        public ICommand NavigateToShoppingListsCommand { get; private set; }
        public ICommand NavigateToUserPreferencesCommand { get; private set; }

        // New analytics and client reporting commands
        public ICommand NavigateToClientReportsCommand { get; private set; }
        public ICommand NavigateToPerformanceInsightsCommand { get; private set; }
        
        // System management commands moved to configuration
        public ICommand NavigateToLogManagementCommand { get; private set; }

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
            NavigateToConfigurationCommand = new Command(async () => await NavigateToConfiguration());
            NavigateToGeneratePlanCommand = new Command(async () => await NavigateToGeneratePlan());
            NavigateToPlanHistoryCommand = new Command(async () => await NavigateToPlanHistory());
            NavigateToFoodCatalogCommand = new Command(async () => await NavigateToFoodCatalog());
            NavigateToNutritionRulesCommand = new Command(async () => await NavigateToNutritionRules());
            NavigateToSettingsCommand = new Command(async () => await NavigateToSettings());
            NavigateToProfileCommand = new Command(async () => await NavigateToProfile());
            NavigateToHomeCommand = new Command(async () => await NavigateToHome());
            NavigateToDesktopApiDemoCommand = new Command(async () => await NavigateToDesktopApiDemo());
            NavigateToDesignTokensDemoCommand = new Command(async () => await NavigateToDesignTokensDemo());
            NavigateToRealChartsDemoCommand = new Command(async () => await NavigateToRealChartsDemo());
            NavigateToAnalyticsDashboardCommand = new Command(async () => await NavigateToAnalyticsDashboard());
            NavigateToResponsiveLayoutDemoCommand = new Command(async () => await NavigateToResponsiveLayoutDemo());
            NavigateToClientComparisonCommand = new Command(async () => await NavigateToClientComparison());

            // Initialize specific navigation commands
            NavigateToUserManagementCommand = new Command(async () => await NavigateToUserManagement());
            NavigateToUserProfileListCommand = new Command(async () => await NavigateToUserProfileList());
            NavigateToGeneratedPlansCommand = new Command(async () => await NavigateToGeneratedPlans());
            NavigateToPlanDetailsCommand = new Command(async () => await NavigateToPlanDetails());
            NavigateToWeeklyPlansCommand = new Command(async () => await NavigateToWeeklyPlans());
            NavigateToDailyPlansCommand = new Command(async () => await NavigateToDailyPlans());
            NavigateToMealsCommand = new Command(async () => await NavigateToMeals());
            NavigateToFoodPortionsCommand = new Command(async () => await NavigateToFoodPortions());
            NavigateToShoppingListsCommand = new Command(async () => await NavigateToShoppingLists());
            NavigateToUserPreferencesCommand = new Command(async () => await NavigateToUserPreferences());

            // Initialize new analytics commands
            NavigateToClientReportsCommand = new Command(async () => await NavigateToClientReports());
            NavigateToPerformanceInsightsCommand = new Command(async () => await NavigateToPerformanceInsights());
            
            // Initialize system management commands moved to configuration
            NavigateToLogManagementCommand = new Command(async () => await NavigateToLogManagement());
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

                // Find all expansion view controls by name
                var dashboardSection = this.FindByName<ExpansionView>("DashboardSection");
                var userManagementSection = this.FindByName<ExpansionView>("UserManagementSection");
                var nutritionPlanningSection = this.FindByName<ExpansionView>("NutritionPlanningSection");
                var foodResourcesSection = this.FindByName<ExpansionView>("FoodResourcesSection");
                var configurationSection = this.FindByName<ExpansionView>("ConfigurationSection");
                var analyticsSection = this.FindByName<ExpansionView>("AnalyticsSection");
                var examplesSection = this.FindByName<ExpansionView>("ExamplesSection");
                // ? REMOVED: var autoGeneratedPagesSection - no longer generating XAML pages

                System.Diagnostics.Debug.WriteLine($"?? SECTION SEARCH RESULTS:");
                System.Diagnostics.Debug.WriteLine($"?? Dashboard: {dashboardSection != null}");
                System.Diagnostics.Debug.WriteLine($"?? UserManagement: {userManagementSection != null}");
                System.Diagnostics.Debug.WriteLine($"?? Analytics: {analyticsSection != null}");
                System.Diagnostics.Debug.WriteLine($"?? Examples: {examplesSection != null}");
                // ? REMOVED: AutoGenerated debug - no longer needed

                // Add all sections to the list for accordion behavior
                _allExpansionViews.Clear();
                if (dashboardSection != null) _allExpansionViews.Add(dashboardSection);
                if (userManagementSection != null) _allExpansionViews.Add(userManagementSection);
                if (nutritionPlanningSection != null) _allExpansionViews.Add(nutritionPlanningSection);
                if (foodResourcesSection != null) _allExpansionViews.Add(foodResourcesSection);
                if (configurationSection != null) _allExpansionViews.Add(configurationSection);
                if (analyticsSection != null) _allExpansionViews.Add(analyticsSection);
                if (examplesSection != null) _allExpansionViews.Add(examplesSection);
                // if (autoGeneratedPagesSection != null) _allExpansionViews.Add(autoGeneratedPagesSection);

                // Subscribe to expansion events for accordion behavior
                foreach (var section in _allExpansionViews)
                {
                    section.ExpansionChanged += OnSectionExpansionChanged;
                    System.Diagnostics.Debug.WriteLine($"?? Subscribed to {section.Title} expansion events");
                }

                System.Diagnostics.Debug.WriteLine($"?? Found sections - Dashboard: {dashboardSection != null}, User: {userManagementSection != null}, Analytics: {analyticsSection != null}");
                System.Diagnostics.Debug.WriteLine($"?? Accordion setup - Total sections: {_allExpansionViews.Count}");

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

                // USER MANAGEMENT SECTION (single item - direct navigation)
                if (userManagementSection != null)
                {
                    System.Diagnostics.Debug.WriteLine($"?? Configuring User Management section as single item");
                    userManagementSection.ColorIndex = 2;
                    userManagementSection.IsSingleItem = true; // Convert to single item
                    userManagementSection.SingleItemCommand = NavigateToUserManagementCommand; // Direct navigation
                    userManagementSection.MenuItems = new ObservableCollection<MenuItemData>(); // Empty collection
                    System.Diagnostics.Debug.WriteLine($"? User Management section configured as single item");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"? USER MANAGEMENT SECTION NOT FOUND!");
                }

                // NUTRITION PLANNING SECTION (EXPANDED with Nutrition Rules)
                if (nutritionPlanningSection != null)
                {
                    nutritionPlanningSection.ColorIndex = 3;
                    nutritionPlanningSection.MenuItems = new ObservableCollection<MenuItemData>
                    {
                        new MenuItemData { Title = "Generated Plans", Icon = "", Command = NavigateToGeneratedPlansCommand, Route = "//GeneratedPlans" },
                        new MenuItemData { Title = "Plan Details", Icon = "", Command = NavigateToPlanDetailsCommand, Route = "plandetails" },
                        new MenuItemData { Title = "Weekly Plans", Icon = "", Command = NavigateToWeeklyPlansCommand, Route = "//WeeklyPlans" },
                        new MenuItemData { Title = "Daily Plans", Icon = "", Command = NavigateToDailyPlansCommand, Route = "//DailyPlans" },
                        new MenuItemData { Title = "Meals", Icon = "", Command = NavigateToMealsCommand, Route = "//Meals" },
                        new MenuItemData { Title = "Generate Plan", Icon = "", Command = NavigateToGeneratePlanCommand, Route = "generate-plan" },
                        new MenuItemData { Title = "Plan History", Icon = "", Command = NavigateToPlanHistoryCommand, Route = "plan-history" },
                        new MenuItemData { Title = "Nutrition Rules", Icon = "", Command = NavigateToNutritionRulesCommand, Route = "//NutritionRules" }
                    };
                    System.Diagnostics.Debug.WriteLine($"? Nutrition Planning section configured with {nutritionPlanningSection.MenuItems.Count} items (including Nutrition Rules)");
                }

                // FOOD RESOURCES SECTION
                if (foodResourcesSection != null)
                {
                    foodResourcesSection.ColorIndex = 4;
                    foodResourcesSection.MenuItems = new ObservableCollection<MenuItemData>
                    {
                        new MenuItemData { Title = "Food Catalog", Icon = "", Command = NavigateToFoodCatalogCommand, Route = "//FoodCatalog" },
                        new MenuItemData { Title = "Food Portions", Icon = "", Command = NavigateToFoodPortionsCommand, Route = "//FoodPortions" },
                        new MenuItemData { Title = "Shopping Lists", Icon = "", Command = NavigateToShoppingListsCommand, Route = "//ShoppingLists" }
                    };
                    System.Diagnostics.Debug.WriteLine($"? Food Resources section configured with {foodResourcesSection.MenuItems.Count} items");
                }

                // CONFIGURATION SECTION (EXPANDED with System Management features)
                if (configurationSection != null)
                {
                    configurationSection.ColorIndex = 5;
                    configurationSection.MenuItems = new ObservableCollection<MenuItemData>
                    {
                        new MenuItemData { Title = "Settings", Icon = "", Command = NavigateToSettingsCommand, Route = "settings" },
                        new MenuItemData { Title = "Log Management", Icon = "", Command = NavigateToLogManagementCommand, Route = "log-management" }
                    };
                    System.Diagnostics.Debug.WriteLine($"? Configuration section configured with {configurationSection.MenuItems.Count} items (including system management)");
                }

                // ANALYTICS SECTION (EXPANDED with Client Management features)
                if (analyticsSection != null)
                {
                    analyticsSection.ColorIndex = 6;
                    analyticsSection.MenuItems = new ObservableCollection<MenuItemData>
                    {
                        new MenuItemData { Title = "Analytics Dashboard", Icon = "", Command = NavigateToAnalyticsDashboardCommand, Route = "//AnalyticsDashboard" },
                        new MenuItemData { Title = "Real Charts Demo", Icon = "", Command = NavigateToRealChartsDemoCommand, Route = "//RealChartsDemo" },
                        new MenuItemData { Title = "Client Comparison", Icon = "", Command = NavigateToClientComparisonCommand, Route = "MultiClientComparison" },
                        new MenuItemData { Title = "Client Reports", Icon = "", Command = NavigateToClientReportsCommand, Route = "client-reports" },
                        new MenuItemData { Title = "Performance Insights", Icon = "", Command = NavigateToPerformanceInsightsCommand, Route = "performance-insights" }
                    };
                    System.Diagnostics.Debug.WriteLine($"? Analytics section configured with {analyticsSection.MenuItems.Count} items (including client features)");
                }

                // EXAMPLES SECTION
                if (examplesSection != null)
                {
                    examplesSection.ColorIndex = 7;
                    examplesSection.MenuItems = new ObservableCollection<MenuItemData>
                    {
                        new MenuItemData { Title = "Design Tokens Demo", Icon = "", Command = NavigateToDesignTokensDemoCommand, Route = "//DesignTokensDemo" },
                        new MenuItemData { Title = "DesktopAPI Demo", Icon = "", Command = NavigateToDesktopApiDemoCommand, Route = "//DesktopApiDemo" },
                        new MenuItemData { Title = "Responsive Layout", Icon = "", Command = NavigateToResponsiveLayoutDemoCommand, Route = "responsive-layout" }
                    };
                    System.Diagnostics.Debug.WriteLine($"? Examples section configured with {examplesSection.MenuItems.Count} items");
                }

                // ? REMOVED: AUTO-GENERATED PAGES SECTION - No longer generating XAML pages
                // The Generator now creates useful helpers instead:
                // - ServiceHelpers (optimized DesktopAPI communication)
                // - Mappers (DTO ? ViewModel conversion)
                // - ValidationHelpers (UI-friendly validation)
                // - Base ViewModels (common CRUD functionality)
                
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

        private async Task NavigateToConfiguration()
        {
            try
            {
                _logger?.LogInformation("Navigating to Configuration");
                // For now, show a placeholder - later this will be a real page
                await Shell.Current.DisplayAlert("Navigation", "Configuration page - Coming soon!", "OK");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Configuration");
            }
        }

        private async Task NavigateToGeneratePlan()
        {
            try
            {
                _logger?.LogInformation("Navigating to Generate Plan");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("generateplan");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Navigation", "Generate Plan page - Coming soon!", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Generate Plan");
            }
        }

        private async Task NavigateToPlanHistory()
        {
            try
            {
                _logger?.LogInformation("Navigating to Plan History");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("planhistory");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Navigation", "Plan History page - Coming soon!", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Plan History");
            }
        }

        private async Task NavigateToHome()
        {
            try
            {
                _logger?.LogInformation("Navigating to Home Demo");
                // Find the parent PrincipalPage and call its navigation method
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("home");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Navigation", "Could not find parent page", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Home");
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

        // Add all missing navigation methods
        private async Task NavigateToFoodCatalog()
        {
            try
            {
                _logger?.LogInformation("Navigating to Food Catalog");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("foodcatalog");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Food Catalog");
            }
        }

        private async Task NavigateToNutritionRules()
        {
            try
            {
                _logger?.LogInformation("Navigating to Nutrition Rules");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("nutritionrules");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Nutrition Rules");
            }
        }

        private async Task NavigateToSettings()
        {
            try
            {
                _logger?.LogInformation("Navigating to Settings");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("settings");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Navigation", "Settings - Unified configuration interface. Coming soon!", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Settings");
            }
        }

        private async Task NavigateToProfile()
        {
            try
            {
                _logger?.LogInformation("Navigating to Profile");
                await Shell.Current.DisplayAlert("Navigation", "Profile page - Coming soon!", "OK");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Profile");
            }
        }

        private async Task NavigateToDesktopApiDemo()
        {
            try
            {
                _logger?.LogInformation("Navigating to DesktopAPI Demo");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("desktopapidemo");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to DesktopAPI Demo");
            }
        }

        private async Task NavigateToDesignTokensDemo()
        {
            try
            {
                _logger?.LogInformation("Navigating to Design Tokens Demo");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("designtokensdemo");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Design Tokens Demo");
            }
        }

        private async Task NavigateToRealChartsDemo()
        {
            try
            {
                _logger?.LogInformation("Navigating to Real Charts Demo");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("realchartsdemo");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Real Charts Demo");
            }
        }

        private async Task NavigateToAnalyticsDashboard()
        {
            try
            {
                _logger?.LogInformation("Navigating to Analytics Dashboard");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("analyticsdashboard");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Analytics Dashboard");
            }
        }

        private async Task NavigateToResponsiveLayoutDemo()
        {
            try
            {
                _logger?.LogInformation("Navigating to Responsive Layout Demo");
                await Shell.Current.DisplayAlert("Navigation", "Responsive Layout Demo - Coming soon!", "OK");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Responsive Layout Demo");
            }
        }

        // Placeholder methods for all generated page navigation
        private async Task NavigateToUserManagement()
        {
            try
            {
                _logger?.LogInformation("Navigating to User Management");
                System.Diagnostics.Debug.WriteLine("?? NavigateToUserManagement called - Finding parent PrincipalPage");
                
                // Find the parent PrincipalPage and call its navigation method
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    System.Diagnostics.Debug.WriteLine("?? Found PrincipalPage, calling LoadContentForSection('usermanagement')");
                    principalPage.LoadContentForSection("usermanagement");
                    System.Diagnostics.Debug.WriteLine("? User Management navigation completed");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("? Could not find parent PrincipalPage");
                    await Shell.Current.DisplayAlert("Navigation", "Could not find parent page", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to User Management");
                System.Diagnostics.Debug.WriteLine($"? User Management navigation failed: {ex.Message}");
            }
        }

        private async Task NavigateToUserProfileList()
        {
            await Shell.Current.DisplayAlert("Navigation", "User Profile List - Coming soon!", "OK");
        }

        private async Task NavigateToGeneratedPlans()
        {
            try
            {
                _logger?.LogInformation("Navigating to Generated Plans");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("nutritionplanning");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Navigation", "Generated Plans - Coming soon!", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Generated Plans");
            }
        }

        private async Task NavigateToPlanDetails()
        {
            try
            {
                _logger?.LogInformation("Navigating to Plan Details");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("plandetails");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Navigation", "Plan Details - Coming soon!", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Plan Details");
            }
        }

        private async Task NavigateToWeeklyPlans()
        {
            await Shell.Current.DisplayAlert("Navigation", "Weekly Plans - Coming soon!", "OK");
        }

        private async Task NavigateToDailyPlans()
        {
            await Shell.Current.DisplayAlert("Navigation", "Daily Plans - Coming soon!", "OK");
        }

        private async Task NavigateToMeals()
        {
            await Shell.Current.DisplayAlert("Navigation", "Meals - Coming soon!", "OK");
        }

        private async Task NavigateToFoodPortions()
        {
            await Shell.Current.DisplayAlert("Navigation", "Food Portions - Coming soon!", "OK");
        }

        private async Task NavigateToShoppingLists()
        {
            await Shell.Current.DisplayAlert("Navigation", "Shopping Lists - Coming soon!", "OK");
        }

        private async Task NavigateToUserPreferences()
        {
            await Shell.Current.DisplayAlert("Navigation", "User Preferences - Coming soon!", "OK");
        }

        private async Task NavigateToClientComparison()
        {
            try
            {
                _logger?.LogInformation("Navigating to Client Comparison");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("MultiClientComparison");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Navigation", "Client Comparison - Coming soon!", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Client Comparison");
            }
        }

        // New analytics navigation methods
        private async Task NavigateToClientReports()
        {
            try
            {
                _logger?.LogInformation("Navigating to Client Reports");
                await Shell.Current.DisplayAlert("Navigation", "Client Reports - Advanced reporting and analytics for your clients. Coming soon!", "OK");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Client Reports");
            }
        }

        private async Task NavigateToPerformanceInsights()
        {
            try
            {
                _logger?.LogInformation("Navigating to Performance Insights");
                await Shell.Current.DisplayAlert("Navigation", "Performance Insights - Track client progress and performance metrics. Coming soon!", "OK");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Performance Insights");
            }
        }

        // System management navigation methods moved to configuration
        private async Task NavigateToLogManagement()
        {
            try
            {
                _logger?.LogInformation("Navigating to Log Management");
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection("log-management");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Navigation", "Log Management - Unified logging interface. Coming soon!", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to Log Management");
            }
        }

        /// <summary>
        /// Navigate to auto-generated pages (REAL NAVIGATION)
        /// </summary>
        private async Task NavigateToRealGeneratedPage(string pageName)
        {
            try
            {
                _logger?.LogInformation("Navigating to auto-generated page: {PageName}", pageName);
                
                // Navigate to the real generated page through PrincipalPage
                if (FindParentOfType<PrincipalPage>(this) is PrincipalPage principalPage)
                {
                    principalPage.LoadContentForSection(pageName.ToLower());
                }
                else
                {
                    // Fallback: show page info if navigation fails
                    var pageInfo = GetGeneratedPageInfo(pageName);
                    await Shell.Current.DisplayAlert("Auto-Generated Page", 
                        $"Page: {pageName}\n\n{pageInfo}", "OK");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to navigate to auto-generated page: {PageName}", pageName);
                await Shell.Current.DisplayAlert("Error", $"Failed to navigate to {pageName}", "OK");
            }
        }

        /// <summary>
        /// Get information about the generated page structure
        /// </summary>
        private string GetGeneratedPageInfo(string pageName)
        {
            return pageName switch
            {
                "FoodItemListPage" => "Lista de alimentos con campos:\n• Name, Subgroup, FoodGroup\n• KcalPer100, CarbsPer100\n• ProteinPer100, FatPer100\n• Allergens, IsActive",
                "FoodItemCreatePage" => "Formulario para crear alimentos\nCampos editables para todos los valores nutricionales",
                "FoodPortionListPage" => "Lista de porciones por subgrupo:\n• BreakfastGrams, LunchGrams\n• DinnerGrams, SnackGrams\n• DefaultGrams",
                "FoodPortionCreatePage" => "Formulario para configurar porciones\nPor tipo de comida y subgrupo",
                "GeneratedPlanListPage" => "Lista de planes generados:\n• Name, NumberOfWeeks\n• Include21DayReset, TotalDays\n• AverageKcal, IsArchived",
                "GeneratedPlanCreatePage" => "Formulario para crear planes\nConfiguración de semanas y parámetros",
                "PlanWeekListPage" => "Lista de semanas de planes\nEstructura jerárquica de planificación",
                "PlanDayListPage" => "Lista de días de planes\nDetalles diarios de comidas",
                "PlanMealListPage" => "Lista de comidas planificadas\nComidas específicas por día",
                "PlanMealIngredientListPage" => "Lista de ingredientes por comida\nDetalles nutricionales completos",
                "UserListPage" => "Lista de usuarios del sistema\nGestión de usuarios y perfiles",
                "UserCreatePage" => "Formulario de creación de usuarios\nDatos básicos y configuración inicial",
                "UserProfileListPage" => "Lista de perfiles de usuario\nInformación nutricional personal",
                "UserPreferencesListPage" => "Lista de preferencias de usuario\nConfiguración personalizada",
                "ShoppingListListPage" => "Lista de listas de compras\nGeneradas desde planes de comidas",
                "NutritionRulesListPage" => "Lista de reglas nutricionales\nConfiguración de restricciones dietéticas",
                "LogEntryListPage" => "Lista de entradas de log\nSistema de logging y debugging",
                "LogConfigurationListPage" => "Configuración de logging\nNiveles y destinos de logs",
                "LogSummaryListPage" => "Resúmenes de logs\nAnálisis y estadísticas de logs",
                _ => "Página autogenerada con CRUD completo\nCreate, Read, Update, Delete operations"
            };
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