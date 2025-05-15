using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace E_Vita.Views;

public partial class WardRounds : ContentPage
{
    private ObservableCollection<WardRound> wardRounds;
    private ObservableCollection<WardRound> filteredWardRounds;
    private WardRoundsViewModel viewModel;
    private WardRound selectedRound;

    public WardRounds()
    {
        InitializeComponent();
        viewModel = new WardRoundsViewModel();
        BindingContext = viewModel;
        InitializeWardRounds();
    }

    private void InitializeWardRounds()
    {
        // Sample data - replace with actual data from your database
        wardRounds = new ObservableCollection<WardRound>
    {
        new WardRound
        {
            Time = "09:00 AM",
            Ward = "Ward A",
            Doctor = "Dr. Smith",
            Status = "Scheduled",
            Date = DateTime.Today,
            Notes = "",
            Patients = new ObservableCollection<PatientForRound>
            {
                new PatientForRound { PatientId = "P001", PatientName = "Ahmed Mohamed", RoomNumber = "A101", Condition = "Stable", Visited = false },
                new PatientForRound { PatientId = "P002", PatientName = "Fatima Ali", RoomNumber = "A102", Condition = "Improving", Visited = false },
                new PatientForRound { PatientId = "P003", PatientName = "Ibrahim Hassan", RoomNumber = "A103", Condition = "Critical", Visited = false }
            }
        },
        new WardRound
        {
            Time = "11:00 AM",
            Ward = "Ward B",
            Doctor = "Dr. Johnson",
            Status = "In Progress",
            Date = DateTime.Today,
            Notes = "Patient in A205 needs additional tests",
            Patients = new ObservableCollection<PatientForRound>
            {
                new PatientForRound { PatientId = "P004", PatientName = "Layla Karim", RoomNumber = "B201", Condition = "Stable", Visited = true },
                new PatientForRound { PatientId = "P005", PatientName = "Omar Youssef", RoomNumber = "B202", Condition = "Critical", Visited = false },
                new PatientForRound { PatientId = "P006", PatientName = "Noor Mahmoud", RoomNumber = "B203", Condition = "Improving", Visited = false }
            }
        },
        new WardRound
        {
            Time = "02:00 PM",
            Ward = "Ward C",
            Doctor = "Dr. Williams",
            Status = "Completed",
            Date = DateTime.Today,
            Notes = "All patients stable, follow-up scheduled for tomorrow",
            Patients = new ObservableCollection<PatientForRound>
            {
                new PatientForRound { PatientId = "P007", PatientName = "Kareem Saleh", RoomNumber = "C301", Condition = "Stable", Visited = true },
                new PatientForRound { PatientId = "P008", PatientName = "Amina Farouk", RoomNumber = "C302", Condition = "Improving", Visited = true },
                new PatientForRound { PatientId = "P009", PatientName = "Ziad Ahmed", RoomNumber = "C303", Condition = "Stable", Visited = true }
            }
        }
    };

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

        // Add new ward round to the collection
        var newRound = new WardRound
        {
            Time = time,
            Ward = ward,
            Doctor = doctor,
            Status = "Scheduled",
            Date = WardRoundsCalendar.SelectedDate != null ?
                   (DateTime)WardRoundsCalendar.SelectedDate : DateTime.Today,
            Notes = "",
            Patients = new ObservableCollection<PatientForRound>()
        };

        wardRounds.Add(newRound);
        filteredWardRounds.Add(newRound);

        await DisplayAlert("Success", "New ward round scheduled successfully!", "OK");
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
        var patientsToAdd = GetPatientsByIds(idArray);

        // Initialize Patients collection if it's null
        if (selectedRound.Patients == null)
        {
            selectedRound.Patients = new ObservableCollection<PatientForRound>();
        }

        int addedCount = 0;

        // Add the patients to the ward round
        foreach (var patient in patientsToAdd)
        {
            // Add only if not already in the list
            if (!selectedRound.Patients.Any(p => p.PatientId == patient.PatientId))
            {
                selectedRound.Patients.Add(patient);
                addedCount++;
            }
        }

        // Refresh the patients list
        PatientsList.ItemsSource = null;
        PatientsList.ItemsSource = selectedRound.Patients;

        await DisplayAlert("Success", $"Added {addedCount} patient(s) to the round", "OK");
    }

    // Method to get patients by their IDs
    private List<PatientForRound> GetPatientsByIds(string[] patientIds)
    {
        // This would typically query your database
        // For demo purposes, we'll simulate looking up patients from a sample dataset
        var allPatients = GetAllHospitalPatients();

        return allPatients.Where(p => patientIds.Contains(p.PatientId)).ToList();
    }

    // Method to get all hospital patients (simulated database)
    private List<PatientForRound> GetAllHospitalPatients()
    {
        // This would typically come from your database
        // For demo purposes, return sample data with IDs
        return new List<PatientForRound>
    {
        new PatientForRound { PatientId = "P001", PatientName = "Saeed Ahmed", RoomNumber = "A101", Condition = "Stable", Visited = false },
        new PatientForRound { PatientId = "P002", PatientName = "Maha Omar", RoomNumber = "A102", Condition = "Improving", Visited = false },
        new PatientForRound { PatientId = "P003", PatientName = "Jamal Karim", RoomNumber = "B103", Condition = "Critical", Visited = false },
        new PatientForRound { PatientId = "P004", PatientName = "Leila Ali", RoomNumber = "B104", Condition = "Stable", Visited = false },
        new PatientForRound { PatientId = "P005", PatientName = "Ahmed Mohamed", RoomNumber = "C201", Condition = "Stable", Visited = false },
        new PatientForRound { PatientId = "P006", PatientName = "Fatima Hassan", RoomNumber = "C202", Condition = "Critical", Visited = false },
        new PatientForRound { PatientId = "P007", PatientName = "Omar Youssef", RoomNumber = "C203", Condition = "Improving", Visited = false }
    };
    }

}

// WardRound Model (extended)
public class WardRound
{
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
