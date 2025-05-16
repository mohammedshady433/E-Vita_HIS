using E_Vita.Models;
using E_Vita.Services;
using System.Collections.ObjectModel;
using E_Vita_APIs.Models;
namespace E_Vita.Views
{
    public partial class DischargePlanning : ContentPage
    {
        private ObservableCollection<Patient> _patients;
        private Patient _selectedPatient;

        public DischargePlanning()
        {
            InitializeComponent();
            DischargeDatePicker.MinimumDate = DateTime.Today;
            LoadPatients();
        }

        private async void LoadPatients()
        {
            try
            {
                // TODO: Replace with actual service call
                //var patients = await PatientServices.GetPatientsReadyForDischarge();
                //_patients = new ObservableCollection<Patient>(patients);
                //PatientsGrid.ItemsSource = _patients;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load patients: " + ex.Message, "OK");
            }
        }

        private void OnPatientSelected(object sender, Syncfusion.Maui.DataGrid.DataGridSelectionChangedEventArgs e)
        {
            // Get the selected item using the SelectedIndex
            if (PatientsGrid.SelectedIndex >= 0)
            {
                var selectedItem = _patients[PatientsGrid.SelectedIndex] as Patient;
                if (selectedItem != null)
                {
                    _selectedPatient = selectedItem;
                    PatientNameEntry.Text = _selectedPatient.PatientName;
                    RoomNumberEntry.Text = _selectedPatient.RoomNumber;
                    DischargeDatePicker.Date = DateTime.Today;
                }
            }
        }

        private async void OnProcessDischarge(object sender, EventArgs e)
        {
            if (_selectedPatient == null)
            {
                await DisplayAlert("Error", "Please select a patient first", "OK");
                return;
            }

            if (string.IsNullOrEmpty(DischargeNotesEditor.Text))
            {
                await DisplayAlert("Error", "Please enter discharge notes", "OK");
                return;
            }

            if (DischargeTypePicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please select a discharge type", "OK");
                return;
            }

            try
            {
                var dischargeInfo = new DischargeInfo
                {
                    PatientId = _selectedPatient.PatientId,
                    DischargeDate = DischargeDatePicker.Date,
                    Notes = DischargeNotesEditor.Text,
                    DischargeType = DischargeTypePicker.SelectedItem.ToString()
                };

                // TODO: Replace with actual service call
                await PatientService.ProcessDischarge(dischargeInfo);

                await DisplayAlert("Success", "Patient discharged successfully", "OK");
                
                // Clear form and refresh patient list
                ClearForm();
                LoadPatients();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to process discharge: " + ex.Message, "OK");
            }
        }

        private void ClearForm()
        {
            _selectedPatient = null;
            PatientNameEntry.Text = string.Empty;
            RoomNumberEntry.Text = string.Empty;
            DischargeNotesEditor.Text = string.Empty;
            DischargeTypePicker.SelectedItem = null;
            DischargeDatePicker.Date = DateTime.Today;
        }
    }
} 