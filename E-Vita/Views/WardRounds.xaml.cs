using E_Vita.Services;
using E_Vita_APIs.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace E_Vita.Views;

public partial class WardRounds : ContentPage
{
    private ObservableCollection<WardRound> wardRounds;
    private ObservableCollection<WardRound> filteredWardRounds;
    private WardRoundsViewModel viewModel;
    private WardRound selectedRound;
    //------------------------------------------------------------------------------------------------
    private List<Patient> patients = new List<Patient>();
    public WardRounds()
    {
        InitializeComponent();
        viewModel = new WardRoundsViewModel();
        BindingContext = viewModel;
        InitializeWardRounds();
        loadPatients();
    }

    private async void loadPatients()
    {
        PatientServices patientServices = new PatientServices();
        var listofpatients = await patientServices.GetPatientsAsync();
        listofpatients = listofpatients.FindAll(p => p.Status == OUTIN_Patient.In_Patient);
        patients = listofpatients;
    }

    private async void InitializeWardRounds()
    {
        // Fetch data from the database using WardroundServices
        var wardroundService = new WardroundServices();
        var roundsFromDb = await wardroundService.GetAllAsync();
        PractitionerServices practitionerServices = new PractitionerServices();
        var allpract = await practitionerServices.GetPractitionersAsync();

        // Map DB model to UI model
        wardRounds = new ObservableCollection<WardRound>();
        foreach (var dbRound in roundsFromDb)
        {
            // Map properties from DB model to UI model
            var uiRound = new WardRound
            {
                Id = dbRound.Id,
                // Convert TimeOnly to string for display
                Time = dbRound.Time.ToString("hh\\:mm tt"),
                Ward = "", // You may need to fetch or map the ward name if available
                Doctor = dbRound.PractitionerID != null ? allpract.Find(p => p.Id == dbRound.PractitionerID)?.Name ?? "Unknown" : "Unknown",
                Status = "Scheduled",
                Date = dbRound.Date,
                Notes = dbRound.Note,
                Patients = new ObservableCollection<PatientForRound>() // You may need to fetch patients for the round if available
            };

            // Load patients for this ward round
            var patientsForRound = patients.Where(p => p.WardRoundId == dbRound.Id && p.Status == OUTIN_Patient.In_Patient).ToList();
            if (patientsForRound.Any())
            {
                RoomService roomService = new RoomService();
                var rooms = await roomService.GetAllAsync();

                foreach (var patient in patientsForRound)
                {
                    var room = rooms.FirstOrDefault(r => r.PatientId == patient.ID);
                    uiRound.Patients.Add(new PatientForRound
                    {
                        PatientId = patient.ID.ToString(),
                        PatientName = patient.Name,
                        RoomNumber = room != null ? room.RoomNumber.ToString() : "Unknown",
                        Condition = "Unknown", // You may want to store/retrieve this information
                        Visited = false // You may want to store/retrieve this information
                    });
                }
            }

            wardRounds.Add(uiRound);
        }

        // Initialize filtered data
        filteredWardRounds = new ObservableCollection<WardRound>(wardRounds);
        viewModel.FilteredWardRounds = filteredWardRounds;
    }


    private void OnCalendarDateSelected(object sender, Syncfusion.Maui.Calendar.CalendarSelectionChangedEventArgs e)
    {
        if (e.NewValue != null)
        {
            DateTime selectedDate = (DateTime)e.NewValue;

            // Filter ward rounds for selected date
            FilterRoundsByDate(selectedDate);
        }
    }

    private void FilterRoundsByDate(DateTime date)
    {
        // Filter rounds by date
        var filtered = wardRounds.Where(r => r.Date.Date == date.Date).ToList();

        // Update the filtered collection
        filteredWardRounds.Clear();
        foreach (var round in filtered)
        {
            filteredWardRounds.Add(round);
        }

        // Update view model
        viewModel.FilteredWardRounds = new ObservableCollection<WardRound>(filteredWardRounds);
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        // Auto-filter as user types (optional, could be triggered by button instead)
        ApplyFilter(SearchEntry.Text);
    }

    private void FilterButton_Clicked(object sender, EventArgs e)
    {
        ApplyFilter(SearchEntry.Text);
    }

    private void ApplyFilter(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            // If search is empty, show all rounds for the selected date
            if (WardRoundsCalendar.SelectedDate != null)
            {
                FilterRoundsByDate((DateTime)WardRoundsCalendar.SelectedDate);
            }
            else
            {
                // No date selected, show all rounds
                filteredWardRounds = new ObservableCollection<WardRound>(wardRounds);
                viewModel.FilteredWardRounds = filteredWardRounds;
            }
            return;
        }

