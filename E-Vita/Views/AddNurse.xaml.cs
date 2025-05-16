namespace E_Vita.Views;

public partial class AddNurse : ContentPage
{
    public List<string> Specialties { get; set; }
    public AddNurse()
	{
		InitializeComponent();
        LoadSpecialities();
        LoadNationalities();
        LoadDepartments();
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
    "Critical Care Nursing",
    "Emergency Nursing",
    "Pediatric Nursing",
    "Neonatal Nursing",
    "Oncology Nursing",
    "Cardiac Nursing",
    "Psychiatric and Mental Health Nursing",
    "Geriatric Nursing",
    "Orthopedic Nursing",
    "Surgical Nursing",
    "Anesthesia Nursing (CRNA)",
    "Nurse Midwifery",
    "Family Nurse Practitioner",
    "Women's Health Nursing",
    "Infection Control Nursing",
    "Public Health Nursing",
    "Community Health Nursing",
    "Occupational Health Nursing",
    "School Nursing",
    "Hospice and Palliative Care Nursing",
    "Rehabilitation Nursing",
    "Diabetes Education Nursing",
    "Perioperative Nursing",
    "Wound, Ostomy, and Continence Nursing",
    "Case Management Nursing",
    "Telehealth Nursing",
    "Home Health Nursing",
    "Forensic Nursing",
    "Addiction Nursing"
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

        // Save the nurse (you can replace this with actual saving logic)
        Console.WriteLine($"Nurse Saved: {name}, {phone}, {nationality}, {specialty}, {department}");
        await DisplayAlert("Success", "Nurse added successfully!", "OK");

        // Navigate back to the previous page
        await Shell.Current.GoToAsync("..");
    }

    private async void close(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
    }
}