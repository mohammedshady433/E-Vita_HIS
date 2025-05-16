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
            new Person { ID = "P001", Name = "John Smith" },
            new Person { ID = "P002", Name = "Mary Johnson" },
            new Person { ID = "P003", Name = "Robert Brown" },
            new Person { ID = "P004", Name = "Sarah Davis" },
            new Person { ID = "P005", Name = "Michael Wilson" }
        };

        _doctors = new ObservableCollection<Person>
        {
            new Person { ID = "D001", Name = "Dr. James Wilson" },
            new Person { ID = "D002", Name = "Dr. Emily Parker" },
            new Person { ID = "D003", Name = "Dr. Robert Chen" },
            new Person { ID = "D004", Name = "Dr. Lisa Anderson" },
            new Person { ID = "D005", Name = "Dr. David Miller" }
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
		await Navigation.PopAsync();
	}
}

public class Person
{
    public string ID { get; set; }
    public string Name { get; set; }
}