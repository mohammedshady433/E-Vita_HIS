using E_Vita.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace E_Vita.Views;

public partial class CancelAppointment : ContentPage
{
    public ObservableCollection<AppointmentModel> ScheduleList { get; set; }= new ObservableCollection<AppointmentModel>();

    // The master list of all appointments
    private List<AppointmentModel> _allAppointments = new List<AppointmentModel>();

    public CancelAppointment()
    {
        InitializeComponent();
        _allAppointments = new List<AppointmentModel> { };
        // Set the binding context
        BindingContext = this;
        // Dummy data for testing  
        LoadAppointments();
        //    new AppointmentModel
        //    {
        //        Time = "02:00 PM",
        //        Patient_appointment = new Patientsooooo { Patient_ID = "456", Patient_Name = "Alice Johnson", Time = "02:00 PM" },
        //        Doctor_appointment = new Doctor { Doctor_Name = "Dr. Smith", ID = "001", Specialty = "Neurology" }

        // Initialize ScheduleList with all appointments
        //ScheduleList = new ObservableCollection<AppointmentModel>(_allAppointments);
        //BindingContext = this;
    }

    private async void LoadAppointments()
    {
        AppointmentsService appointmentsService = new AppointmentsService();
        var allappointments = await appointmentsService.GetAllAsync();
        PractitionerServices practitionerServices = new PractitionerServices();
        List<AppointmentModel> finallist = new List<AppointmentModel>();
        PatientServices patientService = new PatientServices();
        ScheduleList.Clear();
        foreach (var appointment in allappointments)
        {
            var patient = await patientService.GetPatientsAsync(appointment.PatientId);
            if (appointment.Start <= DateTime.Now.AddHours(2) || appointment.Status == E_Vita_APIs.Models.AppointmentStatus.Completed || appointment.Status == E_Vita_APIs.Models.AppointmentStatus.Cancelled)
            {
                continue;
            }
            else
            {
                AppointmentModel appointmentModel = new AppointmentModel
                {
                    Time = appointment.Start.ToString("hh:mm tt"),
                    id = appointment.Id,
                    Patient_appointment = new Patientsooooo
                    {
                        Patient_ID = patient.ID,
                        Patient_Name = patient.Name,
                        Time = appointment.Start.ToString("hh:mm tt")
                    },
                };
                // Add the appointment to the final list
                _allAppointments.Add(appointmentModel);
                ScheduleList.Add(appointmentModel);
            }
        }
    }

    // Update the filtering logic in OnSearchTextChanged to convert the integer Patient_ID to a string before calling Contains.
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
            appt.Patient_appointment.Patient_ID.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();

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
            AppointmentsService appointmentsService = new AppointmentsService();
            var updatedAppointment = await appointmentsService.GetByIdAsync(appointment.id);
            updatedAppointment.Cancelation_Date = DateTime.Now;
            updatedAppointment.Status = E_Vita_APIs.Models.AppointmentStatus.Cancelled;
            appointmentsService.UpdateAsync(updatedAppointment.Id, updatedAppointment);
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
            AppointmentsService appointmentsService = new AppointmentsService();
            var updatedAppointment = await appointmentsService.GetByIdAsync(appointment.id);
            updatedAppointment.Status = E_Vita_APIs.Models.AppointmentStatus.NoShow;
            appointmentsService.UpdateAsync(updatedAppointment.Id, updatedAppointment);
            await DisplayAlert("Updated", "Appointment has been marked as No Show.", "OK");
        }
    }
}

// Models  
public class AppointmentModel
{
    public string Time { get; set; } = string.Empty;
    public int id { get; set; }
    public Patientsooooo Patient_appointment { get; set; } = new Patientsooooo();
    public bool IsNoShow { get; set; } = false;
}

 public class Patientsooooo
{
    public int Patient_ID { get; set; }
    public string Patient_Name { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
}
