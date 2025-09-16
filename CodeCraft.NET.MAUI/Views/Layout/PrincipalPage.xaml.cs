using Microsoft.Extensions.Logging;

namespace CodeCraft.NET.MAUI.Views.Layout
{
    /// <summary>
    /// Principal page with responsive sidebar navigation and dynamic content area
    /// All pages load in the content area to maintain consistent navigation
    /// </summary>
    public partial class PrincipalPage : ContentPage
    {
        private readonly ILogger<PrincipalPage> _logger;
        private readonly IServiceProvider _serviceProvider;
        private bool _isMobileMode = false;
        private bool _isSidebarVisible = true;

        public PrincipalPage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILogger<PrincipalPage>>();
            InitializeComponent();
            
            // Initialize responsive behavior
            SizeChanged += OnSizeChanged;
            
            // Set initial state
            SetInitialState();
        }

        /// <summary>
        /// Set initial state based on screen size
        /// </summary>
        private void SetInitialState()
        {
            // Check if we're in mobile mode initially
            CheckMobileMode();
            
            // Load default Dashboard content
            LoadDashboardContent();
            
            _logger?.LogInformation("PrincipalPage initialized - IsMobile: {IsMobile}", _isMobileMode);
        }

        /// <summary>
        /// Handle size changes for responsive behavior
        /// </summary>
        private void OnSizeChanged(object sender, EventArgs e)
        {
            CheckMobileMode();
        }

        /// <summary>
        /// Check if we're in mobile mode and update layout accordingly
        /// </summary>
        private void CheckMobileMode()
        {
            if (Width > 0)
            {
                var wasMobileMode = _isMobileMode;
                _isMobileMode = Width < 1024; // Desktop breakpoint
                
                if (wasMobileMode != _isMobileMode)
                {
                    UpdateLayoutForScreenSize();
                    _logger?.LogInformation("Screen mode changed - Width: {Width}, IsMobile: {IsMobile}", Width, _isMobileMode);
                }
            }
        }

        /// <summary>
        /// Update layout based on screen size
        /// </summary>
        private void UpdateLayoutForScreenSize()
        {
            if (_isMobileMode)
            {
                // Mobile: Hide sidebar by default, show hamburger
                HamburgerMenuButton.IsVisible = true;
                SidebarColumn.Width = new GridLength(0);
                SidebarBorder.IsVisible = false;
                _isSidebarVisible = false;
                MobileOverlay.IsVisible = false;
            }
            else
            {
                // Desktop: Always show sidebar, hide hamburger
                HamburgerMenuButton.IsVisible = false;
                SidebarColumn.Width = new GridLength(280);
                SidebarBorder.IsVisible = true;
                MobileOverlay.IsVisible = false;
                _isSidebarVisible = true;
            }
        }

        /// <summary>
        /// Handle hamburger menu click
        /// </summary>
        private void OnHamburgerMenuClicked(object sender, EventArgs e)
        {
            if (_isMobileMode)
            {
                _isSidebarVisible = !_isSidebarVisible;
                
                if (_isSidebarVisible)
                {
                    // Show mobile overlay with sidebar
                    MobileOverlay.IsVisible = true;
                }
                else
                {
                    // Hide mobile overlay
                    MobileOverlay.IsVisible = false;
                }
                
                _logger?.LogInformation("Hamburger menu clicked - Sidebar visible: {IsVisible}", _isSidebarVisible);
            }
        }

        /// <summary>
        /// Handle mobile overlay tap (close sidebar)
        /// </summary>
        private void OnMobileOverlayTapped(object sender, TappedEventArgs e)
        {
            if (_isMobileMode && _isSidebarVisible)
            {
                _isSidebarVisible = false;
                MobileOverlay.IsVisible = false;
                
                _logger?.LogInformation("Mobile overlay tapped - Sidebar closed");
            }
        }

