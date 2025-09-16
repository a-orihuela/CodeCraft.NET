using System.Collections.ObjectModel;

namespace CodeCraft.NET.MAUI.Views.Controls;

/// <summary>
/// Reusable user profile edit control that can be used for both current user profile and other users
/// </summary>
public partial class UserProfileEditControl : ContentView
{
    public static readonly BindableProperty UserIdProperty =
        BindableProperty.Create(nameof(UserId), typeof(int?), typeof(UserProfileEditControl), null, propertyChanged: OnUserIdChanged);

    public static readonly BindableProperty IsCurrentUserProperty =
        BindableProperty.Create(nameof(IsCurrentUser), typeof(bool), typeof(UserProfileEditControl), false, propertyChanged: OnIsCurrentUserChanged);

    public static readonly BindableProperty ShowDeleteButtonProperty =
        BindableProperty.Create(nameof(ShowDeleteButton), typeof(bool), typeof(UserProfileEditControl), false, propertyChanged: OnShowDeleteButtonChanged);

    /// <summary>
    /// Event fired when save button is clicked
    /// </summary>
    public event EventHandler<UserProfileSaveEventArgs>? SaveRequested;

    /// <summary>
    /// Event fired when delete button is clicked
    /// </summary>
    public event EventHandler<UserProfileDeleteEventArgs>? DeleteRequested;

    public int? UserId
    {
        get => (int?)GetValue(UserIdProperty);
        set => SetValue(UserIdProperty, value);
    }

    public bool IsCurrentUser
    {
        get => (bool)GetValue(IsCurrentUserProperty);
        set => SetValue(IsCurrentUserProperty, value);
    }

    public bool ShowDeleteButton
    {
        get => (bool)GetValue(ShowDeleteButtonProperty);
        set => SetValue(ShowDeleteButtonProperty, value);
    }

    private UserProfileData? _currentUserData;

    public UserProfileEditControl()
    {
        InitializeComponent();
        InitializeDefaults();
    }

