using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Calendar;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
using E_Vita.Services;

namespace E_Vita.Views;

public partial class InpatientDoctorDashboard : ContentPage
{
    public ObservableCollection<Inpatient> Inpatients { get; set; }
    public ObservableCollection<CriticalAlert> CriticalAlerts { get; set; }
    public ObservableCollection<WardStatistic> WardStatistics { get; set; }
    public ObservableCollection<Patient> Appointments { get; set; } = new ObservableCollection<Patient>();
    private readonly AppointmentsService _appointmentsService = new AppointmentsService();
    private readonly PractitionerServices _practitionerService;
    private readonly PatientServices _patientService = new PatientServices();
    private readonly AppointmentPractitionerService _appointmentPractitionerService = new AppointmentPractitionerService();
    private int _currentDoctorId;
    public InpatientDoctorDashboard()
    {
        InitializeComponent();
        if (Preferences.ContainsKey("CurrentDoctorId"))
        {
            _currentDoctorId = Preferences.Get("CurrentDoctorId", 0);
            if (_currentDoctorId > 0)
            {
                LoadAppointmentsForDoctor(123456784);
            }
        }
        // Initialize collections
        Inpatients = new ObservableCollection<Inpatient>
        {
            new Inpatient { PatientName = "Mohammed Ali", RoomNumber = "301", AdmissionDate = "2024-03-15", Condition = "Stable" },
            new Inpatient { PatientName = "Fatima Hassan", RoomNumber = "302", AdmissionDate = "2024-03-14", Condition = "Critical" },
            new Inpatient { PatientName = "Ahmed Ibrahim", RoomNumber = "303", AdmissionDate = "2024-03-13", Condition = "Improving" }
        };

        CriticalAlerts = new ObservableCollection<CriticalAlert>
        {
            new CriticalAlert { Timestamp = "10:30 AM", AlertType = "Vital Signs", Description = "Blood pressure dropping in Room 302" },
            new CriticalAlert { Timestamp = "09:45 AM", AlertType = "Lab Results", Description = "Critical lab results for Patient in Room 301" },
            new CriticalAlert { Timestamp = "09:15 AM", AlertType = "Medication", Description = "Medication due for Room 303" }
        };

        WardStatistics = new ObservableCollection<WardStatistic>
        {
            new WardStatistic { Category = "Occupied Beds", Value = 15 },
            new WardStatistic { Category = "Available Beds", Value = 5 },
            new WardStatistic { Category = "Critical Patients", Value = 3 },
            new WardStatistic { Category = "Stable Patients", Value = 12 }
        };

        // Set the BindingContext
        this.BindingContext = this;
    }
    private async void LoadAppointmentsForDoctor(int doctorId)
    {
        try
        {
            //get all data from the AppointmentPractitionerService
            var appointmentPractitioners = await _appointmentPractitionerService.GetByPractitionerIdAsync(doctorId);
            //get the all data that is in AppointmentService by the Appointments IDs that is in the appointmentPractitioners
            var appointments = await _appointmentsService.GetAllAsync();
            // Filter appointments for this doctor
            var filteredAppointments = appointments
                .Where(a => appointmentPractitioners.Any(ap => ap.AppointmentId == a.Id))
                .ToList();
            // from the patint IDs in the filteredAppointments get all the patients data from the PatientService by this IDs
            var patients = await _patientService.GetPatientsAsync();
            // Clear the existing appointments
            Appointments?.Clear();
            if (filteredAppointments != null && filteredAppointments.Any())
            {
                foreach (var apiAppointment in filteredAppointments)
                {
                    var patient = patients.FirstOrDefault(p => p.ID == apiAppointment.PatientId);
                    Appointments.Add(new Patient
                    {
                        Name = patient?.Name ?? "Unknown Patient",
                        Phone = patient?.Phone ?? "No phone",
                        ID = apiAppointment.PatientId
                    });
                }
            }
        }
        catch (Exception ex)
        {
            // Keep the dummy data as fallback
            Console.WriteLine($"Error loading doctor appointments: {ex.Message}");
        }
    }

    private async void LogOut_Click(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }

    private async void InpatientManagement_Click(object sender, EventArgs e)
    {
        // Navigate to inpatient management page
    }

    private async void WardRounds_Click(object sender, EventArgs e)
    {
        // Navigate to ward rounds page
        await Shell.Current.GoToAsync($"{nameof(WardRounds)}");
    }

    private  void PatientMonitoring_Click(object sender, EventArgs e)
    {
        // Navigate to patient monitoring page
    }

    private async void DischargePlanning_Click(object sender, EventArgs e)
    {
        // Navigate to discharge planning page
       await Shell.Current.GoToAsync(nameof(DischargePlanning));
    }

    private void OnCalendarDateSelected(object sender, CalendarSelectionChangedEventArgs e)
    {
        if (e.NewValue != null)
        {
            DateTime selectedDate = (DateTime)e.NewValue;
            // Handle date selection
        }
    }

    private void InpatientListView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        if (e.DataItem is Inpatient selectedPatient)
        {
            // Navigate to patient details page with the selected patient
        }
    }

    private async void SfListView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        //DisplayAlert("Tapped", "You selected: " + ((Appointment)e.DataItem).PatientName, "OK");
        var patient = listView.SelectedItem;
        if (patient != null)
        {
            var patientId = ((Patient)patient).ID;
            // Store the selected patient ID in preferences
            await Shell.Current.GoToAsync(nameof(InpatientOrdersPage));

        }
    }
}

// Model classes
public class Inpatient
{
    public string PatientId { get; set; }
    public string PatientName { get; set; }
    public string RoomNumber { get; set; }
    public string AdmissionDate { get; set; }
    public string Condition { get; set; }
}

public class CriticalAlert
{
    public string Timestamp { get; set; }
    public string AlertType { get; set; }
    public string Description { get; set; }
}

public class WardStatistic
{
    public string Category { get; set; }
    public double Value { get; set; }
}