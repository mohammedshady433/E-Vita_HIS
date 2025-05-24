using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Runtime.Intrinsics.Arm;
using E_Vita.Views;
using E_Vita_APIs.Models;
using E_Vita.Services;


namespace E_Vita.Views;

public partial class AddPatient : ContentPage
{
    Patient patient = new Patient();

    public AddPatient()
    {
        InitializeComponent();
    }

    private void ReadPatientInfo()
    {
        string name = PatientNameEntry.Text;
        string patientId = PatientIdEntry.Text;
        string phoneNumber = PhoneNumberEntry.Text;

        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"ID: {patientId}");
        Console.WriteLine($"Phone Number: {phoneNumber}");
    }

    // Assuming the error occurs when assigning a string value to a property of type Gender.
    // Fix: Use Enum.Parse or Enum.TryParse to convert the string to the Gender enum.

    private void OnGenderSelected(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value is true) // Ensure the RadioButton is checked
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (Enum.TryParse<Gender>(radioButton.Value.ToString(), out var selectedGender))
                {
                    patient.Gender = selectedGender;
                }
                else
                {
                    Console.WriteLine("Invalid gender value selected.");
                }
            }
        }
    }
    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime selectedDate = e.NewDate;
        int age = DateTime.Today.Year - selectedDate.Year;
        if (selectedDate > DateTime.Today.AddYears(-age)) age--;
        patient.DateOfBirth = selectedDate;
        
    }
    private void OnBloodTypeSelected(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedIndex != -1)
        {
            string selectedBloodType = picker.Items[picker.SelectedIndex];
            // Do something with selectedBloodType, e.g. display it or store it
            Console.WriteLine($"Selected Blood Type: {selectedBloodType}");
        }
    }
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;

        // Example: sample suggestions
        var allSuggestions = new List<string> { "Diabetes", "Hypertension", "Asthma", "Cancer", "Anemia" };

        var filteredSuggestions = allSuggestions
            .Where(d => d.StartsWith(searchText, StringComparison.OrdinalIgnoreCase))
            .ToList();

        SuggestionsListView.ItemsSource = filteredSuggestions;
        SuggestionsListView.IsVisible = filteredSuggestions.Any();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var nationalities = await LoadNationalitiesAsync();
        NationalityPicker.ItemsSource = nationalities;
    }

    private async Task<List<string>> LoadNationalitiesAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("nationalities.json");
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();
        return JsonSerializer.Deserialize<List<string>>(json);
    }
    private void OnAddPatientClicked(object sender, EventArgs e)
    {
        string name = PatientNameEntry.Text;
        string id = PatientIdEntry.Text;
        string phone = PhoneNumberEntry.Text;
        string gender = "";
        DateTime dob = DobPicker.Date;
        string bloodType = BloodTypePicker.SelectedIndex != -1 ? BloodTypePicker.Items[BloodTypePicker.SelectedIndex] : "";
        string nationality = (string)NationalityPicker.SelectedItem;

        // Example output
        Console.WriteLine($"Saving: {name}, {id}, {phone}, {gender}, {dob.ToShortDateString()}, {bloodType}, {nationality}");

        // You can add validation and saving logic here
    }
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        string name = PatientNameEntry.Text;
        string patientId = PatientIdEntry.Text;
        string phoneNumber = PhoneNumberEntry.Text;
        patient.Name = name;
        patient.Phone = phoneNumber;
        patient.Nationality = NationalityPicker.SelectedItem as string;
        patient.WardRoundId = null;
        patient.Email = EmailEntry.Text;
        patient.Address = AddressEntry.Text;
        patient.Status = OUTIN_Patient.Out_Patient;

        PatientServices patientServices = new PatientServices();
        await patientServices.AddPatientAsync(patient);
        await DisplayAlert("Success", "Patient added successfully!", "OK");
    }

    private async void close(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
    }

    private void SuggestionsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        SearchEntry.Text = SuggestionsListView.SelectedItem.ToString();
    }
}