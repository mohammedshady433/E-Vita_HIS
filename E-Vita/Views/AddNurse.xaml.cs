using E_Vita.Services;
using E_Vita_APIs.Models;
using System;

namespace E_Vita.Views;

public partial class AddNurse : ContentPage
{
    public List<string> Specialties { get; set; }
    public List<DayOfWeekSelection> DaysOfWeek { get; set; }

    public AddNurse()
	{
		InitializeComponent();
        LoadSpecialities();
        LoadNationalities();
        LoadDepartments();
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
    private void LoadSpecialities()
    {
        var specialties = new List<string>
{
    "CriticalCareNursing",
    "EmergencyNursing",
    "PediatricNursing",
    "NeonatalNursing",
    "OncologyNursing",
    "CardiacNursing",
    "PsychiatricAndMentalHealthNursing",
    "GeriatricNursing",
    "OrthopedicNursing",
    "SurgicalNursing",
    "AnesthesiaNursing(CRNA)",
    "NurseMidwifery",
    "FamilyNursePractitioner",
    "WomensHealthNursing",
    "InfectionControlNursing",
    "PublicHealthNursing",
    "CommunityHealthNursing",
    "OccupationalHealthNursing",
    "SchoolNursing",
    "HospiceAndPalliativeCareNursing",
    "RehabilitationNursing",
    "DiabetesEducationNursing",
    "PerioperativeNursing",
    "Wound,Ostomy,AndContinenceNursing",
    "CaseManagementNursing",
    "TelehealthNursing",
    "HomeHealthNursing",
    "ForensicNursing",
    "AddictionNursing"
};
        SpecialtyPicker.ItemsSource = specialties;
    }
    private void LoadDepartments()
    {
        var departments = new List<string>
        {
            "Emergency Department",
            "Intensive Care Unit",
            "Pediatrics",
            "Maternity",
            "Surgery",
            "Cardiology",
            "Neurology",
            "Orthopedics",
            "Oncology",
            "Radiology",
            "Laboratory",
            "Rehabilitation",

        };

        DepartmentPicker.ItemsSource = departments;
    }

    private async void OnSaveNurseClicked(object sender, EventArgs e)
    {
        string name = NurseNameEntry.Text?.Trim();
        string phone = PhoneNumberEntry.Text?.Trim();
        string nationality = (string)NationalityPicker.SelectedItem;
        string specialty = (string)SpecialtyPicker.SelectedItem;
        string department = (string)DepartmentPicker.SelectedItem;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || 
            nationality == null || specialty == null || department == null)
        {
            await DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }
        PractitionerServices practitionerServices = new PractitionerServices();
        PractitionerRoleService practitionerRoleService = new PractitionerRoleService();
        var selectedDays = DaysOfWeek.Where(d => d.IsSelected).Select(d => d.Name).ToList();
        Practitioner practitioner = new Practitioner
        {
            Name = name,
            Phone = phone,
            Email = Emailentry.Text?.Trim(),
            Address = Addressentry.Text?.Trim(),
            Rank = specialty,
            PasswordHash = Passwordentry.Text,
            Id = int.Parse(NurseID.Text)

        };
        Practitioner_Role practitionerRole = new Practitioner_Role
        {
            Code = "Nurse" + practitioner.Id,
            Specialty = (MedicalSpecialty)Enum.Parse(typeof(MedicalSpecialty), specialty),
            Service = Service.Nurse,
            PractitionerId = practitioner.Id,
            DayOfWeek = selectedDays.FirstOrDefault(),
            StartTime = StartTimePicker.Time,
            EndTime = EndTimePicker.Time
        };

        // Save the practitioner and role
        await practitionerServices.AddPractitionerAsync(practitioner);
        await practitionerRoleService.AddPractitionerRoleAsync(practitionerRole);   

        await DisplayAlert("Success", "Nurse added successfully!", "OK");

        // Navigate back to the previous page
    }

    private async void close(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
    }
}