        // Filter by search text (case insensitive)
        searchText = searchText.ToLower();
        var filtered = wardRounds.Where(r =>
            r.Doctor.ToLower().Contains(searchText) ||
            r.Ward.ToLower().Contains(searchText) ||
            r.Status.ToLower().Contains(searchText)).ToList();

        // Update the filtered collection
        filteredWardRounds.Clear();
        foreach (var round in filtered)
        {
            filteredWardRounds.Add(round);
        }

        // Update view model
        viewModel.FilteredWardRounds = new ObservableCollection<WardRound>(filteredWardRounds);
    }

    private void AllFilter_Clicked(object sender, EventArgs e)
    {
        if (WardRoundsCalendar.SelectedDate != null)
        {
            FilterRoundsByDate((DateTime)WardRoundsCalendar.SelectedDate);
        }
        else
        {
            // No date selected, show all rounds
            filteredWardRounds = new ObservableCollection<WardRound>(wardRounds);
            viewModel.FilteredWardRounds = filteredWardRounds;
        }
    }

    private void ScheduledFilter_Clicked(object sender, EventArgs e)
    {
        FilterByStatus("Scheduled");
    }

    private void InProgressFilter_Clicked(object sender, EventArgs e)
    {
        FilterByStatus("In Progress");
    }

    private void CompletedFilter_Clicked(object sender, EventArgs e)
    {
        FilterByStatus("Completed");
    }

    private void FilterByStatus(string status)
    {
        var baseCollection = wardRounds;

        // If date is selected, first filter by date
        if (WardRoundsCalendar.SelectedDate != null)
        {
            DateTime selectedDate = (DateTime)WardRoundsCalendar.SelectedDate;
            baseCollection = new ObservableCollection<WardRound>(
                wardRounds.Where(r => r.Date.Date == selectedDate.Date));
        }

        // Then filter by status
        var filtered = baseCollection.Where(r => r.Status == status).ToList();

        // Update the filtered collection
        filteredWardRounds.Clear();
        foreach (var round in filtered)
        {
            filteredWardRounds.Add(round);
        }

        // Update view model
        viewModel.FilteredWardRounds = new ObservableCollection<WardRound>(filteredWardRounds);
    }

    private void WardRoundsGrid_SelectionChanged(object sender, Syncfusion.Maui.DataGrid.DataGridSelectionChangedEventArgs e)
    {
        if (e.AddedRows.Count > 0)
        {
            // Get the selected ward round
            selectedRound = (WardRound)e.AddedRows[0];
            ShowRoundDetails(selectedRound);
        }
    }

    private void ShowRoundDetails(WardRound round)
    {
        // Populate the details in the popup
        DetailTime.Text = round.Time;
        DetailWard.Text = round.Ward;
        DetailDoctor.Text = round.Doctor;
        RoundNotes.Text = round.Notes;

        // Populate patients list
        PatientsList.ItemsSource = round.Patients;

        // Show the popup
        DetailsPopup.IsVisible = true;
    }

    private void CloseDetailsPopup_Clicked(object sender, EventArgs e)
    {
        DetailsPopup.IsVisible = false;
    }

    private async void UpdateStatus_Clicked(object sender, EventArgs e)
    {
        if (selectedRound == null) return;

        // Show action sheet to select status
        string action = await DisplayActionSheet("Update Status", "Cancel", null, "Scheduled", "In Progress", "Completed");

        if (action != "Cancel" && action != null)
        {
            // Update the status
            selectedRound.Status = action;

            // Refresh the grid
            WardRoundsGrid.Refresh();

            await DisplayAlert("Success", $"Status updated to: {action}", "OK");
        }
    }

    private async void SaveNotes_Clicked(object sender, EventArgs e)
    {
        if (selectedRound == null) return;

        // Update notes
        selectedRound.Notes = RoundNotes.Text;

        await DisplayAlert("Success", "Notes saved successfully", "OK");
    }

    private async void ScheduleNewRound_Click(object sender, EventArgs e)
    {
        // Show a form to schedule a new ward round
        string ward = await DisplayPromptAsync("New Ward Round", "Enter Ward:");
        if (string.IsNullOrWhiteSpace(ward)) return;

        string time = await DisplayPromptAsync("New Ward Round", "Enter Time (HH:MM AM/PM):");
        if (string.IsNullOrWhiteSpace(time)) return;

        string doctor = await DisplayPromptAsync("New Ward Round", "Enter Doctor Name:");
        if (string.IsNullOrWhiteSpace(doctor)) return;

        // Find the practitioner by name (assuming unique names)
        PractitionerServices practitionerServices = new PractitionerServices();
        var practitioners = await practitionerServices.GetPractitionersAsync();
        var selectedPractitioner = practitioners.FirstOrDefault(p => p.Name.Equals(doctor, StringComparison.OrdinalIgnoreCase));
        if (selectedPractitioner == null)
        {
            await DisplayAlert("Error", "Doctor not found. Please ensure the doctor is registered.", "OK");
            return;
        }

        // Parse time to TimeOnly
        if (!TimeOnly.TryParseExact(time, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out var parsedTime))
        {
            await DisplayAlert("Error", "Invalid time format. Please use HH:MM AM/PM.", "OK");
            return;
        }

        // Create backend model
        var newWardRound = new E_Vita_APIs.Models.WardRound
        {
            Date = WardRoundsCalendar.SelectedDate != null ? (DateTime)WardRoundsCalendar.SelectedDate : DateTime.Today,
            Time = parsedTime,
            Note = RoundNotes.Text, // Add notes if needed
            PractitionerID = selectedPractitioner.Id
        };
        // Save to database via API
        var wardroundService = new WardroundServices();
        var createdRound = await wardroundService.AddAsyncreturnID(newWardRound);

        if (createdRound != null)
        {
            // Add to UI collection
            var uiRound = new WardRound
            {
                Id = createdRound.Id, // Now you have the real Id
                Time = parsedTime.ToString("hh\\:mm tt"),
                Ward = ward,
                Doctor = doctor,
                Status = "Scheduled",
                Date = newWardRound.Date,
                Notes = "",
                Patients = new ObservableCollection<PatientForRound>()
            };
            wardRounds.Add(uiRound);
            filteredWardRounds.Add(uiRound);

            await DisplayAlert("Success", "New ward round scheduled and saved to the database!", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Failed to save ward round to the database.", "OK");
        }
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(InpatientDoctorDashboard));
    }

    // Add the missing AddPatient_Clicked method
    private async void AddPatient_Clicked(object sender, EventArgs e)
    {
        if (selectedRound == null) return;

        // Get patient details using prompts
        string patientId = await DisplayPromptAsync("Add Patient", "Enter Patient ID:");
        if (string.IsNullOrWhiteSpace(patientId)) return;

        string patientName = await DisplayPromptAsync("Add Patient", "Enter Patient Name:");
        if (string.IsNullOrWhiteSpace(patientName)) return;

        string roomNumber = await DisplayPromptAsync("Add Patient", "Enter Room Number:");
        if (string.IsNullOrWhiteSpace(roomNumber)) return;

        string condition = await DisplayActionSheet("Select Condition", "Cancel", null,
            "Stable", "Improving", "Critical");
        if (condition == "Cancel" || condition == null) return;

        // Create new patient and add to the round
        var newPatient = new PatientForRound
        {
            PatientId = patientId,
            PatientName = patientName,
            RoomNumber = roomNumber,
            Condition = condition,
            Visited = false
        };

        // Initialize Patients collection if it's null
        if (selectedRound.Patients == null)
        {
            selectedRound.Patients = new ObservableCollection<PatientForRound>();
        }

        // Check if patient with same ID already exists
        if (selectedRound.Patients.Any(p => p.PatientId == patientId))
        {
            await DisplayAlert("Warning", $"Patient with ID {patientId} already exists in this round", "OK");
            return;
        }

        // Add the patient to the collection
        selectedRound.Patients.Add(newPatient);
        // Persist the patient-round relationship
        var patient = patients.FirstOrDefault(p => p.ID.ToString() == patientId);
        if (patient != null)
        {
            patient.WardRoundId = selectedRound.Id;
            PatientServices patientService = new PatientServices();
            await patientService.UpdatePatientAsync(patient); // Implement this API call in your service
        }
        // Refresh the patients list
        PatientsList.ItemsSource = null;
        PatientsList.ItemsSource = selectedRound.Patients;

        await DisplayAlert("Success", $"Patient {patientName} added to the round", "OK");
    }

    // Move these methods from the ViewModel class to here
    private async void SelectExistingPatients_Clicked(object sender, EventArgs e)
    {
        if (selectedRound == null) return;

        // Show prompt for entering patient IDs
        string patientIds = await DisplayPromptAsync("Select Patients",
            "Enter Patient IDs (comma-separated):",
            maxLength: 100,
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(patientIds)) return;

        // Parse the comma-separated list of IDs
        string[] idArray = patientIds.Split(',', StringSplitOptions.RemoveEmptyEntries);

        // Trim whitespace from each ID
        for (int i = 0; i < idArray.Length; i++)
        {
            idArray[i] = idArray[i].Trim();
        }

        // Get patients by their IDs
        var patientsToAdd = await GetPatientsByIds(idArray); // Await the Task to get the result

        // Initialize Patients collection if it's null
        if (selectedRound.Patients == null)
        {
            selectedRound.Patients = new ObservableCollection<PatientForRound>();
        }

        int addedCount = 0;

        // Add the patients to the ward round
        foreach (var patient in patientsToAdd) // Now patientsToAdd is a List<PatientForRound>
        {
            // Add only if not already in the list
            if (!selectedRound.Patients.Any(p => p.PatientId == patient.PatientId))
            {
                selectedRound.Patients.Add(patient);

                // Persist the patient-round relationship
                var dbPatient = patients.FirstOrDefault(p => p.ID.ToString() == patient.PatientId);
                if (dbPatient != null)
                {
                    dbPatient.WardRoundId = selectedRound.Id;
                    PatientServices patientService = new PatientServices();
                    await patientService.UpdatePatientAsync(dbPatient); // Implement this API call in your service
                }

                addedCount++;
            }
        }

        // Refresh the patients list
        PatientsList.ItemsSource = null;
        PatientsList.ItemsSource = selectedRound.Patients;

        await DisplayAlert("Success", $"Added {addedCount} patient(s) to the round", "OK");
    }

    // Fix for CS0029 and CS8601 errors in the GetPatientsByIds method
    private async Task<List<PatientForRound>> GetPatientsByIds(string[] patientIds)
    {
        // Ensure the patientIds array is not null
        if (patientIds == null || patientIds.Length == 0)
        {
            return new List<PatientForRound>();
        }

        RoomService roomService = new RoomService();
        var rooms = await roomService.GetAllAsync();

        // Filter patients based on the provided IDs
        var filteredPatients = patients
            .Where(p => patientIds.Contains(p.ID.ToString()))
            .Select(p =>
            {
                // Find the room associated with the patient
                var room = rooms.FirstOrDefault(r => r.PatientId == p.ID);

                return new PatientForRound
                {
                    PatientId = p.ID.ToString(),
                    PatientName = p.Name,
                    RoomNumber = room != null ? room.RoomNumber.ToString() : "Unknown", // Convert RoomNumber to string
                    Condition = "Unknown", // Default value, update as needed
                    Visited = false
                };
            })
            .ToList();

        return filteredPatients;
    }
}

