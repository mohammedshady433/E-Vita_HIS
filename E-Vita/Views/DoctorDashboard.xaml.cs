using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Calendar;
using System.Threading.Tasks;
using E_Vita.Services;
using E_Vita_APIs.Models;
using System.Linq;
namespace E_Vita.Views;
public partial class DoctorDashboard : ContentPage
{
    public ObservableCollection<Patient> Appointments { get; set; }
    private readonly AppointmentsService _appointmentsService= new AppointmentsService();
    private readonly PractitionerServices _practitionerService;
    private readonly PatientServices _patientService = new PatientServices();
    private readonly AppointmentPractitionerService _appointmentPractitionerService = new AppointmentPractitionerService();
    private int _currentDoctorId;
    public DoctorDashboard()
    {
		InitializeComponent();
        Appointments = new ObservableCollection<Patient>();

        //_appointmentsService = new AppointmentsService();
        //Appointments = new ObservableCollection<Appointment>
        //    {
        //        new Appointment { PatientName = "Ahmed", Time = "10:00 AM", PatientPhone = "01012345678", PatientID = "P1001" },
        //        new Appointment { PatientName = "Sara", Time = "10:30 AM", PatientPhone = "01087654321", PatientID = "P1002" },
        //        new Appointment { PatientName = "Omar", Time = "11:00 AM", PatientPhone = "01099999999", PatientID = "P1003" }
        //    };
        // Set the BindingContext so the ListView can access the Appointments property
        this.BindingContext = this;

        // Get doctor ID from preferences (set during login in MainPage.xaml.cs)
        if (Preferences.ContainsKey("CurrentDoctorId"))
        {
            _currentDoctorId = Preferences.Get("CurrentDoctorId", 0);
            if (_currentDoctorId > 0)
            {
                LoadAppointmentsForDoctor(_currentDoctorId);
            }
        }
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

    //private void OnCalendarDateSelected(object sender, Syncfusion.Maui.Calendar.CalendarSelectionChangedEventArgs e)
    //{
    //    if (e.NewValue != null)
    //    {
    //        DateTime selectedDate = (DateTime)e.NewValue;
    //        //SelectedDateLabel.Text = $"You selected: {selectedDate.ToString("D")}";

    //        // Filter appointments by the selected date
    //        FilterAppointmentsByDate(selectedDate);
    //    }
    //}

    //private async void FilterAppointmentsByDate(DateTime date)
    //{
    //    try
    //    {
    //        // Only proceed if we have a valid doctor ID
    //        if (_currentDoctorId <= 0)
    //            return;

    //        // Get all appointments
    //        var allAppointments = await _appointmentsService.GetAllAsync();

    //        if (allAppointments != null && allAppointments.Any())
    //        {
    //            // Filter appointments for this doctor on the selected date
    //            var filteredAppointments = allAppointments
    //                .Where(a => a.AppointmentPractitioners != null &&
    //                       a.AppointmentPractitioners.Any(ap => ap.PractitionersId == _currentDoctorId) &&
    //                       a.Start.Date == date.Date)
    //                .ToList();

    //            // Clear and update the UI appointments collection
    //            Appointments.Clear();

    //            if (filteredAppointments.Any())
    //            {
    //                foreach (var apiAppointment in filteredAppointments)
    //                {
    //                    Appointments.Add(new Appointment
    //                    {
    //                        PatientName = apiAppointment.Patient?.Name ?? "Unknown Patient",
    //                        Time = apiAppointment.Start.ToString("h:mm tt"),
    //                        PatientPhone = apiAppointment.Patient?.Phone ?? "No phone",
    //                        PatientID = apiAppointment.PatientId.ToString()
    //                    });
    //                }
    //            }
    //            // If no appointments are found, the list will remain empty
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error filtering appointments: {ex.Message}");
    //    }
    //}
    private async void LogOut_Click(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }

    private void settings(object sender, EventArgs e)
    {

    }

    private async void PatientData(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Patient_data));
    }

    private async void labImagesbtn(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LabImages));
    }

    private async void SfListView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        //DisplayAlert("Tapped", "You selected: " + ((Appointment)e.DataItem).PatientName, "OK");
        var patient = listView.SelectedItem;
        if (patient != null)
        {
            var patientId = ((Patient)patient).ID;
            // Store the selected patient ID in preferences
            Preferences.Set("SelectedPatientId", patientId);
            await Shell.Current.GoToAsync(nameof(Prescription));

        }
    }

    private async void oproombtn(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(OperationRoomReservation));
    }

    private void OnCalendarDateSelected(object sender, CalendarSelectionChangedEventArgs e)
    {

    }
}