        /// <summary>
        /// Handle navigation item tapped
        /// </summary>
        private void OnNavigationItemTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter?.ToString() is string sectionName)
            {
                // Close sidebar in mobile mode
                if (_isMobileMode && _isSidebarVisible)
                {
                    _isSidebarVisible = false;
                    MobileOverlay.IsVisible = false;
                }
                
                // Load content for the selected section
                LoadContentForSection(sectionName);
                
                _logger?.LogInformation("Navigation to section: {Section}", sectionName);
            }
        }

        /// <summary>
        /// Load content for the selected section
        /// </summary>
        public void LoadContentForSection(string sectionName)
        {
            _logger?.LogInformation("LoadContentForSection called with: {SectionName}", sectionName);
            
            switch (sectionName.ToLowerInvariant())
            {
                case "dashboard":
                    LoadDashboardContent();
                    _logger?.LogInformation("Loaded Dashboard content");
                    break;
                case "configuration":
                    LoadConfigurationContent();
                    _logger?.LogInformation("Loaded Configuration content");
                    break;
                default:
                    LoadPlaceholderContent(sectionName);
                    break;
            }
        }

        /// <summary>
        /// Load Dashboard content
        /// </summary>
        private void LoadDashboardContent()
        {
            var content = new ScrollView
            {
                Content = new StackLayout
                {
                    Spacing = 24,
                    Children = {
                        new StackLayout
                        {
                            Spacing = 8,
                            Children = {
                                new Label
                                {
                                    Text = "Dashboard",
                                    FontFamily = "PrimaryFontSemibold",
                                    FontSize = 28,
                                    TextColor = Color.FromArgb("#1F2937")
                                },
                                new Label
                                {
                                    Text = "Overview and quick actions",
                                    FontFamily = "PrimaryFont",
                                    FontSize = 16,
                                    TextColor = Color.FromArgb("#6B7280")
                                }
                            }
                        },
						CreatePageHeader("Dashboard content", "Configure relevant data"),
						CreatePlaceholderCard("Dashboard", "Set up relevant information.")
					}
                }
            };
            
            DynamicContentArea.Content = content;
        }

        /// <summary>
        /// Load Configuration content
        /// </summary>
        private void LoadConfigurationContent()
        {
            var content = new ScrollView
            {
                Content = new StackLayout
                {
                    Spacing = 24,
                    Children = {
                        CreatePageHeader("Configuration", "Configure data"),
                        CreatePlaceholderCard("Configuration", "Set up your personal information.")
                    }
                }
            };
            
            DynamicContentArea.Content = content;
        }

        /// <summary>
        /// Load placeholder content for unknown sections
        /// </summary>
        private void LoadPlaceholderContent(string sectionName)
        {
            var content = new ScrollView
            {
                Content = new StackLayout
                {
                    Spacing = 24,
                    Children = {
                        CreatePageHeader(sectionName, "Section information"),
                        CreatePlaceholderCard($"{sectionName} Section", $"Content for the {sectionName} section will be implemented in future iterations.")
                    }
                }
            };
            
            DynamicContentArea.Content = content;
        }

        /// <summary>
        /// Load error content when page fails to load
        /// </summary>
        private void LoadErrorContent(string sectionName, string errorMessage)
        {
            var content = new ScrollView
            {
                Content = new StackLayout
                {
                    Spacing = 24,
                    Children = {
                        CreatePageHeader($"Error Loading {sectionName}", "Failed to load page content"),
                        CreateErrorCard("Loading Error", $"Failed to load {sectionName}: {errorMessage}")
                    }
                }
            };
            
            DynamicContentArea.Content = content;
        }

        /// <summary>
        /// Create page header
        /// </summary>
        private StackLayout CreatePageHeader(string title, string subtitle)
        {
            return new StackLayout
            {
                Spacing = 8,
                Children = {
                    new Label
                    {
                        Text = title,
                        FontFamily = "PrimaryFontSemibold",
                        FontSize = 28,
                        TextColor = Color.FromArgb("#1F2937")
                    },
                    new Label
                    {
                        Text = subtitle,
                        FontFamily = "PrimaryFont",
                        FontSize = 16,
                        TextColor = Color.FromArgb("#6B7280")
                    }
                }
            };
        }

        /// <summary>
        /// Create placeholder card
        /// </summary>
        private Border CreatePlaceholderCard(string title, string description)
        {
            return new Border
            {
                BackgroundColor = Colors.White,
                Stroke = new SolidColorBrush(Colors.LightGray),
                //CornerRadius = 8,
                Padding = new Thickness(20),
                Margin = new Thickness(0),
                Content = new StackLayout
                {
                    Spacing = 12,
                    Children = {
                        new Label
                        {
                            Text = title,
                            FontFamily = "PrimaryFontSemibold",
                            FontSize = 20,
                            TextColor = Color.FromArgb("#1F2937")
                        },
                        new Label
                        {
                            Text = description,
                            FontSize = 16,
                            TextColor = Color.FromArgb("#6B7280")
                        }
                    }
                }
            };
        }

		/// <summary>
		/// Create error card
		/// </summary>
		private Frame CreateErrorCard(string title, string description)
		{
			return new Frame
			{
				BackgroundColor = Color.FromArgb("#FEF2F2"),
				BorderColor = Color.FromArgb("#FECACA"),
				HasShadow = false,
				CornerRadius = 8,
				Padding = new Thickness(20),
				Margin = new Thickness(0),
				Content = new StackLayout
				{
					Spacing = 12,
					Children = {
						new Label
						{
							Text = title,
							FontFamily = "PrimaryFontSemibold",
							FontSize = 20,
							TextColor = Color.FromArgb("#991B1B")
						},
						new Label
						{
							Text = description,
							FontSize = 16,
							TextColor = Color.FromArgb("#7F1D1D")
						}
					}
				}
			};
		}

		/// <summary>
		/// Handle settings clicked
		/// </summary>
		private void OnSettingsClicked(object sender, EventArgs e)
        {
            // Redirect to sidebar menu instead
            LoadContentForSection("Settings");
            
            _logger?.LogInformation("Settings clicked - redirected to sidebar");
        }

        /// <summary>
        /// Handle profile menu tapped
        /// </summary>
        private void OnProfileMenuTapped(object sender, TappedEventArgs e)
        {
            // Redirect to sidebar menu instead
            LoadContentForSection("Profile");
            
            _logger?.LogInformation("Profile menu tapped - redirected to sidebar");
        }
    }
}