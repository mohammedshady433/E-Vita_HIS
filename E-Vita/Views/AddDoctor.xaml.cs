using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
namespace E_Vita.Views;
public partial class AddDoctor : ContentPage
{
    public List<string> Specialties { get; set; }
    public AddDoctor()
    {
        InitializeComponent();
        LoadSpecialities();
        LoadNationalities();
        LoadRanks();
    }

    private void LoadNationalities()
    {
        // Example nationalities (you can replace this with dynamic data if needed)
        var nationalities = new List<string>
        {
            "American", "British", "Canadian", "Indian", "Australian", "German", "French", "Chinese", "Japanese","Egyptian"
        };

        NationalityPicker.ItemsSource = nationalities;
    }

    private void LoadRanks()
    {
        var ranks = new List<string>
        {
            "Chief of Medicine",
            "Department Head",
            "Senior Consultant",
            "Consultant",
            "Senior Specialist",
            "Specialist",
            "Senior Resident",
            "Resident",
            "Fellow",
            "Medical Officer",
            "House Officer",
            "Intern"
        };

        RankPicker.ItemsSource = ranks;
    }

    private void LoadSpecialities()
    {
        // Example nationalities (you can replace this with dynamic data if needed)
        var specialties = new List <string> { "Allergy and Immunology", "Anesthesiology", "Cardiology", "Colon and Rectal Surgery", "Critical Care Medicine", "Dermatology", "Emergency Medicine", "Endocrinology and Metabolism", "Family Medicine", "Gastroenterology", "General Surgery", "Geriatric Medicine", "Hematology", "Hospice and Palliative Medicine", "Infectious Disease", "Internal Medicine", "Medical Genetics and Genomics", "Nephrology", "Neurology", "Neurosurgery", "Nuclear Medicine", "Obstetrics and Gynecology", "Oncology", "Ophthalmology", "Orthopedic Surgery", "Otolaryngology", "Pathology", "Pediatrics", "Physical Medicine and Rehabilitation", "Plastic Surgery", "Podiatry", "Preventive Medicine", "Psychiatry", "Pulmonology", "Radiology", "Rheumatology", "Sleep Medicine", "Sports Medicine", "Thoracic Surgery", "Transplant Surgery", "Urology", "Vascular Surgery" };
        SpecialtyPicker.ItemsSource = specialties;
    }

    private async void OnSaveDoctorClicked(object sender, EventArgs e)
    {
        string name = DoctorNameEntry.Text?.Trim();
        string phone = PhoneNumberEntry.Text?.Trim();
        string nationality = (string)NationalityPicker.SelectedItem;
        string specialty = (string)SpecialtyPicker.SelectedItem;
        string rank = (string)RankPicker.SelectedItem;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || 
            nationality == null || specialty == null || rank == null)
        {
            await DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }

        if (phone.Length != 11)
        {
            await DisplayAlert("Error", "Phone number must be exactly 11 digits.", "OK");
            return;
        }
        if (!phone.All(char.IsDigit))
        {
            await DisplayAlert("Error", "Phone number must contain only digits (no letters or special characters).", "OK");
            return;
        }

        // Save the doctor (you can replace this with actual saving logic)
        Console.WriteLine($"Doctor Saved: {name}, {phone}, {nationality}, {specialty}, {rank}");
        await DisplayAlert("Success", "Doctor added successfully!", "OK");

        // Navigate back to the previous page
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
    }

    private async void close(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
    }
}
