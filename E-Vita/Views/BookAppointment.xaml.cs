using Microsoft.Maui.Graphics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace E_Vita.Views;

public partial class BookAppointment : ContentPage
{
    public BookAppointment()
    {
        InitializeComponent();
    }

    private async void Close(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
    }

    private async void ConfirmAppointment_Clicked(object sender, EventArgs e)
    {
        string patientId = patientIdEntry.Text;
        string doctorId = doctorIdEntry.Text;
        string appointmentTime = timeEntry.Text;
        string appointmentDate = dateEntry.Text;

        List<string> missingFields = new();
        if (string.IsNullOrWhiteSpace(patientId)) missingFields.Add("Patient ID");
        if (string.IsNullOrWhiteSpace(doctorId)) missingFields.Add("Doctor ID");
        if (string.IsNullOrWhiteSpace(appointmentTime)) missingFields.Add("Appointment Time");
        if (string.IsNullOrWhiteSpace(appointmentDate)) missingFields.Add("Appointment Date");

        if (missingFields.Count > 0)
        {
            string errorMessage = "Please enter the following field(s):\n\n" + string.Join("\n", missingFields);
            await DisplayAlert("Missing Information", errorMessage, "OK");
            return;
        }

        if (!int.TryParse(patientId, out _))
        {
            await DisplayAlert("Invalid Patient ID", "Patient ID must contain only digits.", "OK");
            return;
        }

        if (!int.TryParse(doctorId, out _))
        {
            await DisplayAlert("Invalid Doctor ID", "Doctor ID must contain only digits.", "OK");
            return;
        }

        // Patient and Doctor ID must be exactly 9 digits
        if (!Regex.IsMatch(patientId, @"^\d{9}$"))
        {
            await DisplayAlert("Invalid Patient ID", "Patient ID must contain exactly 9 digits.", "OK");
            return;
        }

        if (!Regex.IsMatch(doctorId, @"^\d{9}$"))
        {
            await DisplayAlert("Invalid Doctor ID", "Doctor ID must contain exactly 9 digits.", "OK");
            return;
        }

        if (patientId == doctorId)
        {
            await DisplayAlert("Invalid IDs", "Patient ID and Doctor ID must be different.", "OK");
            return;
        }

        string message = $"Patient ID: {patientId}\nDoctor ID: {doctorId}\nTime: {appointmentTime}\nDate: {appointmentDate}";
        await DisplayAlert("Appointment has been confirmed", message, "OK");
    }

    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        dateEntry.Text = e.NewDate.ToString("yyyy-MM-dd");

        // Set the minimum selectable date to today
        datePicker.MinimumDate = DateTime.Today;

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

    private void OnTimeEntryTapped(object sender, FocusEventArgs e)
    {
        timeList.IsVisible = true;
    }

    private void OnTimeSlotSelected(object sender, SelectionChangedEventArgs e)
    {
        string selectedTime = e.CurrentSelection.FirstOrDefault() as string;

        if (!string.IsNullOrEmpty(selectedTime))
        {
            timeEntry.Text = selectedTime;
            timeList.IsVisible = false;
        }
    }
}