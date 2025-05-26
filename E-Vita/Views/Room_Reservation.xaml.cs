using E_Vita.Services;
using E_Vita_APIs.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace E_Vita.Views;

public partial class Room_Reservation : ContentPage
{
    private ObservableCollection<string> _allRooms; // Stores all rooms
    private ObservableCollection<string> _availableRooms; // Stores only available rooms
    private Dictionary<string, bool> _roomAvailability;
    //-------------------------------------------------------------
    private Practitioner _selectedNurse;
    private List<Practitioner> filteredNurses = new List<Practitioner>();
    //-------------------------------------------------------------
    public bool IsNurseSearchVisible { get; set; }
    private Practitioner _selectedPatient;
    private Practitioner _selectedDoctor;
    private List<Patient> patients;
    private List<Practitioner> practitioners = new List<Practitioner>();
    private List<Practitioner> filtereddoctors = new List<Practitioner>();

    public bool IsPatientSearchVisible { get; set; }
    public bool IsDoctorSearchVisible { get; set; }

    public Room_Reservation()
	{
		InitializeComponent();
		InitializeRooms();
        LoadPatients();
        LoadNurses();
        LoadDoctors();
        BindingContext = this;
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
    // Add this method to the constructor after InitializeComponent();
    private async void LoadNurses()
    {

        PractitionerServices nurseServices = new PractitionerServices();
        var nursePractitioners = await nurseServices.GetPractitionersAsync(); // Use a separate variable
        PractitionerRoleService roleService = new PractitionerRoleService();
        var allpractRolesList = await roleService.GetPractitionerRolesAsync();

        // Clear the list before adding new items
        filteredNurses.Clear();

        foreach (var role in allpractRolesList)
        {
            if (role.Service == Service.Nurse)
            {
                var nurse = nursePractitioners.FirstOrDefault(n => n.Id == role.PractitionerId);
                if (nurse != null)
                {
                    filteredNurses.Add(new Practitioner
                    {
                        Id = nurse.Id,
                        Name = nurse.Name,
                    });
                }
            }
        }

        // Debug information
        Console.WriteLine($"Loaded {filteredNurses.Count} nurses");
    }

    private async void LoadDoctors()
    {
        PractitionerServices doctorServices = new PractitionerServices();
        practitioners = await doctorServices.GetPractitionersAsync();
        PractitionerRoleService roleService = new PractitionerRoleService();
        var allpractRolesList = await roleService.GetPractitionerRolesAsync();
        foreach (var role in allpractRolesList)
        {
            if (role.Service == Service.DoctorIN || role.Service == Service.DoctorOUT)
            {
                var doctor = practitioners.FirstOrDefault(d => d.Id == role.PractitionerId);
                if (doctor != null)
                {
                    filtereddoctors.Add(new Practitioner
                    {
                        Id = doctor.Id,
                        Name = doctor.Name,
                    });
                }
            }
        }
    }

    private async void LoadPatients()
    {
        PatientServices patientServices = new PatientServices();
        patients = await patientServices.GetPatientsAsync();

    }

    private void OnPatientSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            IsPatientSearchVisible = false;
            OnPropertyChanged(nameof(IsPatientSearchVisible));
            return;
        }

        var searchText = e.NewTextValue.ToLower();
        var filteredPatients = patients
            .Where(p => p.ID.ToString().Contains(searchText) || p.Name.ToLower().Contains(searchText))
            .Select(p => new Practitioner { Id = p.ID, Name = p.Name })
            .ToList();

        patientSearchResults.ItemsSource = filteredPatients;
        IsPatientSearchVisible = filteredPatients.Any();
        OnPropertyChanged(nameof(IsPatientSearchVisible));
    }


    private void OnDoctorSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            IsDoctorSearchVisible = false;
            OnPropertyChanged(nameof(IsDoctorSearchVisible));
            return;
        }

        var searchText = e.NewTextValue.ToLower();

        // If using a loaded list of doctors (from a service)
        if (filtereddoctors != null && filtereddoctors.Any())
        {
            var filteredDoctors = filtereddoctors
                .Where(d => d.Id.ToString().Contains(searchText) || d.Name.ToLower().Contains(searchText))
                .Select(d => new Practitioner { Id = d.Id, Name = d.Name })
                .ToList();

            doctorSearchResults.ItemsSource = filteredDoctors;
            IsDoctorSearchVisible = filteredDoctors.Any();
        }

        OnPropertyChanged(nameof(IsDoctorSearchVisible));
    }


    private void OnPatientSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Practitioner selectedPatient)
        {
            _selectedPatient = selectedPatient;
            selectedPatientLabel.Text = $"Selected Patient: {selectedPatient.Name} ({selectedPatient.Id})";
            patientSearchBar.Text = string.Empty;
            IsPatientSearchVisible = false;
            OnPropertyChanged(nameof(IsPatientSearchVisible));
        }
    }

    private void OnDoctorSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Practitioner selectedDoctor)
        {
            _selectedDoctor = selectedDoctor;
            selectedDoctorLabel.Text = $"Selected Doctor: {selectedDoctor.Name} ({selectedDoctor.Id})";
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
        if (_selectedNurse == null)
        {
            await DisplayAlert("Error", "Please select a nurse", "OK");
            return;
        }

        _roomAvailability[selectedRoom] = false;
        UpdateAvailableRooms();
        Room reserved_Room = new Room();
        reserved_Room.availablity = RoomStatus.Occupied;
        reserved_Room.RoomNumber = int.Parse(selectedRoom);
        reserved_Room.PatientId = _selectedPatient.Id;
        reserved_Room.Name = _selectedPatient.Name;
        reserved_Room.DoctorId = _selectedDoctor.Id;
        reserved_Room.NurseId = _selectedNurse.Id;  // Add the nurse ID to the room
        RoomService roomServices = new RoomService();
        await roomServices.AddAsync(reserved_Room);

        var patientupdateroom = patients.FirstOrDefault(p => p.ID == _selectedPatient.Id);
        patientupdateroom.Status = OUTIN_Patient.In_Patient;
        PatientServices patientServices = new PatientServices();
        await patientServices.UpdatePatientAsync(patientupdateroom);
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
    // Add this method to handle nurse search
    private void OnNurseSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            IsNurseSearchVisible = false;
            OnPropertyChanged(nameof(IsNurseSearchVisible));
            return;
        }

        var searchText = e.NewTextValue.ToLower();

        // If using a loaded list of nurses
        if (filteredNurses != null && filteredNurses.Any())
        {
            var filteredResults = filteredNurses
                .Where(n => n.Id.ToString().Contains(searchText) || n.Name.ToLower().Contains(searchText))
                .Select(n => new Practitioner { Id = n.Id, Name = n.Name })
                .ToList();

            nurseSearchResults.ItemsSource = filteredResults;
            IsNurseSearchVisible = filteredResults.Any();
        }

        OnPropertyChanged(nameof(IsNurseSearchVisible));
    }
    // Add this method to handle nurse selection
    private void OnNurseSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Practitioner selectedNurse)
        {
            _selectedNurse = selectedNurse;
            selectedNurseLabel.Text = $"Selected Nurse: {selectedNurse.Name} ({selectedNurse.Id})";
            nurseSearchBar.Text = string.Empty;
            IsNurseSearchVisible = false;
            OnPropertyChanged(nameof(IsNurseSearchVisible));
        }
    }
}

