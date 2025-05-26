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
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadPatients();
        }
        private async void LoadPatients()
        {
            try
            {
                // TODO: Replace with actual service call
                //var patients = await PatientServices.GetPatientsReadyForDischarge();

                PatientServices patientServices = new PatientServices();
                var patients = await patientServices.GetPatientsAsync();
                patients = patients.Where(p => p.Status == OUTIN_Patient.In_Patient).ToList();
                _patients = new ObservableCollection<Patient>(patients);
                PatientsGrid.ItemsSource = _patients;


            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load patients: " + ex.Message, "OK");
            }
        }

        private void OnPatientSelected(object sender, Syncfusion.Maui.DataGrid.DataGridSelectionChangedEventArgs e)
        {
            // Safely get the selected item directly from the event args
            if (e.AddedRows != null && e.AddedRows.Count > 0)
            {
                var selectedItem = e.AddedRows[0] as Patient;
                if (selectedItem != null)
                {
                    _selectedPatient = selectedItem;
                    PatientNameEntry.Text = _selectedPatient.Name;
                    RoomNumberEntry.Text = _selectedPatient.ID.ToString();
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
                var dischargeInfo = new Discharge
                {
                    PatientId = _selectedPatient.ID,
                    When = TimeOnly.FromDateTime(DischargeDatePicker.Date),
                    Note = DischargeNotesEditor.Text,
                    Date = DischargeDatePicker.Date,
                    //DischargeType = (DischargeType)DischargeTypePicker.SelectedItem
                };
                // Convert the string to the enum value
                string selectedDischargeTypeString = DischargeTypePicker.SelectedItem.ToString();
                DischargeType dischargeTypeValue;

                // Parse the string to get the enum value
                if (Enum.TryParse<DischargeType>(selectedDischargeTypeString.Replace(" ", ""), out dischargeTypeValue))
                {
                    dischargeInfo.DischargeType = dischargeTypeValue;
                }
                DischargeService dischargeService = new DischargeService();
                await dischargeService.CreateAsync(dischargeInfo);

                //update the patient to be outpaitnet
                _selectedPatient.Status = OUTIN_Patient.Out_Patient;
                PatientServices patientServices = new PatientServices();
                await patientServices.UpdatePatientAsync(_selectedPatient);

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


        private async void Close(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(InpatientDoctorDashboard));
        }
    }
} 