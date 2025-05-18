using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace E_Vita.Views;

public partial class ReceptionistDashboard : ContentPage
{
	public ReceptionistDashboard()
	{
		InitializeComponent();
        LoadNotes();
        StartClock();
        LoadDoctors(); // Load dummy doctor data
    }
    private void LoadDoctors()
    {
        var doctors = new List<Doctor>
        {
            new Doctor { Name = "Dr. Sahar Fawzi", Specialty = "Cardiologist", PhoneNumber = "01001236585" },
            new Doctor { Name = "Dr. Shady Mohammed", Specialty = "Dermatologist", PhoneNumber = "0156625842" },
            new Doctor { Name = "Dr. Sandy Melad", Specialty = "Pediatrician", PhoneNumber = "01108409974" },
            new Doctor { Name = "Dr. Dalia Saudi", Specialty = "Neurologist", PhoneNumber = "01180860990" }
        };

        DoctorListBox.ItemsSource = doctors;
    }


public class Doctor
{
    public string Name { get; set; }
    public string Specialty { get; set; }
    public string PhoneNumber { get; set; }
}
    private void StartClock()
    {
        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            ClockLabel.Text = DateTime.Now.ToString("HH:mm:ss");
            return true; // Keep the timer running
        });
    }

    private async void OnPostNoteClicked(object sender, EventArgs e)
    {
        string noteText = NoteEditor.Text?.Trim();

        if (!string.IsNullOrEmpty(noteText))
        {
            // TODO: Post to backend API here
            // await YourApi.PostNoteAsync(noteText);

            AddNoteToUI(noteText, "You", DateTime.Now); // Temporary local addition

            NoteEditor.Text = string.Empty;
        }
        else
        {
            await DisplayAlert("Empty Note", "Please enter some text before posting.", "OK");
        }
    }

    // Simulate loading notes from the backend
    private void LoadNotes()
    {
        // TODO: Replace with your API call
        AddNoteToUI("i have to leave now, cancel john's appointment", "receptionist ali", DateTime.Now.AddMinutes(-30));
    }

    // UI Helper
    private void AddNoteToUI(string text, string author, DateTime timestamp)
    {
        var noteFrame = new Frame
        {
            BackgroundColor = Color.FromArgb("#fff8e1"),
            BorderColor = Colors.LightGray,
            CornerRadius = 10,
            Padding = new Thickness(10),
            Content = new VerticalStackLayout
            {
                Spacing = 4,
                Children =
                {
                    new Label { Text = text, FontSize = 16, TextColor = Colors.Black },
                    new Label { Text = $"— {author}, {timestamp:t}", FontSize = 12, TextColor = Colors.Gray }
                }
            }
        };

        NotesStack.Children.Insert(0, noteFrame); // insert at top
    }
    private async void LogOut_Click(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }

    private async void newAppointment(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BookAppointment));
    }


    private async void AddnewPatient(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddPatient));
    }

    private async void addNewDoc(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddDoctor));
    }

    private async void financebtn(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Finance));
    }

    private async void cancelappointment(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CancelAppointment));
    }

    private async void newRoom(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Room_Reservation));
    }

    private async void addNewNurse(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddNurse));
    }
}