    /// <summary>
    /// Initialize default values
    /// </summary>
    private void InitializeDefaults()
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            try
            {
                // Set default selections using FindByName
                var genderPicker = this.FindByName<Picker>("GenderPicker");
                var activityLevelPicker = this.FindByName<Picker>("ActivityLevelPicker");
                var goalPicker = this.FindByName<Picker>("GoalPicker");
                var timeZonePicker = this.FindByName<Picker>("TimeZonePicker");
                var languagePicker = this.FindByName<Picker>("LanguagePicker");
                var themePicker = this.FindByName<Picker>("ThemePicker");
                
                if (genderPicker != null) genderPicker.SelectedIndex = 0;
                if (activityLevelPicker != null) activityLevelPicker.SelectedIndex = 2;
                if (goalPicker != null) goalPicker.SelectedIndex = 0;
                if (timeZonePicker != null) timeZonePicker.SelectedIndex = 2;
                if (languagePicker != null) languagePicker.SelectedIndex = 0;
                if (themePicker != null) themePicker.SelectedIndex = 0;

                System.Diagnostics.Debug.WriteLine("? UserProfileEditControl defaults initialized");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"?? Error initializing defaults: {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Handle user ID change
    /// </summary>
    private static void OnUserIdChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is UserProfileEditControl control && newValue is int userId)
        {
            control.LoadUserData(userId);
        }
    }

    /// <summary>
    /// Handle current user flag change
    /// </summary>
    private static void OnIsCurrentUserChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is UserProfileEditControl control && newValue is bool isCurrentUser)
        {
            control.UpdateUIForUserType(isCurrentUser);
        }
    }

    /// <summary>
    /// Handle delete button visibility change
    /// </summary>
    private static void OnShowDeleteButtonChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is UserProfileEditControl control && newValue is bool showDelete)
        {
            var deleteButton = control.FindByName<Button>("DeleteButton");
            if (deleteButton != null)
                deleteButton.IsVisible = showDelete;
        }
    }

    /// <summary>
    /// Update UI based on whether this is the current user or another user
    /// </summary>
    private void UpdateUIForUserType(bool isCurrentUser)
    {
        var appPreferencesFrame = this.FindByName<Frame>("AppPreferencesFrame");
        var saveButton = this.FindByName<Button>("SaveButton");
        var deleteButton = this.FindByName<Button>("DeleteButton");
        
        if (appPreferencesFrame != null)
            appPreferencesFrame.IsVisible = isCurrentUser;

        if (saveButton != null)
            saveButton.Text = isCurrentUser ? "Save Profile & Preferences" : "Save User Changes";

        if (deleteButton != null)
            deleteButton.IsVisible = !isCurrentUser && ShowDeleteButton;
    }

    /// <summary>
    /// Load user data based on user ID
    /// </summary>
    private void LoadUserData(int userId)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"?? Loading user data for ID: {userId}");

            if (userId == 1) // Current user (John Doe)
            {
                LoadCurrentUserData();
            }
            else
            {
                LoadOtherUserData(userId);
            }

            System.Diagnostics.Debug.WriteLine($"? User data loaded successfully for ID: {userId}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"? Error loading user data: {ex.Message}");
        }
    }

    /// <summary>
    /// Load current user data (John Doe)
    /// </summary>
    private void LoadCurrentUserData()
    {
        _currentUserData = new UserProfileData
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john.doe@example.com",
            Phone = "+1 (555) 123-4567",
            DateOfBirth = new DateTime(1985, 6, 15),
            Role = "Administrator",
            Initials = "JD",
            Gender = "M",
            Age = 38,
            Weight = 75.5m,
            Height = 178,
            ActivityLevel = "Moderately Active",
            Goal = "Weight Loss",
            TimeZone = "UTC-05:00 (Eastern Time)",
            Language = "en-US (English)",
            Allergens = "Nuts, Shellfish",
            ExcludedSubgroups = "Processed Meats",
            ExcludedFoods = "Mushrooms, Liver",
            FavoriteFoods = "Chicken, Salmon, Avocado, Broccoli",
            DislikedFoods = "Brussels Sprouts, Olives",
            Theme = "Light",
            NotificationsEnabled = true
        };

        LoadDataIntoForm(_currentUserData);
    }

    /// <summary>
    /// Load other user data (with generated sample data)
    /// </summary>
    private void LoadOtherUserData(int userId)
    {
        // Generate sample data based on user ID
        var userData = GenerateSampleUserData(userId);
        _currentUserData = userData;
        LoadDataIntoForm(userData);
    }

    /// <summary>
    /// Generate sample user data based on ID
    /// </summary>
    private UserProfileData GenerateSampleUserData(int userId)
    {
        var names = new[] { "Jane Smith", "Mike Johnson", "Sarah Wilson", "David Brown", "Emily Davis" };
        var emails = new[] { "jane.smith@example.com", "mike.johnson@example.com", "sarah.wilson@example.com", "david.brown@example.com", "emily.davis@example.com" };
        var roles = new[] { "User", "Administrator", "Viewer", "User", "User" };
        var initials = new[] { "JS", "MJ", "SW", "DB", "ED" };

        var index = (userId - 2) % names.Length; // userId 2-6 maps to index 0-4

        return new UserProfileData
        {
            Id = userId,
            FullName = names[index],
            Email = emails[index],
            Phone = GenerateSamplePhone(userId),
            DateOfBirth = GenerateSampleDateOfBirth(userId),
            Role = roles[index],
            Initials = initials[index],
            Gender = (userId % 2 == 0) ? "M" : "F",
            Age = 25 + (userId * 3),
            Weight = 65 + (userId * 2.5m),
            Height = 165 + (userId * 3),
            ActivityLevel = new[] { "Sedentary", "Lightly Active", "Moderately Active", "Very Active", "Extra Active" }[userId % 5],
            Goal = new[] { "Weight Loss", "Weight Gain", "Maintenance", "Muscle Gain", "Fat Loss" }[userId % 5],
            TimeZone = "UTC-05:00 (Eastern Time)",
            Language = "en-US (English)",
            Allergens = new[] { "None", "Nuts", "Dairy", "Gluten", "Shellfish" }[userId % 5],
            ExcludedSubgroups = new[] { "None", "Red Meat", "Processed Foods", "Dairy Products", "Grains" }[userId % 5],
            ExcludedFoods = new[] { "None", "Mushrooms", "Liver", "Spinach", "Broccoli" }[userId % 5],
            FavoriteFoods = new[] { "Chicken, Rice", "Salmon, Vegetables", "Beef, Potatoes", "Fish, Salad", "Turkey, Quinoa" }[userId % 5],
            DislikedFoods = new[] { "None", "Brussels Sprouts", "Olives", "Anchovies", "Blue Cheese" }[userId % 5],
            Theme = "Light",
            NotificationsEnabled = true
        };
    }

    /// <summary>
    /// Load data into form controls
    /// </summary>
    private void LoadDataIntoForm(UserProfileData userData)
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            try
            {
                // Update header using FindByName
                var userInitialsLabel = this.FindByName<Label>("UserInitialsLabel");
                var userNameLabel = this.FindByName<Label>("UserNameLabel");
                var userEmailLabel = this.FindByName<Label>("UserEmailLabel");
                var userRoleLabel = this.FindByName<Label>("UserRoleLabel");
                
                if (userInitialsLabel != null) userInitialsLabel.Text = userData.Initials;
                if (userNameLabel != null) userNameLabel.Text = userData.FullName;
                if (userEmailLabel != null) userEmailLabel.Text = userData.Email;
                if (userRoleLabel != null) userRoleLabel.Text = userData.Role;

                // Update basic information using FindByName
                var fullNameEntry = this.FindByName<Entry>("FullNameEntry");
                var emailEntry = this.FindByName<Entry>("EmailEntry");
                var phoneEntry = this.FindByName<Entry>("PhoneEntry");
                var dateOfBirthPicker = this.FindByName<DatePicker>("DateOfBirthPicker");
                
                if (fullNameEntry != null) fullNameEntry.Text = userData.FullName;
                if (emailEntry != null) emailEntry.Text = userData.Email;
                if (phoneEntry != null) phoneEntry.Text = userData.Phone;
                if (dateOfBirthPicker != null) dateOfBirthPicker.Date = userData.DateOfBirth;

                // Update physical profile using FindByName
                var genderPicker = this.FindByName<Picker>("GenderPicker");
                var ageEntry = this.FindByName<Entry>("AgeEntry");
                var weightEntry = this.FindByName<Entry>("WeightEntry");
                var heightEntry = this.FindByName<Entry>("HeightEntry");
                var activityLevelPicker = this.FindByName<Picker>("ActivityLevelPicker");
                var goalPicker = this.FindByName<Picker>("GoalPicker");
                var timeZonePicker = this.FindByName<Picker>("TimeZonePicker");
                var languagePicker = this.FindByName<Picker>("LanguagePicker");
                
                if (genderPicker != null) SetPickerValue(genderPicker, userData.Gender);
                if (ageEntry != null) ageEntry.Text = userData.Age.ToString();
                if (weightEntry != null) weightEntry.Text = userData.Weight.ToString("F1");
                if (heightEntry != null) heightEntry.Text = userData.Height.ToString();
                if (activityLevelPicker != null) SetPickerValue(activityLevelPicker, userData.ActivityLevel);
                if (goalPicker != null) SetPickerValue(goalPicker, userData.Goal);
                if (timeZonePicker != null) SetPickerValue(timeZonePicker, userData.TimeZone);
                if (languagePicker != null) SetPickerValue(languagePicker, userData.Language);

                // Update food preferences using FindByName
                var allergensEntry = this.FindByName<Entry>("AllergensEntry");
                var excludedSubgroupsEntry = this.FindByName<Entry>("ExcludedSubgroupsEntry");
                var excludedFoodsEntry = this.FindByName<Entry>("ExcludedFoodsEntry");
                var favoriteFoodsEntry = this.FindByName<Entry>("FavoriteFoodsEntry");
                var dislikedFoodsEntry = this.FindByName<Entry>("DislikedFoodsEntry");
                
                if (allergensEntry != null) allergensEntry.Text = userData.Allergens;
                if (excludedSubgroupsEntry != null) excludedSubgroupsEntry.Text = userData.ExcludedSubgroups;
                if (excludedFoodsEntry != null) excludedFoodsEntry.Text = userData.ExcludedFoods;
                if (favoriteFoodsEntry != null) favoriteFoodsEntry.Text = userData.FavoriteFoods;
                if (dislikedFoodsEntry != null) dislikedFoodsEntry.Text = userData.DislikedFoods;

                // Update app preferences (only for current user) using FindByName
                if (IsCurrentUser)
                {
                    var themePicker = this.FindByName<Picker>("ThemePicker");
                    var notificationsSwitch = this.FindByName<Switch>("NotificationsSwitch");
                    
                    if (themePicker != null) SetPickerValue(themePicker, userData.Theme);
                    if (notificationsSwitch != null) notificationsSwitch.IsToggled = userData.NotificationsEnabled;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error loading data into form: {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Set picker value by finding matching item
    /// </summary>
    private void SetPickerValue(Picker picker, string value)
    {
        if (picker?.Items != null && !string.IsNullOrEmpty(value))
        {
            for (int i = 0; i < picker.Items.Count; i++)
            {
                if (picker.Items[i].Contains(value) || picker.Items[i] == value)
                {
                    picker.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Generate sample phone number
    /// </summary>
    private string GenerateSamplePhone(int userId)
    {
        var baseNumber = 5551000 + (userId * 123);
        var numStr = baseNumber.ToString();
        return $"+1 ({numStr.Substring(0, 3)}) {numStr.Substring(3, 3)}-{numStr.Substring(6)}";
    }

    /// <summary>
    /// Generate sample date of birth
    /// </summary>
    private DateTime GenerateSampleDateOfBirth(int userId)
    {
        var baseYear = 1990 - (userId * 2);
        var month = (userId % 12) + 1;
        var day = (userId % 28) + 1;
        return new DateTime(baseYear, month, day);
    }

    /// <summary>
    /// Collect current form data
    /// </summary>
    public UserProfileData CollectFormData()
    {
        var fullNameEntry = this.FindByName<Entry>("FullNameEntry");
        var emailEntry = this.FindByName<Entry>("EmailEntry");
        var phoneEntry = this.FindByName<Entry>("PhoneEntry");
        var dateOfBirthPicker = this.FindByName<DatePicker>("DateOfBirthPicker");
        var genderPicker = this.FindByName<Picker>("GenderPicker");
        var ageEntry = this.FindByName<Entry>("AgeEntry");
        var weightEntry = this.FindByName<Entry>("WeightEntry");
        var heightEntry = this.FindByName<Entry>("HeightEntry");
        var activityLevelPicker = this.FindByName<Picker>("ActivityLevelPicker");
        var goalPicker = this.FindByName<Picker>("GoalPicker");
        var timeZonePicker = this.FindByName<Picker>("TimeZonePicker");
        var languagePicker = this.FindByName<Picker>("LanguagePicker");
        var allergensEntry = this.FindByName<Entry>("AllergensEntry");
        var excludedSubgroupsEntry = this.FindByName<Entry>("ExcludedSubgroupsEntry");
        var excludedFoodsEntry = this.FindByName<Entry>("ExcludedFoodsEntry");
        var favoriteFoodsEntry = this.FindByName<Entry>("FavoriteFoodsEntry");
        var dislikedFoodsEntry = this.FindByName<Entry>("DislikedFoodsEntry");
        var themePicker = this.FindByName<Picker>("ThemePicker");
        var notificationsSwitch = this.FindByName<Switch>("NotificationsSwitch");
        
        var data = new UserProfileData
        {
            Id = UserId ?? 0,
            FullName = fullNameEntry?.Text ?? "",
            Email = emailEntry?.Text ?? "",
            Phone = phoneEntry?.Text ?? "",
            DateOfBirth = dateOfBirthPicker?.Date ?? DateTime.Now,
            Role = _currentUserData?.Role ?? "User",
            Initials = GetInitials(fullNameEntry?.Text ?? ""),
            Gender = genderPicker?.SelectedItem?.ToString() ?? "",
            Age = int.TryParse(ageEntry?.Text, out var age) ? age : 0,
            Weight = decimal.TryParse(weightEntry?.Text, out var weight) ? weight : 0,
            Height = int.TryParse(heightEntry?.Text, out var height) ? height : 0,
            ActivityLevel = activityLevelPicker?.SelectedItem?.ToString() ?? "",
            Goal = goalPicker?.SelectedItem?.ToString() ?? "",
            TimeZone = timeZonePicker?.SelectedItem?.ToString() ?? "",
            Language = languagePicker?.SelectedItem?.ToString() ?? "",
            Allergens = allergensEntry?.Text ?? "",
            ExcludedSubgroups = excludedSubgroupsEntry?.Text ?? "",
            ExcludedFoods = excludedFoodsEntry?.Text ?? "",
            FavoriteFoods = favoriteFoodsEntry?.Text ?? "",
            DislikedFoods = dislikedFoodsEntry?.Text ?? "",
            Theme = themePicker?.SelectedItem?.ToString() ?? "Light",
            NotificationsEnabled = notificationsSwitch?.IsToggled ?? false
        };

        return data;
    }

    /// <summary>
    /// Generate initials from full name
    /// </summary>
    private string GetInitials(string fullName)
    {
        if (string.IsNullOrEmpty(fullName))
            return "U";

        var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
            return parts[0].Substring(0, 1).ToUpper();

        return (parts[0].Substring(0, 1) + parts[^1].Substring(0, 1)).ToUpper();
    }

    /// <summary>
    /// Handle save button click
    /// </summary>
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            var formData = CollectFormData();
            var eventArgs = new UserProfileSaveEventArgs(formData, IsCurrentUser);
            SaveRequested?.Invoke(this, eventArgs);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"? Error in save click: {ex.Message}");
            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Error", "Failed to save changes. Please try again.", "OK");
        }
    }

    /// <summary>
    /// Handle delete button click
    /// </summary>
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        try
        {
            if (UserId.HasValue)
            {
                var eventArgs = new UserProfileDeleteEventArgs(UserId.Value, _currentUserData?.FullName ?? "");
                DeleteRequested?.Invoke(this, eventArgs);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"? Error in delete click: {ex.Message}");
            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Error", "Failed to delete user. Please try again.", "OK");
        }
    }
}

/// <summary>
/// Event arguments for save operation
/// </summary>
public class UserProfileSaveEventArgs : EventArgs
{
    public UserProfileData UserData { get; }
    public bool IsCurrentUser { get; }

    public UserProfileSaveEventArgs(UserProfileData userData, bool isCurrentUser)
    {
        UserData = userData;
        IsCurrentUser = isCurrentUser;
    }
}

/// <summary>
/// Event arguments for delete operation
/// </summary>
public class UserProfileDeleteEventArgs : EventArgs
{
    public int UserId { get; }
    public string UserName { get; }

    public UserProfileDeleteEventArgs(int userId, string userName)
    {
        UserId = userId;
        UserName = userName;
    }
}

/// <summary>
/// Data class for user profile information
/// </summary>
public class UserProfileData
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
    public string Role { get; set; } = "";
    public string Initials { get; set; } = "";
    public string Gender { get; set; } = "";
    public int Age { get; set; }
    public decimal Weight { get; set; }
    public int Height { get; set; }
    public string ActivityLevel { get; set; } = "";
    public string Goal { get; set; } = "";
    public string TimeZone { get; set; } = "";
    public string Language { get; set; } = "";
    public string Allergens { get; set; } = "";
    public string ExcludedSubgroups { get; set; } = "";
    public string ExcludedFoods { get; set; } = "";
    public string FavoriteFoods { get; set; } = "";
    public string DislikedFoods { get; set; } = "";
    public string Theme { get; set; } = "Light";
    public bool NotificationsEnabled { get; set; }
}