// WardRound Model (extended)
public class WardRound
{
    public int Id { get; set; }
    public string Time { get; set; }
    public string Ward { get; set; }
    public string Doctor { get; set; }
    public string Status { get; set; }
    public DateTime Date { get; set; }
    public string Notes { get; set; }
    public ObservableCollection<PatientForRound> Patients { get; set; }
}

// Patient for Round model
public class PatientForRound
{
    public string PatientId { get; set; }  // Add this property
    public string PatientName { get; set; }
    public string RoomNumber { get; set; }
    public string Condition { get; set; }
    public bool Visited { get; set; }
}


// Base ViewModel
public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

// WardRounds ViewModel (extended)
public class WardRoundsViewModel : BaseViewModel
{
    private ObservableCollection<WardRound> _wardRounds;
    public ObservableCollection<WardRound> WardRounds
    {
        get => _wardRounds;
        set => SetProperty(ref _wardRounds, value);
    }

    private ObservableCollection<WardRound> _filteredWardRounds;
    public ObservableCollection<WardRound> FilteredWardRounds
    {
        get => _filteredWardRounds;
        set => SetProperty(ref _filteredWardRounds, value);
    }

    public WardRoundsViewModel()
    {
        WardRounds = new ObservableCollection<WardRound>();
        FilteredWardRounds = new ObservableCollection<WardRound>();
        LoadWardRounds();
    }

    private void LoadWardRounds()
    {
        // Here you would typically load data from your database or service
        // This is just sample data
        WardRounds.Add(new WardRound
        {
            Time = "09:00 AM",
            Ward = "Ward A",
            Doctor = "Dr. Smith",
            Status = "Scheduled",
            Date = DateTime.Today
        });
        // Add more sample data as needed
    }
}
