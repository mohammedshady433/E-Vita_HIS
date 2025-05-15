using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Calendar;
using System.Threading.Tasks;

namespace E_Vita.Views;

public partial class InpatientDoctorDashboard : ContentPage
{
    public ObservableCollection<Inpatient> Inpatients { get; set; }
    public ObservableCollection<CriticalAlert> CriticalAlerts { get; set; }
    public ObservableCollection<WardStatistic> WardStatistics { get; set; }

    public InpatientDoctorDashboard()
    {
        InitializeComponent();

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