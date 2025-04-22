using System.Collections.ObjectModel;
using System.Linq;

namespace E_Vita.Views;

public partial class CancelAppointment : ContentPage
{
    public ObservableCollection<AppointmentModel> ScheduleList { get; set; }

    // The master list of all appointments
    public List<AppointmentModel> AllAppointments { get; set; } = new List<AppointmentModel>();

    public CancelAppointment()
    {
        InitializeComponent();
        AllAppointments = new List<AppointmentModel> { };

        // Dummy data for testing  
        ScheduleList = new ObservableCollection<AppointmentModel>
        {
            new AppointmentModel
            {
                Time = "10:30 AM",
                Patient_appointment = new Patient { Patient_ID = "123", Patient_Name = "John Doe", Time = "10:00 AM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Smith", ID = "001", Specialty = "Cardiology" }
            },
            new AppointmentModel
            {
                Time = "11:00 AM",
                Patient_appointment = new Patient { Patient_ID = "456", Patient_Name = "Alice Johnson", Time = "11:00 AM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Johnson", ID = "002", Specialty = "Neurology" }
            },
            new AppointmentModel
            {
                Time = "11:30 AM",
                Patient_appointment = new Patient { Patient_ID = "789", Patient_Name = "Mark Lee", Time = "11:30 AM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Smith", ID = "001", Specialty = "Neurology" }
            },
            new AppointmentModel
            {
                Time = "12:00 PM",
                Patient_appointment = new Patient { Patient_ID = "234", Patient_Name = "Nina Patel", Time = "12:00 PM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Johnson", ID = "002", Specialty = "Neurology" }
            },
            new AppointmentModel
            {
                Time = "12:30 PM",
                Patient_appointment = new Patient { Patient_ID = "867", Patient_Name = "Tom Hardy", Time = "12:30 PM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Smith", ID = "001", Specialty = "Cardiology" }
            },
            new AppointmentModel
            {
                Time = "01:00 PM",
                Patient_appointment = new Patient { Patient_ID = "476", Patient_Name = "Emma Stone", Time = "01:00 PM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Smith", ID = "001", Specialty = "Neurology" }
            },
            new AppointmentModel
            {
                Time = "01:30 PM",
                Patient_appointment = new Patient { Patient_ID = "453", Patient_Name = "Luke Wilson", Time = "01:30 PM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Johnson", ID = "002", Specialty = "Neurology" }
            },
            new AppointmentModel
            {
                Time = "02:00 PM",
                Patient_appointment = new Patient { Patient_ID = "456", Patient_Name = "Alice Johnson", Time = "02:00 PM" },
                Doctor_appointment = new Doctor { Doctor_Name = "Dr. Smith", ID = "001", Specialty = "Neurology" }
            }
        };

        BindingContext = this;
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
            await DisplayAlert("Deleted", "Appointment has been deleted.", "OK");
        }
    }

    // Search appointment button
    private void OnSearchClicked(object sender, EventArgs e)
    {
        string searchText = searchEntry?.Text?.Trim(); // Prevent null reference

        if (string.IsNullOrEmpty(searchText))
        {
            DisplayAlert("Invalid Input", "Please enter a Patient ID or Doctor ID.", "OK");
            return;
        }

        if (!searchText.All(char.IsDigit))
        {
            DisplayAlert("Invalid Input", "Only digits are allowed in the search field.", "OK");
            return;
        }

        // Filter appointments by Patient ID or Doctor ID
        var filtered = AllAppointments.Where(appt =>
            appt.Patient_appointment.Patient_ID.Equals(searchText, StringComparison.OrdinalIgnoreCase) ||
            appt.Doctor_appointment.ID.Equals(searchText, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        ScheduleList.Clear(); // Clear previous items
        foreach (var item in filtered)
        {
            ScheduleList.Add(item); // Add filtered results
        }
    }

    //navigate back to ReceptionistDashboard
    private async void OnBackClicked (object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ReceptionistDashboard());
        
    }
}

// Models  
public class AppointmentModel
{
    public string Time { get; set; } = string.Empty;
    public Patient Patient_appointment { get; set; } = new Patient();
    public Doctor Doctor_appointment { get; set; } = new Doctor();
}

public class Patient
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

