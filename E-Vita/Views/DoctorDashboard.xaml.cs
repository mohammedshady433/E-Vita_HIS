using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Calendar;
using System.Threading.Tasks;
using E_Vita_APIs.Models;

namespace E_Vita.Views;
public partial class DoctorDashboard : ContentPage
{
    public ObservableCollection<Appointment> Appointments { get; set; }
    private readonly Practitioner _loggedInDoctor;

    public DoctorDashboard(Practitioner doctor)
    {
		InitializeComponent();
        Appointments = new ObservableCollection<Appointment>
            {
                new Appointment { PatientName = "Ahmed", Time = "10:00 AM", PatientPhone = "01012345678", PatientID = "P1001" },
                new Appointment { PatientName = "Sara", Time = "10:30 AM", PatientPhone = "01087654321", PatientID = "P1002" },
                new Appointment { PatientName = "Omar", Time = "11:00 AM", PatientPhone = "01099999999", PatientID = "P1003" }
            };

        _loggedInDoctor = doctor;
        // Set the BindingContext so the ListView can access the Appointments property
        this.BindingContext = this;
    }

    private async void LogOut_Click(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }

    private void settings(object sender, EventArgs e)
    {

    }

    private void OnCalendarDateSelected(object sender, Syncfusion.Maui.Calendar.CalendarSelectionChangedEventArgs e)
    {
        if (e.NewValue != null)
        {
            DateTime selectedDate = (DateTime)e.NewValue;
            //SelectedDateLabel.Text = $"You selected: {selectedDate.ToString("D")}";
        }
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
        await Shell.Current.GoToAsync(nameof(Prescription));
    }

    private async void oproombtn(object sender, EventArgs e)
    {
        //await Shell.Current.GoToAsync(nameof(OperationRoomReservation));
        await Shell.Current.Navigation.PushAsync(new OperationRoomReservation(_loggedInDoctor));

    }
}

