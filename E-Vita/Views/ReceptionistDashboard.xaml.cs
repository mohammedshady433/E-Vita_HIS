using E_Vita.Services;
using E_Vita_APIs.Models;
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
    private async void LoadDoctors()
    {
        string currentDay = DateTime.Now.DayOfWeek.ToString();       // e.g., "Monday"
        TimeSpan currentTime = DateTime.Now.TimeOfDay;
        List<Practitioner_Role> availabilityList = new List<Practitioner_Role>();
        PractitionerRoleService practitionerRoleService = new PractitionerRoleService();
        var allpractRolesList = await practitionerRoleService.GetPractitionerRolesAsync();
        allpractRolesList = allpractRolesList
        .Where(a => a.DayOfWeek == currentDay && currentTime >= a.StartTime && currentTime <= a.EndTime)
        .ToList();
        foreach (var item in allpractRolesList)
        {
            if (item.Service == Service.DoctorIN || item.Service == Service.DoctorOUT)
            {
                availabilityList.Add(item);
            }
        }
        List<Doctor> doctors = new List<Doctor>();
        PractitionerServices practitionerServices = new PractitionerServices();
        //get the name of the doctor
        foreach (var item in availabilityList)
        {
            var doctor = await practitionerServices.GetPractitionerByIdAsync(item.PractitionerId);
            if (doctor != null)
            {
                doctors.Add(new Doctor
                {
                    Name = doctor.Name,
                    Specialty = item.Specialty.ToString(),
                    
                });

            }
        }
        
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
            SharednotesServices sharednotesServices = new SharednotesServices();
            SharedNote sharedNote = new SharedNote();
            //need to be changed to the practitionerid
            sharedNote.PractitionerID = 123456783;
            sharedNote.content = noteText;

            sharednotesServices.AddAsync(sharedNote);

        }
        else
        {
            await DisplayAlert("Empty Note", "Please enter some text before posting.", "OK");
        }
    }

    // Simulate loading notes from the backend
    private async void LoadNotes()
    {
        //// TODO: Replace with your API call
        //AddNoteToUI("i have to leave now, cancel john's appointment", "receptionist ali", DateTime.Now.AddMinutes(-30));
        SharednotesServices sharednotesServices = new SharednotesServices();
        var notes = await sharednotesServices.GetAllAsync();
        PractitionerServices practitionerServices = new PractitionerServices();
        var practitioners = await practitionerServices.GetPractitionersAsync();
        var receptionlist = practitioners.Where(a => a.Id.ToString().EndsWith("3")).ToList();
        int i = 0;
        foreach (var pract in receptionlist)
        {
            var filteredNotes = notes.Where(p => p.PractitionerID == pract.Id).ToList();
            foreach (var note in filteredNotes)
            {
                AddNoteToUI(note.content, pract.Name, DateTime.Now.AddMinutes(-3+i));
                i++;
            }
        }
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