using Microsoft.Maui.Controls;
using E_Vita_APIs.Models;
using E_Vita.Services;

namespace E_Vita.Views
{
    public partial class AddOperationRoomForm : ContentPage
    {
        private readonly OperationRoomServices _operationRoomServices = new OperationRoomServices();
        private readonly RoomService _RoomServices = new RoomService();

        private Picker equipmentPicker;
        private Picker operationPicker;

        public AddOperationRoomForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            Title = "Add New Operation Room";
            BackgroundColor = Color.FromArgb("#F5F5F5");

            var mainLayout = new ScrollView
            {
                Content = new StackLayout { Spacing = 20, Padding = 20 }
            };

            // Back Button
            var backButton = new Button
            {
                Text = "Back",
                FontFamily = "Qatar2022 Arabic",
                BackgroundColor = Color.FromArgb("#3b0054"),
                TextColor = Colors.AntiqueWhite,
                CornerRadius = 20,
                HeightRequest = 50,
                WidthRequest = 80,
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(0, 0, 0, 20)
            };
            backButton.Clicked += OnBackClicked;
            ((StackLayout)mainLayout.Content).Children.Add(backButton);

            // Equipment Selection
            var equipmentFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15
            };
            var equipmentStack = new StackLayout { Spacing = 5 };
            equipmentStack.Children.Add(new Label
            {
                Text = "Select Equipment",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black
            });
            equipmentPicker = new Picker
            {
                ItemsSource = new List<string> 
                { 
                    "Basic Surgical Equipment",
                    "Advanced Surgical Equipment",
                    "Cardiac Surgery Equipment",
                    "Neurosurgery Equipment",
                    "Orthopedic Equipment"
                },
                FontSize = 16,
                TextColor = Colors.Black
            };
            equipmentStack.Children.Add(equipmentPicker);
            equipmentFrame.Content = equipmentStack;

            // Operation Type Selection
            var operationFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15
            };
            var operationStack = new StackLayout { Spacing = 5 };
            operationStack.Children.Add(new Label
            {
                Text = "Select Operation Type",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black
            });
            operationPicker = new Picker
            {
                ItemsSource = new List<string>
                {
                    "General Surgery",
                    "Cardiac Surgery",
                    "Neurosurgery",
                    "Orthopedic Surgery",
                    "Emergency Surgery"
                },
                FontSize = 16,
                TextColor = Colors.Black
            };
            operationStack.Children.Add(operationPicker);
            operationFrame.Content = operationStack;

            // Save Button
            var saveButton = new Button
            {
                Text = "Save Room",
                BackgroundColor = Color.FromArgb("#4CAF50"),
                TextColor = Colors.White,
                CornerRadius = 25,
                HeightRequest = 50,
                FontSize = 16,
                Margin = new Thickness(0, 20, 0, 0)
            };
            saveButton.Clicked += OnSaveClicked;

            // Add all frames to the main layout
            ((StackLayout)mainLayout.Content).Children.Add(equipmentFrame);
            ((StackLayout)mainLayout.Content).Children.Add(operationFrame);
            ((StackLayout)mainLayout.Content).Children.Add(saveButton);

            Content = mainLayout;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (equipmentPicker.SelectedItem == null || operationPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please fill in all required fields", "OK");
                return;
            }

            try
            {
                
                var newRoom = new Operation_Room
                {
                    RoomId = 1,
                    Equipment = equipmentPicker.SelectedItem.ToString(),
                    Operation = operationPicker.SelectedItem.ToString(),
                    RoomStatus = RoomStatus.Available
                };

                var success = await _operationRoomServices.CreateAsync(newRoom);
                if (success)
                {
                    await DisplayAlert("Success", "Operation room has been added successfully!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to add operation room", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
            }
        }
    }
} 