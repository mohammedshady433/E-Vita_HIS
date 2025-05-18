using System.Collections.ObjectModel;
using System.Linq;

namespace E_Vita.Views;

public partial class Room_Reservation : ContentPage
{
    private ObservableCollection<string> _allRooms; // Stores all rooms
    private ObservableCollection<string> _availableRooms; // Stores only available rooms
    private Dictionary<string, bool> _roomAvailability;
    private ObservableCollection<Person> _patients;
    private ObservableCollection<Person> _doctors;
    private Person _selectedPatient;
    private Person _selectedDoctor;

    public bool IsPatientSearchVisible { get; set; }
    public bool IsDoctorSearchVisible { get; set; }

    public Room_Reservation()
	{
		InitializeComponent();
		InitializeRooms();
		InitializePeople();
		BindingContext = this;
	}

	private void InitializePeople()
	{
        // Initialize with some sample data - replace with your actual data source
        _patients = new ObservableCollection<Person>
        {
            new Person { ID = "200", Name = "Mohammed Shady" },
            new Person { ID = "201", Name = "Mohammed Hesham" },
            new Person { ID = "202", Name = "Shahd Mostafa" },
            new Person { ID = "203", Name = "Mohammed Hamaki" },
            new Person { ID = "204", Name = "Sausan Badr" }
        };

        _doctors = new ObservableCollection<Person>
        {
            new Person { ID = "605", Name = "Dr. Sahar Fawzi" },
            new Person { ID = "654", Name = "Dr. Sandy Melad" },
            new Person { ID = "632", Name = "Dr. Shady Mohammed" },
            new Person { ID = "614", Name = "Dr. Dalia Saudi" },
            new Person { ID = "678", Name = "Dr. Ali Rabia" }
        };
    }

	private void InitializeRooms()
	{
        _allRooms = new ObservableCollection<string>();
        _availableRooms = new ObservableCollection<string>();
        _roomAvailability = new Dictionary<string, bool>();

        // Generate rooms for 4 floors (1-4), 20 rooms per floor
        for (int floor = 1; floor <= 4; floor++)
		{
			for (int room = 1; room <= 20; room++)
			{
				// Format: floor number (1 digit) + room number (2 digits)
				string roomNumber = $"{floor}{room:D2}";
				_allRooms.Add(roomNumber);
                _availableRooms.Add(roomNumber);
                _roomAvailability[roomNumber] = true;
            }
		}

        avialable_rooms.ItemsSource = _availableRooms;
    }

    private void UpdateAvailableRooms()
    {
        _availableRooms.Clear();
        foreach (var room in _allRooms)
        {
            if (_roomAvailability[room])
            {
                _availableRooms.Add(room);
            }
        }
    }

    private void OnPatientSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? string.Empty;
        var filteredPatients = _patients.Where(p => 
            p.Name.ToLower().Contains(searchText) || 
            p.ID.ToLower().Contains(searchText)).ToList();
        
        patientSearchResults.ItemsSource = filteredPatients;
        IsPatientSearchVisible = !string.IsNullOrEmpty(searchText);
        OnPropertyChanged(nameof(IsPatientSearchVisible));
    }

    private void OnDoctorSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? string.Empty;
        var filteredDoctors = _doctors.Where(d => 
            d.Name.ToLower().Contains(searchText) || 
            d.ID.ToLower().Contains(searchText)).ToList();
        
        doctorSearchResults.ItemsSource = filteredDoctors;
        IsDoctorSearchVisible = !string.IsNullOrEmpty(searchText);
        OnPropertyChanged(nameof(IsDoctorSearchVisible));
    }

    private void OnPatientSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Person selectedPatient)
        {
            _selectedPatient = selectedPatient;
            selectedPatientLabel.Text = $"Selected Patient: {selectedPatient.Name} ({selectedPatient.ID})";
            patientSearchBar.Text = string.Empty;
            IsPatientSearchVisible = false;
            OnPropertyChanged(nameof(IsPatientSearchVisible));
        }
    }

    private void OnDoctorSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Person selectedDoctor)
        {
            _selectedDoctor = selectedDoctor;
            selectedDoctorLabel.Text = $"Selected Doctor: {selectedDoctor.Name} ({selectedDoctor.ID})";
            doctorSearchBar.Text = string.Empty;
            IsDoctorSearchVisible = false;
            OnPropertyChanged(nameof(IsDoctorSearchVisible));
        }
    }

    private async void SavePatient(object sender, EventArgs e)
	{
		string selectedRoom = avialable_rooms.SelectedItem?.ToString();

		if (string.IsNullOrEmpty(selectedRoom))
		{
			await DisplayAlert("Error", "Please select a room", "OK");
			return;
		}

		if (_selectedPatient == null)
		{
			await DisplayAlert("Error", "Please select a patient", "OK");
			return;
		}

		if (_selectedDoctor == null)
		{
			await DisplayAlert("Error", "Please select a doctor", "OK");
			return;
		}

		if (!_roomAvailability[selectedRoom])
		{
			await DisplayAlert("Error", "This room is already occupied", "OK");
			return;
		}

		_roomAvailability[selectedRoom] = false;
        UpdateAvailableRooms();
        await DisplayAlert("Success", $"Room {selectedRoom} has been reserved for Patient {_selectedPatient.Name}", "OK");
		
		// Clear the form
		_selectedPatient = null;
		_selectedDoctor = null;
		selectedPatientLabel.Text = "Selected Patient: None";
		selectedDoctorLabel.Text = "Selected Doctor: None";
		avialable_rooms.SelectedItem = null;
	}

    private async void close(object sender, EventArgs e)
    {
        // Corrected the usage of Shell navigation
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
    }
}

public class Person
{
    public string ID { get; set; }
    public string Name { get; set; }
}