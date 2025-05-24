using Microsoft.Maui.Graphics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using E_Vita.Services;
using E_Vita_APIs.Models;

namespace E_Vita.Views;

public partial class BookAppointment : ContentPage
{
    private class Doctor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DateTime> AvailableDates { get; set; }
    }


    private List<Practitioner> doctors;
    private List<Practitioner_Role> Roles; 
    private List<Patient> patients;
    private Patient selectedPatient;

    public BookAppointment()
    {
        InitializeComponent();
        LoadDoctors();
        LoadPatients();
    }

    private async void LoadDoctors()
    {
        PractitionerServices practitionerServices = new PractitionerServices();
        PractitionerRoleService practitionerRoleService = new PractitionerRoleService();

        var allpractRolesList = await practitionerRoleService.GetPractitionerRolesAsync();
        var practitioners = await practitionerServices.GetPractitionersAsync();
        doctors = practitioners;
        var timeselected = datePicker.Date;
        var selectedDate = timeselected.Date;
        var DayName = selectedDate.DayOfWeek.ToString();
        allpractRolesList = allpractRolesList.FindAll(a => a.DayOfWeek == DayName && (a.Service == Service.DoctorIN || a.Service == Service.DoctorOUT));
        Roles = allpractRolesList;
        List<string> filteredPractitioners = new List<string>();
        foreach (var iteration in allpractRolesList)
        {
            if(iteration == null) continue;
            var practitioner = practitioners.FirstOrDefault(p => p.Id == iteration.PractitionerId );
            if (practitioner != null)
            {
                filteredPractitioners.Add(practitioner.Name);
            }
        }
        if (!filteredPractitioners.Any())
        {
            DisplayAlert("No Doctors Available", "There are no doctors available on the selected date. Please choose another date.", "OK");
            return;
        }
        doctorPicker.ItemsSource = filteredPractitioners;

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
            patientList.IsVisible = false;
            return;
        }

        var searchText = e.NewTextValue.ToLower();
        var filteredPatients = patients
            .Where(p => (p.ID.ToString()).Contains(searchText) || p.Name.ToLower().Contains(searchText))
            .ToList();

        patientList.ItemsSource = filteredPatients;
        patientList.IsVisible = filteredPatients.Any();
    }

    // Pseudocode plan:
    // 1. The method is using a local Patient class, but the ItemsSource is likely set to a list of E_Vita_APIs.Models.Patient (from LoadPatients).
    // 2. The type check in the selection handler is for the local Patient, so the cast fails and the block is not executed.
    // 3. Fix: Use the correct Patient type in the selection handler and update selectedPatient accordingly.

    private void OnPatientSelected(object sender, SelectionChangedEventArgs e)
    {
        // Use the correct Patient type from your data source
        if (e.CurrentSelection.FirstOrDefault() is E_Vita_APIs.Models.Patient selected)
        {
            selectedPatient = selected;
            patientSearchBar.Text = selected.Name;
            patientList.IsVisible = false;
        }
    }

    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        LoadDoctors();
    }

    private async void ConfirmAppointment_Clicked(object sender, EventArgs e)
    {
        if (selectedPatient == null)
        {
            await DisplayAlert("Error", "Please select a patient", "OK");
            return;
        }

        if (doctorPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select a doctor", "OK");
            return;
        }

        string appointmentTime = timeEntry.Text;
        if (string.IsNullOrWhiteSpace(appointmentTime))
        {
            await DisplayAlert("Error", "Please select an appointment time", "OK");
            return;
        }

        string selectedDoctor = doctorPicker.SelectedItem.ToString();
        string message = $"Patient: {selectedPatient.Name}\n" +
                        $"Patient ID: {selectedPatient.ID}\n" +
                        $"Doctor: {selectedDoctor}\n" +
                        $"Time: {appointmentTime}\n" +
                        $"Date: {datePicker.Date:yyyy-MM-dd}";
        AppointmentsService appointmentsService = new AppointmentsService();
        var selectedDoctorvar = doctors.FirstOrDefault(d => d.Name == selectedDoctor);
        var selectedDoctRole = Roles.FirstOrDefault(r => r.PractitionerId == selectedDoctorvar.Id);
        string dateString = datePicker.Date.ToString("yyyy-MM-dd");
        string dateTimeString = $"{dateString} {appointmentTime}";
        DateTime startDateTime = DateTime.ParseExact(
    dateTimeString,
    "yyyy-MM-dd hh:mm tt",
    System.Globalization.CultureInfo.InvariantCulture
);
        int uniqueId = (int)(DateTime.Now.Ticks & 0x7FFFFFFF);
        AppointmentPractitionerService appointmentPractitionerService = new AppointmentPractitionerService();
        Appointment newAppointment = new Appointment
        {
            PatientId = selectedPatient.ID,
            Actor = selectedDoctRole.Service,
            Start = startDateTime,
            End = startDateTime.AddMinutes(15),
            Service_Type = ServiceType.clincal,
            Status = AppointmentStatus.Scheduled,
            Id = uniqueId

        };
        await appointmentsService.CreateAsync(newAppointment);
        AppointmentPractitioner appointmentPractitioner = new AppointmentPractitioner
        {
            AppointmentId = uniqueId,
            PractitionersId = selectedDoctorvar.Id
        };
        await appointmentPractitionerService.AddAsync(appointmentPractitioner);
        await DisplayAlert("Appointment has been confirmed", message, "OK");
    }

    private async void Close(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
    }

    private void OnTimeEntryTapped(object sender, FocusEventArgs e)
    {
        timeList.IsVisible = true;
    }

    private void OnTimeSlotSelected(object sender, SelectionChangedEventArgs e)
    {
        string selectedTime = e.CurrentSelection.FirstOrDefault() as string;

        if (!string.IsNullOrEmpty(selectedTime))
        {
            timeEntry.Text = selectedTime;
            timeList.IsVisible = false;
        }
    }

    private async void OnDoctorPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        // Your logic here, e.g., get the selected doctor:
        var picker = sender as Picker;
        var selectedDoctorname = picker.SelectedItem;
        var targetdoc = doctors.FirstOrDefault(p => p.Name == (string)selectedDoctorname);
        //take the start time and end time of the doctor then make a avialable slot every 15min 
        if (targetdoc != null)
        {
            var selectedDate = datePicker.Date;
            var DayName = selectedDate.DayOfWeek.ToString();
            var selectedRole = Roles.FirstOrDefault(r => r.PractitionerId == targetdoc.Id && r.DayOfWeek == DayName);
            if (selectedRole != null)
            {
                
                DateTime startTime = DateTime.Today.Add(selectedRole.StartTime);
                DateTime endTime = DateTime.Today.Add(selectedRole.EndTime);
                List<string> timeSlots = new List<string>();
                while (startTime <= endTime)
                {                    
                    if (startTime > DateTime.Now) // Skip past times
                    {
                        startTime = startTime.AddMinutes(15);
                        continue;
                    }
                    AppointmentsService appointmentsService = new AppointmentsService();
                    var appointments = await appointmentsService.GetAllAsync();
                    AppointmentPractitionerService appointmentPractitionerService = new AppointmentPractitionerService();
                    var appointmentandpractiotioner = await appointmentPractitionerService.GetAllAsync();

                    //Check if the time slot is already booked
                    bool isBooked = appointments.Any(a => a.Start.Date == selectedDate && a.Start.TimeOfDay == startTime.TimeOfDay
                    && appointmentandpractiotioner.Any(ap => ap.PractitionersId == targetdoc.Id && ap.AppointmentId == a.Id));


                    if (isBooked)
                    {
                        startTime = startTime.AddMinutes(15);
                        continue;
                    }
                    timeSlots.Add(startTime.ToString("hh:mm tt"));
                    startTime = startTime.AddMinutes(15); // 15-minute intervals
                }
                timeList.ItemsSource = timeSlots;
            }
        }
    }
}