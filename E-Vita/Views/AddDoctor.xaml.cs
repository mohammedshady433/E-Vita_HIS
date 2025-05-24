using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using E_Vita.Services;
using E_Vita_APIs.Models;
namespace E_Vita.Views;
public partial class AddDoctor : ContentPage
{
    public List<string> Specialties { get; set; }
    public List<DayOfWeekSelection> DaysOfWeek { get; set; }

    public AddDoctor()
    {
        InitializeComponent();
        LoadSpecialities();
        LoadNationalities();
        LoadRanks();
        LoadDaysOfWeek();
        BindingContext = this;
    }
    public class DayOfWeekSelection
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
    private void LoadDaysOfWeek()
    {
        DaysOfWeek = new List<DayOfWeekSelection>
        {
            new DayOfWeekSelection { Name = "Sunday" },
            new DayOfWeekSelection { Name = "Monday" },
            new DayOfWeekSelection { Name = "Tuesday" },
            new DayOfWeekSelection { Name = "Wednesday" },
            new DayOfWeekSelection { Name = "Thursday" },
            new DayOfWeekSelection { Name = "Friday" },
            new DayOfWeekSelection { Name = "Saturday" }
        };
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
        var specialties = new List <string> { "AllergyandImmunology", "Anesthesiology", "Cardiology", "ColonandRectalSurgery", "CriticalCareMedicine", "Dermatology", "EmergencyMedicine", "EndocrinologyandMetabolism", "FamilyMedicine", "Gastroenterology", "GeneralSurgery", "GeriatricMedicine", "Hematology", "HospiceandPalliativeMedicine", "InfectiousDisease", "InternalMedicine", "MedicalGeneticsandGenomics", "Nephrology", "Neurology", "Neurosurgery", "NuclearMedicine", "ObstetricsandGynecology", "Oncology", "Ophthalmology", "OrthopedicSurgery", "Otolaryngology", "Pathology", "Pediatrics", "PhysicalMedicineandRehabilitation", "PlasticSurgery", "Podiatry", "PreventiveMedicine", "Psychiatry", "Pulmonology", "Radiology", "Rheumatology", "SleepMedicine", "SportsMedicine", "ThoracicSurgery", "TransplantSurgery", "Urology", "VascularSurgery" };
        SpecialtyPicker.ItemsSource = specialties;
    }

    private async void OnSaveDoctorClicked(object sender, EventArgs e)
    {
        string name = DoctorNameEntry.Text?.Trim();
        string phone = PhoneNumberEntry.Text?.Trim();
        string nationality = (string)NationalityPicker.SelectedItem;
        string specialty = (string)SpecialtyPicker.SelectedItem;
        string rank = (string)RankPicker.SelectedItem;

        // Get selected days
        var selectedDays = DaysOfWeek.Where(d => d.IsSelected).Select(d => d.Name).ToList();

        // Get working hours
        var startTime = StartTimePicker.Time;
        var endTime = EndTimePicker.Time;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) ||
            nationality == null || specialty == null || rank == null ||
            !selectedDays.Any())
        {
            await DisplayAlert("Error", "Please fill in all fields and select at least one available day.", "OK");
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

        // Save the doctor (replace with actual saving logic)
        Console.WriteLine($"Doctor Saved: {name}, {phone}, {nationality}, {specialty}, {rank}");
        Console.WriteLine($"Available Days: {string.Join(", ", selectedDays)}");
        Console.WriteLine($"Working Hours: {startTime} - {endTime}");
        PractitionerRoleService practitionerRoleService = new PractitionerRoleService();
        PractitionerServices practitionerServices = new PractitionerServices();

        Practitioner_Role practitioner_Role = new Practitioner_Role();
        Practitioner practitioner = new Practitioner();

        practitioner.Name = name;
        practitioner.Phone = phone;
        practitioner.Email = EmailEntry.Text?.Trim();
        practitioner.Address = AddressEntry.Text?.Trim();
        practitioner.Rank = rank;
        practitioner.PasswordHash = DocPassword.Text;
        practitioner.Id = int.Parse(DocID.Text);


        practitioner_Role.PractitionerId = practitioner.Id;
        practitioner_Role.Specialty = (MedicalSpecialty)Enum.Parse(typeof(MedicalSpecialty), specialty);
        if (practitioner.Id.ToString().EndsWith("1"))
        {
            practitioner_Role.Service = Service.DoctorIN;
            practitioner_Role.Code = "DocIn" + practitioner.Id.ToString();

        }
        else if (practitioner.Id.ToString().EndsWith("4"))
        {
            practitioner_Role.Service = Service.DoctorOUT;
            practitioner_Role.Code ="DocOut" + practitioner.Id.ToString();
        }
        else 
        {
            //show error id message
            await DisplayAlert("Error", "Doctor ID must end with 1 or 4 for DoctorIN or DoctorOUT respectively.", "OK");
            return;
        }
        practitioner_Role.DayOfWeek = selectedDays.FirstOrDefault();
        practitioner_Role.StartTime = startTime;
        practitioner_Role.EndTime = endTime;
        

        await practitionerRoleService.AddPractitionerRoleAsync(practitioner_Role);
        await practitionerServices.AddPractitionerAsync(practitioner);
        await DisplayAlert("Success", "Doctor added successfully!", "OK");

        // Navigate back to the previous page
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
    }

    private async void close(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
    }
}
