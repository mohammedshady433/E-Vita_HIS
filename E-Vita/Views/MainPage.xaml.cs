﻿using E_Vita.Views;
using E_Vita.Services;
using E_Vita_APIs.Models;
namespace E_Vita
{
    public partial class MainPage : ContentPage
    {
        double screenWidth;
        double screenHeight;
        double windowWidth = 350; // Example value, replace with actual window width
        double windowHeight = 390; // Example value, replace with actual window height
        double x;
        double y;
        Task<List<Practitioner>> practlist;
        private readonly PractitionerServices _apiPractServices = new PractitionerServices();
        public MainPage()
        {
            InitializeComponent();
            screenWidth = DeviceDisplay.MainDisplayInfo.Width;
            screenHeight = DeviceDisplay.MainDisplayInfo.Height;

            // Calculate the position to center the window
            x = (screenWidth - windowWidth) / 2;
            y = (screenHeight - windowHeight) / 2;

            practlist=LoadPractData();
        }

        public Task<List<Practitioner>> LoadPractData()
        {
            var practlist = _apiPractServices.GetPractitionersAsync();
            return practlist;
        }
        private async void login(object sender, EventArgs e)
        {
            try
            {
                // Get entered ID
                if (!int.TryParse(userIdEntry.Text?.Trim(), out int enteredId))
                {
                    await DisplayAlert("Error", "Please enter a valid numerical ID.", "OK");
                    return;
                }
                var practitioners = await practlist;
                var practitioner = practitioners.FirstOrDefault(p => p.Id == enteredId && p.PasswordHash.Equals(Passentry.Text));
                if (practitioner == null)
                {
                    await DisplayAlert("Error", "Practitioner not found.", "OK");
                    return;
                }
                var ID=enteredId.ToString();
                if (ID.EndsWith("1"))
                {
                    // Store the doctor ID in preferences before navigation
                    Preferences.Set("CurrentDoctorId", enteredId);
                    await Shell.Current.GoToAsync(nameof(DoctorDashboard));
                    await DisplayAlert("Approved", $"Welcome {practitioner.Name}", "OK");

                }
                else if (ID.EndsWith("2"))
                {
                    await Shell.Current.GoToAsync(nameof(NurseDashboard));
                    await DisplayAlert("Approved", $"Welcome {practitioner.Name}", "OK");

                }
                else if (ID.EndsWith("3"))
                {
                    await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
                    await DisplayAlert("Approved", $"Welcome {practitioner.Name}", "OK");

                }
                else if (ID.EndsWith("4"))
                {
                    await Shell.Current.GoToAsync(nameof(InpatientDoctorDashboard));
                    await DisplayAlert("Approved", $"Welcome {practitioner.Name}", "OK");

                }
                else
                {
                    await DisplayAlert("Error", "Invalid ID. Please check your ID.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
            }
        }

        private void Forgot_pass(object sender, EventArgs e)
        {
            // Create a new window
            var newWindow = new Window
            {
                X = x-200, // Set the x position
                Y = y+15, // Set the y position
                Width = windowWidth, // Set the width
                Height = windowHeight, // Set the height
                Title = "Reset Password" // Optional: Set the window title
            };

            // Create a new page (or use an existing one) to set as the content of the window
            //var newPage = new ContentPage
            //{
            //    Content = new Label { Text = "This is a new window!" }
            //};

            // Set the page as the content of the new window
            newWindow.Page = new PassReset();

            // Open the new window
            Application.Current.OpenWindow(newWindow);
            //Application.Current?.OpenWindow(new Window(new PassReset()));
        }
    }
}

