using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CodeCraft.NET.MAUI.Views.Controls;

/// <summary>
/// Event arguments for expansion state changes
/// </summary>
public class ExpansionViewExpansionChangedEventArgs : EventArgs
{
    public bool IsExpanded { get; set; }
    public string Title { get; set; } = string.Empty;

    public ExpansionViewExpansionChangedEventArgs(bool isExpanded, string title)
    {
        IsExpanded = isExpanded;
        Title = title;
    }
}

/// <summary>
/// Expandable section control for menu navigation.
/// Provides collapsible sections with smooth expand/collapse functionality.
/// </summary>
public partial class ExpansionView : ContentView
{
    /// <summary>
    /// Event fired when the expansion state changes
    /// </summary>
    public event EventHandler<ExpansionViewExpansionChangedEventArgs>? ExpansionChanged;

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(ExpansionView),
        string.Empty);

    public static readonly BindableProperty IsExpandedProperty =
        BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(ExpansionView), true, BindingMode.TwoWay, propertyChanged: OnIsExpandedChanged);

    public static readonly BindableProperty MenuItemsProperty = BindableProperty.Create(
        nameof(MenuItems),
        typeof(ObservableCollection<MenuItemData>),
        typeof(ExpansionView),
        null,
        propertyChanged: OnMenuItemsChanged);

    public static readonly BindableProperty ColorBarColorProperty = BindableProperty.Create(
        nameof(ColorBarColor),
        typeof(Color),
        typeof(ExpansionView),
        Colors.Gray,
        propertyChanged: OnColorBarColorChanged);

    public static readonly BindableProperty ColorIndexProperty = BindableProperty.Create(
        nameof(ColorIndex),
        typeof(int),
        typeof(ExpansionView),
        0);

    public static readonly BindableProperty IsSingleItemProperty = BindableProperty.Create(
        nameof(IsSingleItem),
        typeof(bool),
        typeof(ExpansionView),
        false,
        propertyChanged: OnIsSingleItemChanged);

    public static readonly BindableProperty SingleItemCommandProperty = BindableProperty.Create(
        nameof(SingleItemCommand),
        typeof(ICommand),
        typeof(ExpansionView),
        null);

    /// <summary>
    /// Gets or sets the title of the expandable section.
    /// /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the section is expanded.
    /// </summary>
    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    /// <summary>
    /// Gets or sets the collection of menu items for this section.
    /// </summary>
    public ObservableCollection<MenuItemData> MenuItems
    {
        get => (ObservableCollection<MenuItemData>)GetValue(MenuItemsProperty);
        set => SetValue(MenuItemsProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the left color bar.
    /// </summary>
    public Color ColorBarColor
    {
        get => (Color)GetValue(ColorBarColorProperty);
        set => SetValue(ColorBarColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the color index for automatic color rotation in sub-items.
    /// </summary>
    public int ColorIndex
    {
        get => (int)GetValue(ColorIndexProperty);
        set => SetValue(ColorIndexProperty, value);
    }

    /// <summary>
    /// Gets or sets whether this is a single item section (hides expand icon and enables direct navigation).
    /// </summary>
    public bool IsSingleItem
    {
        get => (bool)GetValue(IsSingleItemProperty);
        set => SetValue(IsSingleItemProperty, value);  // ? CORRECTO - usando IsSingleItemProperty
    }

    /// <summary>
    /// Gets or sets the command to execute when clicking on a single item section.
    /// </summary>
    public ICommand? SingleItemCommand
    {
        get => (ICommand?)GetValue(SingleItemCommandProperty);
        set => SetValue(SingleItemCommandProperty, value);
    }

    /// <summary>
    /// Gets the icon text for expand/collapse state.
    /// For single items, returns empty string to hide the icon.
    /// </summary>
    public string ExpandIcon => IsSingleItem ? "" : (IsExpanded ? "—" : "+");

    /// <summary>
    /// Command to toggle the expanded state.
    /// </summary>
    public ICommand ToggleExpandedCommand { get; }

    public ExpansionView()
    {
        System.Diagnostics.Debug.WriteLine("?? ExpansionView constructor called");
        try
        {
            InitializeComponent();
            ToggleExpandedCommand = new Command(ToggleExpanded);
            MenuItems = new ObservableCollection<MenuItemData>();
            System.Diagnostics.Debug.WriteLine("?? ExpansionView initialized successfully");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"?? ERROR in ExpansionView constructor: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"?? Stack trace: {ex.StackTrace}");
        }
    }

    /// <summary>
    /// Toggles the expanded state of the section.
    /// </summary>
    private void ToggleExpanded()
    {
        System.Diagnostics.Debug.WriteLine($"?? ToggleExpanded called - Current: {IsExpanded}, New: {!IsExpanded}");
        System.Diagnostics.Debug.WriteLine($"?? Title: {Title}");
        IsExpanded = !IsExpanded;
        System.Diagnostics.Debug.WriteLine($"?? ToggleExpanded completed - New state: {IsExpanded}");
        OnPropertyChanged(nameof(ExpandIcon));

        // Raise the ExpansionChanged event
        ExpansionChanged?.Invoke(this, new ExpansionViewExpansionChangedEventArgs(IsExpanded, Title));
    }

    /// <summary>
    /// Event handler for header tap
    /// </summary>
    private void OnHeaderTapped(object sender, TappedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"?? OnHeaderTapped called for section: {Title}, IsSingleItem: {IsSingleItem}");
        
        if (IsSingleItem)
        {
            // For single items, execute the command directly instead of toggling expansion
            if (SingleItemCommand != null)
            {
                System.Diagnostics.Debug.WriteLine($"?? Executing single item command for: {Title}");
                if (SingleItemCommand.CanExecute(null))
                {
                    SingleItemCommand.Execute(null);
                }
            }
            // Do NOT toggle expansion for single items
        }
        else
        {
            // For multi-item sections, toggle expansion
            ToggleExpanded();
        }
    }

    /// <summary>
    /// Event handler for pointer entered (hover start)
    /// </summary>
    private void OnPointerEntered(object sender, PointerEventArgs e)
    {
        if (this.FindByName<Border>("HeaderBorder") is Border headerBorder)
        {
            // Try to get the hover color from resources, fallback to light gray
            var hoverColor = Colors.LightGray;
            if (Microsoft.Maui.Controls.Application.Current?.Resources.TryGetValue("MenuColorBackground", out var bgResource) == true && bgResource is Color bgColor)
            {
                hoverColor = bgColor;
            }
            headerBorder.BackgroundColor = hoverColor;
        }
    }

    /// <summary>
    /// Event handler for pointer exited (hover end)
    /// </summary>
    private void OnPointerExited(object sender, PointerEventArgs e)
    {
        if (this.FindByName<Border>("HeaderBorder") is Border headerBorder)
        {
            headerBorder.BackgroundColor = Colors.Transparent; // Back to transparent
        }
    }

    /// <summary>
    /// Handles changes to the IsExpanded property.
    /// </summary>
    private static void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ExpansionView view)
        {
            System.Diagnostics.Debug.WriteLine($"?? IsExpanded property changed: {oldValue} -> {newValue}");
            view.OnPropertyChanged(nameof(ExpandIcon));
            view.OnPropertyChanged(nameof(IsContentVisible));
            
            // Fire the ExpansionChanged event for accordion behavior
            if (newValue is bool isExpanded)
            {
                view.ExpansionChanged?.Invoke(view, new ExpansionViewExpansionChangedEventArgs(isExpanded, view.Title));
            }
        }
    }

    /// <summary>
    /// Handles changes to the MenuItems collection.
    /// </summary>
    private static void OnMenuItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ExpansionView expansionView && newValue is ObservableCollection<MenuItemData> menuItems)
        {
            expansionView.UpdateMenuItems(menuItems);
        }
    }

    /// <summary>
    /// Animates the content area when expanding/collapsing.
    /// </summary>
    private async void AnimateContent(bool isExpanding)
    {
        var contentContainer = this.FindByName<StackLayout>("ContentContainer");
        if (contentContainer != null)
        {
            if (isExpanding)
            {
                contentContainer.IsVisible = true;
                contentContainer.Opacity = 0;
                await contentContainer.FadeTo(1, 200, Easing.CubicInOut);
            }
            else
            {
                await contentContainer.FadeTo(0, 150, Easing.CubicInOut);
                contentContainer.IsVisible = false;
            }
        }
    }

    /// <summary>
    /// Updates the visual menu items when the collection changes.
    /// </summary>
    private void UpdateMenuItems(ObservableCollection<MenuItemData> menuItems)
    {
        var contentContainer = this.FindByName<StackLayout>("ContentContainer");
        if (contentContainer == null) return;

        contentContainer.Children.Clear();

        for (int i = 0; i < menuItems.Count; i++)
        {
            var menuItem = menuItems[i];
            var itemIndex = ColorIndex + i + 1; // Start from parent color index + 1
            var menuItemView = CreateMenuItemView(menuItem, itemIndex);
            contentContainer.Children.Add(menuItemView);
        }
    }

    /// <summary>
    /// Creates a visual menu item view from menu item data with rotating color.
    /// </summary>
    private View CreateMenuItemView(MenuItemData menuItem, int colorIndex)
    {
        var border = new Border();
        border.SetDynamicResource(StyleProperty, "NavigationItemBorderStyle");

        var tapGesture = new TapGestureRecognizer();
        if (menuItem.Command != null)
        {
            tapGesture.Command = menuItem.Command;
            tapGesture.CommandParameter = menuItem.CommandParameter;
        }
        border.GestureRecognizers.Add(tapGesture);

        // Add hover effect
        var pointerGesture = new PointerGestureRecognizer();
        pointerGesture.PointerEntered += (s, e) => {
            var hoverColor = Colors.LightGray;
            if (Microsoft.Maui.Controls.Application.Current?.Resources.TryGetValue("MenuColorBackground", out var bgResource) == true && bgResource is Color bgColor)
            {
                hoverColor = bgColor;
            }
            border.BackgroundColor = hoverColor;
        };
        pointerGesture.PointerExited += (s, e) => border.BackgroundColor = Colors.Transparent;
        border.GestureRecognizers.Add(pointerGesture);

        var grid = new Grid
        {
            ColumnDefinitions = 
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star)
            },
            ColumnSpacing = 12,
            Padding = new Thickness(16, 8)
        };

        // Color bar indicator with rotating color
        var itemColor = GetRotatingColor(colorIndex);
        var colorBar = new Border
        {
            WidthRequest = 3,
            HeightRequest = 16,
            BackgroundColor = itemColor,
            VerticalOptions = LayoutOptions.Center
        };
        Grid.SetColumn(colorBar, 0);

        // Title
        var titleLabel = new Label
        {
            Text = menuItem.Title,
            VerticalOptions = LayoutOptions.Center,
            FontSize = 14,
            Padding = new Thickness(8, 4)
        };
        titleLabel.SetDynamicResource(StyleProperty, "NavigationLabelStyle");
        Grid.SetColumn(titleLabel, 1);

        grid.Children.Add(colorBar);
        grid.Children.Add(titleLabel);
        border.Content = grid;

        return border;
    }

    private static void OnColorBarColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ExpansionView view && view.FindByName<Border>("ColorBar") is Border colorBar && newValue is Color color)
        {
            colorBar.BackgroundColor = color;
        }
    }

    /// <summary>
    /// Gets a color from the rotating palette based on index (1-9, then loops back to 1).
    /// </summary>
    private Color GetRotatingColor(int index)
    {
        var colorIndex = ((index - 1) % 9) + 1; // Ensures rotation from 1-9
        var resourceKey = $"MenuColor{colorIndex}";
        
        try
        {
            if (Microsoft.Maui.Controls.Application.Current?.Resources.TryGetValue(resourceKey, out var colorResource) == true && colorResource is Color color)
            {
                return color;
            }
        }
        catch
        {
            // Fallback if resource not found
        }
        
        // Fallback colors if resource lookup fails
        var fallbackColors = new Color[]
        {
            Color.FromArgb("#5cb6df"), // MenuColor1
            Color.FromArgb("#ffdb5c"), // MenuColor2
            Color.FromArgb("#c95555"), // MenuColor3
            Color.FromArgb("#5cbd6e"), // MenuColor4
            Color.FromArgb("#b84c82"), // MenuColor5
            Color.FromArgb("#ff915c"), // MenuColor6
            Color.FromArgb("#b4eb5c"), // MenuColor7
            Color.FromArgb("#ff915c"), // MenuColor8
            Color.FromArgb("#84659d")  // MenuColor9
        };
        
        return fallbackColors[(colorIndex - 1) % fallbackColors.Length];
    }

    private static void OnIsSingleItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ExpansionView view)
        {
            System.Diagnostics.Debug.WriteLine($"?? IsSingleItem changed for {view.Title}: {oldValue} -> {newValue}");
            
            // Force UI updates
            view.OnPropertyChanged(nameof(ExpandIcon));
            view.OnPropertyChanged(nameof(IsContentVisible));
            
            // Force icon label update by finding it and updating manually
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var iconLabel = view.FindByName<Label>("ExpandIconLabel");
                    if (iconLabel != null)
                    {
                        iconLabel.Text = view.ExpandIcon;
                        System.Diagnostics.Debug.WriteLine($"?? Force updated icon label text to: '{view.ExpandIcon}'");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"?? Could not find icon label: {ex.Message}");
                }
            });
            
            System.Diagnostics.Debug.WriteLine($"? IsSingleItem update completed - ExpandIcon: '{view.ExpandIcon}', IsContentVisible: {view.IsContentVisible}");
        }
    }

    /// <summary>
    /// Gets whether the content area should be visible.
    /// For single items, content is never visible. For multi-items, visible when expanded.
    /// </summary>
    public bool IsContentVisible => !IsSingleItem && IsExpanded;
}

/// <summary>
/// Data class for menu item information.
/// </summary>
public class MenuItemData
{
    /// <summary>
    /// Gets or sets the display title of the menu item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the icon text for the menu item.
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the command to execute when the item is tapped.
    /// </summary>
    public ICommand? Command { get; set; }

    /// <summary>
    /// Gets or sets the parameter to pass with the command.
    /// </summary>
    public object? CommandParameter { get; set; }

    /// <summary>
    /// Gets or sets the navigation route for this menu item.
    /// </summary>
    public string Route { get; set; } = string.Empty;
}