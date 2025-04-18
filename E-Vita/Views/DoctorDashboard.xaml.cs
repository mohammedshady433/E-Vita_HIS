using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Calendar;

namespace E_Vita.Views;
public partial class DoctorDashboard : ContentPage
{
    public ObservableCollection<Appointment> Appointments { get; set; }

    public DoctorDashboard()
    {
		InitializeComponent();
        Appointments = new ObservableCollection<Appointment>
            {
                new Appointment { PatientName = "Ahmed", Time = "10:00 AM", PatientPhone = "01012345678", PatientID = "P1001" },
                new Appointment { PatientName = "Sara", Time = "10:30 AM", PatientPhone = "01087654321", PatientID = "P1002" },
                new Appointment { PatientName = "Omar", Time = "11:00 AM", PatientPhone = "01099999999", PatientID = "P1003" }
            };
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

    private void SfListView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        DisplayAlert("Tapped", "You selected: " + ((Appointment)e.DataItem).PatientName, "OK");
    }

    public class Appointment
    {
        public string Time { get; set; }
        public string PatientName { get; set; }
        public string PatientPhone { get; set; }
        public string PatientID { get; set; }
    }
}

