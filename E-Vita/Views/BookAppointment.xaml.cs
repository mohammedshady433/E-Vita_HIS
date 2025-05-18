using Microsoft.Maui.Graphics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace E_Vita.Views;

public partial class BookAppointment : ContentPage
{
    private class Doctor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DateTime> AvailableDates { get; set; }
    }

    private class Patient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayText => $"{Name} (ID: {Id})";
    }

    private List<Doctor> doctors;
    private List<Patient> patients;
    private Patient selectedPatient;

    public BookAppointment()
    {
        InitializeComponent();
        LoadDoctors();
        LoadPatients();
    }

    private void LoadDoctors()
    {
        // This would typically come from a database or API
        doctors = new List<Doctor>
        {
            new Doctor { Id = "123456789", Name = "Dr. Sahar", AvailableDates = new List<DateTime> { DateTime.Today.AddDays(1), DateTime.Today.AddDays(2) } },
            new Doctor { Id = "234567890", Name = "Dr. Shady", AvailableDates = new List<DateTime> { DateTime.Today, DateTime.Today.AddDays(3) } },
            new Doctor { Id = "345678901", Name = "Dr. Sandy", AvailableDates = new List<DateTime> { DateTime.Today.AddDays(1), DateTime.Today.AddDays(4) } }
        };
    }

    private void LoadPatients()
    {
        // This would typically come from a database or API
        patients = new List<Patient>
        {
            new Patient { Id = "200", Name = "Mohammed Shady" },
            new Patient { Id = "201", Name = "Mohammed Hesham" },
            new Patient { Id = "202", Name = "Shahd Mostafa" },
            new Patient { Id = "203", Name = "Sausan Badr" },
            new Patient { Id = "204", Name = "Mohammed Hamaki" }
        };
    }

    private void OnPatientSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            patientList.IsVisible = false;
            return;
        }

        var searchText = e.NewTextValue.ToLower();
        var filteredPatients = patients
            .Where(p => p.Id.Contains(searchText) || p.Name.ToLower().Contains(searchText))
            .ToList();

        patientList.ItemsSource = filteredPatients;
        patientList.IsVisible = filteredPatients.Any();
    }

    private void OnPatientSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Patient selected)
        {
            selectedPatient = selected;
            patientSearchBar.Text = selected.DisplayText;
            patientList.IsVisible = false;
        }
    }

    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        UpdateAvailableDoctors(e.NewDate);
    }

    private void UpdateAvailableDoctors(DateTime selectedDate)
    {
        var availableDoctors = doctors
            .Where(d => d.AvailableDates.Any(date => date.Date == selectedDate.Date))
            .Select(d => d.Name)
            .ToList();

        doctorPicker.ItemsSource = availableDoctors;
        doctorPicker.IsEnabled = availableDoctors.Any();

        if (!availableDoctors.Any())
        {
            DisplayAlert("No Doctors Available", "There are no doctors available on the selected date. Please choose another date.", "OK");
        }
    }

    private async void ConfirmAppointment_Clicked(object sender, EventArgs e)
    {
        if (selectedPatient == null)
        {
            await DisplayAlert("Error", "Please select a patient", "OK");
            return;
        }

        if (doctorPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select a doctor", "OK");
            return;
        }

        string appointmentTime = timeEntry.Text;
        if (string.IsNullOrWhiteSpace(appointmentTime))
        {
            await DisplayAlert("Error", "Please select an appointment time", "OK");
            return;
        }

        string selectedDoctor = doctorPicker.SelectedItem.ToString();
        string message = $"Patient: {selectedPatient.Name}\n" +
                        $"Patient ID: {selectedPatient.Id}\n" +
                        $"Doctor: {selectedDoctor}\n" +
                        $"Time: {appointmentTime}\n" +
                        $"Date: {datePicker.Date:yyyy-MM-dd}";
        
        await DisplayAlert("Appointment has been confirmed", message, "OK");
    }

    private async void Close(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
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