using Microsoft.Maui.Graphics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Vita;

public partial class Patientinfo : ContentPage
{
    double screenWidth;
    double screenHeight;
    double windowWidth = 350; // Example value, replace with actual window width
    double windowHeight = 500; // Example value, replace with actual window height
    double x;
    double y;

    public Patientinfo()
	{
		InitializeComponent();
        screenWidth = DeviceDisplay.MainDisplayInfo.Width;
        screenHeight = DeviceDisplay.MainDisplayInfo.Height;

        // Calculate the position to center the window
        x = (screenWidth - windowWidth) / 2;
        y = (screenHeight - windowHeight) / 2;
    }

    //private void close_Click(object sender, EventArgs e)
    //{
    // Example: Navigate back to the login page
    //Application.Current.MainPage = new MainPage(); // Replace with your actual login page
    //}



    private async void Close(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");

    }



    private async void ConfirmAppointment_Clicked(object sender, EventArgs e)
    {
        string patientId = patientIdEntry.Text;
        string doctorId = doctorIdEntry.Text;
        string appointmentTime = timeEntry.Text;
        string appointmentDate = dateEntry.Text;

        List<string> missingFields = new();
        //for NULL entries
        if (string.IsNullOrWhiteSpace(patientId))
            missingFields.Add("Patient ID");

        if (string.IsNullOrWhiteSpace(doctorId))
            missingFields.Add("Doctor ID");

        if (string.IsNullOrWhiteSpace(appointmentTime))
            missingFields.Add("Appointment Time");

        if (string.IsNullOrWhiteSpace(appointmentDate))
            missingFields.Add("Appointment Date");


        if (missingFields.Count > 0)
        {
            string errorMessage = "Please enter the following field(s):\n\n" + string.Join("\n", missingFields);
            await DisplayAlert("Missing Information", errorMessage, "OK");
            return;
        }

        //Check if Patient ID contains only digits
        if (!int.TryParse(patientId, out _))
        {
            await DisplayAlert("Invalid Patient ID", "Patient ID must contain only digits.", "OK");
            return;
        }

        //Check if Doctor ID contains only digits
        if (!int.TryParse(doctorId, out _))
        {
            await DisplayAlert("Invalid Doctor ID", "Doctor ID must contain only digits.", "OK");
            return;
        }

        // Patient ID must be exactly 7 digits
        if (!Regex.IsMatch(patientId, @"^\d{9}$"))
        {
            await DisplayAlert("Invalid Patient ID", "Patient ID must contain exactly 9 digits.", "OK");
            return;
        }


        // Doctor ID must be exactly 7 digits
        if (!Regex.IsMatch(doctorId, @"^\d{9}$"))
        {
            await DisplayAlert("Invalid Doctor ID", "Doctor ID must contain exactly 9 digits.", "OK");
            return;
        }


        // Check if Patient ID and Doctor ID are the same
        if (patientId == doctorId)
        {
            await DisplayAlert("Invalid IDs", "Patient ID and Doctor ID must be different.", "OK");
            return;
        }

        string message = $"Patient ID: {patientId}\nDoctor ID: {doctorId}\nTime: {appointmentTime}\nDate: {appointmentDate}";

        await DisplayAlert("Appointment has been confirmed", message, "OK");
    }

    //calendar and selecting a date
    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        // Example: Show selected date in a label or debug
        string selectedDate = e.NewDate.ToString("yyyy-MM-dd");

        // Example: Log or display
        Console.WriteLine("Selected Date: " + selectedDate);

        //  Update dateEntry field
        dateEntry.Text = e.NewDate.ToString("yyyy-MM-dd");

        // another way to update the entry-----> dateEntry.Text = selectedDate; 
    }






    protected override void OnAppearing()
    {
        base.OnAppearing();

        var timeSlots = new List<string>();
        DateTime start = DateTime.Today.AddHours(9);
        DateTime end = DateTime.Today.AddHours(17);

        while (start <= end)
        {
            timeSlots.Add(start.ToString("hh:mm tt"));
            start = start.AddMinutes(30);
        }

        timeList.ItemsSource = timeSlots;
    }

    // Show the dropdown when entry is tapped
    private void OnTimeEntryTapped(object sender, FocusEventArgs e)
    {
        timeList.IsVisible = true;
    }

    // Optional: Hide list when entry loses focus
    private void OnTimeEntryUnfocused(object sender, FocusEventArgs e)
    {
        // timeList.IsVisible = false; // Optional
    }

    // When user selects a time
    private void OnTimeSlotSelected(object sender, SelectionChangedEventArgs e)
    {
        string selectedTime = e.CurrentSelection.FirstOrDefault() as string;

        if (!string.IsNullOrEmpty(selectedTime))
        {
            timeEntry.Text = selectedTime;
            timeList.IsVisible = false; // Hide after selection
        }
    }






}

