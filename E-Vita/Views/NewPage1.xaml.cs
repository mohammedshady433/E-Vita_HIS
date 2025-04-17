using System.Collections.ObjectModel;

namespace E_Vita.Views;

public partial class NewPage1 : ContentPage
{
    public ObservableCollection<Appointment> ScheduleList { get; set; }
    public NewPage1()
    {
        InitializeComponent();
        LoadNotes(); // Call your backend method here to populate existing notes
                     // Sample data
        ScheduleList = new ObservableCollection<Appointment>
        {
            new Appointment { Time = "10:00 AM", PatientName = "John Doe", PatientPhone = "123-456-7890", PatientID = "001" },
            new Appointment { Time = "11:00 AM", PatientName = "Jane Smith", PatientPhone = "234-567-8901", PatientID = "002" },
            new Appointment { Time = "12:00 PM", PatientName = "Emily Johnson", PatientPhone = "345-678-9012", PatientID = "003" },
        };

        // Setting the BindingContext
        this.BindingContext = this;
    }
    private void OnCalendarDateSelected(object sender, Syncfusion.Maui.Calendar.CalendarSelectionChangedEventArgs e)
    {
        if (e.NewValue != null)
        {
            DateTime selectedDate = (DateTime)e.NewValue;
            SelectedDateLabel.Text = $"You selected: {selectedDate.ToString("D")}";
        }
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
        AddNoteToUI("Patient in Room 2 requires wound dressing every 6 hours.", "Nurse Sarah", DateTime.Now.AddMinutes(-30));
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

    private void turn_over(object sender, EventArgs e)
    {

    }
}
