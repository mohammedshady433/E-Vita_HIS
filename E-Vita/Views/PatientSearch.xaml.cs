using E_Vita.Services;
using E_Vita_APIs.Models;

namespace E_Vita.Views;

public partial class PatientSearch : ContentPage
{
    PatientServices patientServices = new PatientServices();
    PrescriptionService PrescriptionService = new PrescriptionService();
    List<Patient> patients = new List<Patient>();
    List<E_Vita_APIs.Models.Prescription> prescriptions = new List<E_Vita_APIs.Models.Prescription>(); // Update type to match API model

    public PatientSearch()
	{
		InitializeComponent();
        this.BindingContext = this;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await loadpatientsandprescriptions();
    }

    private async Task loadpatientsandprescriptions()
    {
        patients = await patientServices.GetPatientsAsync();
        prescriptions = await PrescriptionService.GetPrescriptionsAsync(); // Ensure type consistency

    }

    private async void search2(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(SearchForPatientTextBox.Text))
            {
                await DisplayAlert("Error", "Please enter a patient ID", "OK");
                return;
            }

            if (!int.TryParse(SearchForPatientTextBox.Text, out int patientId))
            {
                await DisplayAlert("Error", "Please enter a valid patient ID", "OK");
                return;
            }

            var patient = patients.Find(p => p.ID == patientId);
            if (patient == null)
            {
                await DisplayAlert("Error", "Patient not found", "OK");
                return;
            }

            // Set the patient name
            NameTextBlock.Text = patient.Name;

            // Get prescriptions for this patient
            var patientPrescriptions = prescriptions.Where(p => p.PatientId == patient.ID).ToList();
            if (patientPrescriptions.Any())
            {
                // Display the prescriptions in the DataGrid
                PatientsGrid.ItemsSource = patientPrescriptions;
            }
            else
            {
                await DisplayAlert("Info", "No prescription history found for this patient", "OK");
                PatientsGrid.ItemsSource = null;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Search failed: {ex.Message}", "OK");
        }
    }
}