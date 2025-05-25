using E_Vita_APIs.Models;
using E_Vita.Services;
using System.Collections.ObjectModel;

namespace E_Vita.Views;

public partial class InpatientOrdersPage : ContentPage
{
    private int _patientId;
    private int _doctorId;

    public ObservableCollection<string> LabTests { get; set; } = new();
    public ObservableCollection<string> Medications { get; set; } = new();
    public ObservableCollection<string> Radiologies { get; set; } = new();

    public InpatientOrdersPage()
    {
        InitializeComponent();
        _patientId = 201;
        _doctorId = 123456784;
        LabTestsList.ItemsSource = LabTests;
        MedicationsList.ItemsSource = Medications;
        RadiologiesList.ItemsSource = Radiologies;
        // Load patient info here if needed
    }

    private void OnAddLabTestClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(LabTestEntry.Text))
            LabTests.Add(LabTestEntry.Text.Trim());
        LabTestEntry.Text = "";
    }

    private void OnAddMedicationClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(MedicationEntry.Text) && !string.IsNullOrWhiteSpace(MedicationDoseEntry.Text))
            Medications.Add($"{MedicationEntry.Text.Trim()} - {MedicationDoseEntry.Text.Trim()}");
        MedicationEntry.Text = "";
        MedicationDoseEntry.Text = "";
    }

    private void OnAddRadiologyClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(RadiologyEntry.Text))
            Radiologies.Add(RadiologyEntry.Text.Trim());
        RadiologyEntry.Text = "";
    }

    private async void OnRecordVitalsClicked(object sender, EventArgs e)
    {
        // Save vital signs to DB (implement your own model/service)
        await DisplayAlert("Vitals Recorded", "Vital signs have been recorded.", "OK");
    }

    private async void OnSubmitOrdersClicked(object sender, EventArgs e)
    {
        // Save all orders to DB (implement your own model/service)
        await DisplayAlert("Orders Submitted", "All orders have been submitted.", "OK");
        await Shell.Current.GoToAsync(nameof(InpatientDoctorDashboard));
    }
}
