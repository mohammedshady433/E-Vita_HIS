using System.Collections.ObjectModel;
using System.Linq;

namespace E_Vita.Views;

public partial class CancelAppointment : ContentPage
{
    public ObservableCollection<AppointmentModel> ScheduleList { get; set; }

    // The master list of all appointments
    private List<AppointmentModel> _allAppointments = new List<AppointmentModel>();

    public CancelAppointment()
    {
        InitializeComponent();
        _allAppointments = new List<AppointmentModel> { };

        // Dummy data for testing  
        _allAppointments = new List<AppointmentModel>
        {
            new AppointmentModel
            {
                Time = "10:30 AM",
                Patient_appointment = new Patientsooooo { Patient_ID = "123", Patient_Name = "John Doe", Time = "10:00 AM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Smith", ID = "001", Specialty = "Cardiology" }
            },
            new AppointmentModel
            {
                Time = "11:00 AM",
                Patient_appointment = new Patientsooooo { Patient_ID = "456", Patient_Name = "Alice Johnson", Time = "11:00 AM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Johnson", ID = "002", Specialty = "Neurology" }
            },
            new AppointmentModel
            {
                Time = "11:30 AM",
                Patient_appointment = new Patientsooooo { Patient_ID = "789", Patient_Name = "Mark Lee", Time = "11:30 AM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Smith", ID = "001", Specialty = "Neurology" }
            },
            new AppointmentModel
            {
                Time = "12:00 PM",
                Patient_appointment = new Patientsooooo { Patient_ID = "234", Patient_Name = "Nina Patel", Time = "12:00 PM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Johnson", ID = "002", Specialty = "Neurology" }
            },
            new AppointmentModel
            {
                Time = "12:30 PM",
                Patient_appointment = new Patientsooooo { Patient_ID = "867", Patient_Name = "Tom Hardy", Time = "12:30 PM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Smith", ID = "001", Specialty = "Cardiology" }
            },
            new AppointmentModel
            {
                Time = "01:00 PM",
                Patient_appointment = new Patientsooooo { Patient_ID = "476", Patient_Name = "Emma Stone", Time = "01:00 PM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Smith", ID = "001", Specialty = "Neurology" }
            },
            new AppointmentModel
            {
                Time = "01:30 PM",
                Patient_appointment = new Patientsooooo { Patient_ID = "453", Patient_Name = "Luke Wilson", Time = "01:30 PM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Johnson", ID = "002", Specialty = "Neurology" }
            },
            new AppointmentModel
            {
                Time = "02:00 PM",
                Patient_appointment = new Patientsooooo { Patient_ID = "456", Patient_Name = "Alice Johnson", Time = "02:00 PM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Smith", ID = "001", Specialty = "Neurology" }
            }
        };

        // Initialize ScheduleList with all appointments
        ScheduleList = new ObservableCollection<AppointmentModel>(_allAppointments);
        BindingContext = this;
    }

    // Dynamic search as user types
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(searchText))
        {
            // If search is empty, show all appointments
            ScheduleList.Clear();
            foreach (var appointment in _allAppointments)
            {
                ScheduleList.Add(appointment);
            }
            return;
        }

        // Filter appointments by Patient ID or Doctor ID
        var filtered = _allAppointments.Where(appt =>
            appt.Patient_appointment.Patient_ID.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
            appt.Doctor_appointment.ID.Contains(searchText, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        ScheduleList.Clear();
        foreach (var item in filtered)
        {
            ScheduleList.Add(item);
        }
    }

    // Delete appointment button 
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var appointment = button?.CommandParameter as AppointmentModel;

        if (appointment == null)
            return;

        bool isConfirmed = await DisplayAlert("Confirm Delete",
                                              $"Are you sure you want to delete the appointment for {appointment.Patient_appointment.Patient_Name}?",
                                              "Yes", "No");

        if (isConfirmed)
        {
            ScheduleList.Remove(appointment);
            _allAppointments.Remove(appointment);
            await DisplayAlert("Deleted", "Appointment has been deleted.", "OK");
        }
    }

    //navigate back to ReceptionistDashboard
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ReceptionistDashboard());
    }

    // No Show button handler
    private async void OnNoShowClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var appointment = button?.CommandParameter as AppointmentModel;

        if (appointment == null)
            return;

        bool isConfirmed = await DisplayAlert("Confirm No Show",
                                              $"Mark {appointment.Patient_appointment.Patient_Name}'s appointment as No Show?",
                                              "Yes", "No");

        if (isConfirmed)
        {
            appointment.IsNoShow = true;
            // You might want to update the button appearance here
            button.BackgroundColor = Colors.Gray;
            button.IsEnabled = false;
            await DisplayAlert("Updated", "Appointment has been marked as No Show.", "OK");
        }
    }
}

// Models  
public class AppointmentModel
{
    public string Time { get; set; } = string.Empty;
    public Patientsooooo Patient_appointment { get; set; } = new Patientsooooo();
    public Doctor Doctor_appointment { get; set; } = new Doctor();
    public bool IsNoShow { get; set; } = false;
}

 public class Patientsooooo
{
    public string Patient_ID { get; set; } = string.Empty;
    public string Patient_Name { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
}

public class Doctor
{
    public string Doctor_Name { get; set; } = string.Empty;
    public string ID { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
}

