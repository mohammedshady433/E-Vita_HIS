using E_Vita.Services;
using E_Vita_APIs.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;

namespace E_Vita.Views;

public partial class NurseDashboard : ContentPage
{
    private ObservableCollection<Appointment> ScheduleList { get; set; }
    public ObservableCollection<MedicationSchedule> MedicationsList { get; set; }
    private System.Timers.Timer _medicationCheckTimer;
    private int practid=0;
    public NurseDashboard()
    {
        InitializeComponent();
        LoadNotes(); // Call your backend method here to populate existing notes
        InitializeMedicationTracking();

        // Sample data
        ScheduleList = new ObservableCollection<Appointment>
        {
            new Appointment { Time = "10:00 AM", PatientName = "Mohammed Shady", PatientPhone = "01119636882", PatientID = "201" },
            new Appointment { Time = "11:00 AM", PatientName = "Mohammed Hesham", PatientPhone = "01118566425", PatientID = "202" },
            new Appointment { Time = "12:00 PM", PatientName = "Shahd Mostafa", PatientPhone = "01233424242", PatientID = "203" },
        };

        // Setting the BindingContext
        this.BindingContext = this;
    }

    //medication tracker code----------------------------------------------------------------
    private void InitializeMedicationTracking()
    {
        // Create sample medication schedules (dummy data)
        MedicationsList = new ObservableCollection<MedicationSchedule>
        {
            new MedicationSchedule
            {
                PatientId = "201",
                PatientName = "Mohammed Shady",
                MedicationName = "Amoxicillin",
                DosageInstructions = "500mg",
                ScheduledTime = DateTime.Now.AddMinutes(2), // Set to 2 minutes from now for testing
                Status = MedicationStatus.Pending
            },
            new MedicationSchedule
            {
                PatientId = "202",
                PatientName = "Mohammed Hesham",
                MedicationName = "Lisinopril",
                DosageInstructions = "10mg",
                ScheduledTime = DateTime.Now.AddMinutes(5), // Set to 5 minutes from now for testing
                Status = MedicationStatus.Pending
            },new MedicationSchedule
            {
                PatientId = "203",
                PatientName = "Shahd Mostafa",
                MedicationName = "Atorvastatin",
                DosageInstructions = "20mg",
                ScheduledTime = DateTime.Now.AddHours(1),
                Status = MedicationStatus.Pending
            }
        };

        // Set up the UI for medications
        SetupMedicationUI();

        // Set up a timer to check medication status every minute
        _medicationCheckTimer = new System.Timers.Timer(60000); // Check every minute
        _medicationCheckTimer.Elapsed += CheckMedicationSchedules;
        _medicationCheckTimer.Start();
    }
    private void SetupMedicationUI()
    {
        // Clear existing medication UI
        MedicationStack.Children.Clear();

        // Add header
        var headerLabel = new Label
        {
            Text = "Medication Schedule",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 0, 0, 10)
        };
        MedicationStack.Children.Add(headerLabel);

        // Add medication cards
        foreach (var medication in MedicationsList)
        {
            var card = CreateMedicationCard(medication);
            MedicationStack.Children.Add(card);
        }
    }

    private View CreateMedicationCard(MedicationSchedule medication)
    {
        // Create a frame to hold medication info
        var frame = new Frame
        {
            BackgroundColor = GetColorForMedicationStatus(medication.Status),
            BorderColor = Colors.Gray,
            CornerRadius = 10,
            Margin = new Thickness(0, 0, 0, 10),
            Padding = new Thickness(15),
        };

        var layout = new VerticalStackLayout
        {
            Spacing = 5
        };

        // Patient info
        layout.Children.Add(new Label
        {
            Text = medication.PatientName,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.Black
        });

        // Medication info
        layout.Children.Add(new Label
        {
            Text = $"Medication: {medication.MedicationName} ({medication.DosageInstructions})",
            FontSize = 14,
            TextColor = Colors.Black
        });

        // Time info
        layout.Children.Add(new Label
        {
            Text = $"Scheduled Time: {medication.ScheduledTime:t}",
            FontSize = 14,
            TextColor = Colors.Black
        });
        // Status info
        var statusLabel = new Label
        {
            Text = $"Status: {medication.Status}",
            FontSize = 14,
            TextColor = Colors.Black,
            FontAttributes = FontAttributes.Bold
        };
        layout.Children.Add(statusLabel);

        // Add buttons for marking medication as taken or skipped
        var buttonLayout = new HorizontalStackLayout
        {
            Spacing = 10,
            Margin = new Thickness(0, 10, 0, 0)
        };
    
    var takenButton = new Button
    {
        Text = "Mark as Taken",
        BackgroundColor = Colors.Green,
        TextColor = Colors.White,
        CornerRadius = 5,
        WidthRequest = 120,
        HeightRequest = 40,
        CommandParameter = medication
    };
    takenButton.Clicked += (sender, e) => MarkMedicationAsTaken(medication);

    var skipButton = new Button
    {
        Text = "Skip",
        BackgroundColor = Colors.DarkGray,
        TextColor = Colors.White,
        CornerRadius = 5,
        WidthRequest = 80,
        HeightRequest = 40,
        CommandParameter = medication
    };
    skipButton.Clicked += (sender, e) => MarkMedicationAsSkipped(medication);

    buttonLayout.Children.Add(takenButton);
        buttonLayout.Children.Add(skipButton);
        layout.Children.Add(buttonLayout);

        frame.Content = layout;
        return frame;
    }
          private Color GetColorForMedicationStatus(MedicationStatus status)
    {
        return status switch
        {
            MedicationStatus.Pending => Colors.LightYellow,
            MedicationStatus.Taken => Colors.LightGreen,
            MedicationStatus.Missed => Colors.LightPink,
            MedicationStatus.Late => Colors.LightSalmon,
            MedicationStatus.Skipped => Colors.LightGray,
            _ => Colors.White
        };
    }


   

    private void CheckMedicationSchedules(object sender, ElapsedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            bool updated = false;

            foreach (var medication in MedicationsList)
            {
                // If medication is still pending
                if (medication.Status == MedicationStatus.Pending)
                {
                    // Check if it's time to take the medication
                    if (DateTime.Now >= medication.ScheduledTime)
                    {
                        medication.Status = MedicationStatus.Late;
                        updated = true;
                        // Show notification to nurse
                        DisplayMedicationAlert(medication);
                    }
                }

                // Check for medications that are 10 minutes late
                if (medication.Status == MedicationStatus.Late &&
                    (DateTime.Now - medication.ScheduledTime).TotalMinutes >= 10 &&
                    !medication.ReminderSent)
                {
                    medication.ReminderSent = true;
                    updated = true;

                    // Show a more urgent reminder
                    DisplayMedicationReminder(medication);
                }
            }

            // Update UI if changes were made
            if (updated)
            {
                SetupMedicationUI();
            }
        });
    }

    private async void DisplayMedicationAlert(MedicationSchedule medication)
    {
        // Add a note to the shared notes section
        AddNoteToUI(
            $"Medication due for {medication.PatientName}: {medication.MedicationName} {medication.DosageInstructions}",
            "System",
            DateTime.Now
        );

        // Optional: Display an alert
        await DisplayAlert(
            "Medication Due",
            $"It's time for {medication.PatientName} to take {medication.MedicationName} {medication.DosageInstructions}.",
            "OK"
        );
    }
    private async void DisplayMedicationReminder(MedicationSchedule medication)
    {
        // Add a reminder note with higher urgency
        AddNoteToUI(
            $"REMINDER: {medication.PatientName}'s medication {medication.MedicationName} is now 10 minutes overdue!",
            "System (URGENT)",
            DateTime.Now
        );

        // Display a more urgent alert
        await DisplayAlert(
            "URGENT: Medication Overdue",
            $"{medication.PatientName}'s medication {medication.MedicationName} is now 10 minutes overdue! Please attend to this immediately.",
            "I'll Check Now"
        );
    }

    private void MarkMedicationAsTaken(MedicationSchedule medication)
    {
        medication.Status = MedicationStatus.Taken;
        medication.TakenTime = DateTime.Now;

        // Update UI
        SetupMedicationUI();

        // Add note
        AddNoteToUI(
            $"Administered {medication.MedicationName} to {medication.PatientName}",
            "You",
            DateTime.Now
        );
    }

    private void MarkMedicationAsSkipped(MedicationSchedule medication)
    {
        medication.Status = MedicationStatus.Skipped;

        // Update UI
        SetupMedicationUI();

        // Add note
        AddNoteToUI(
            $"Skipped {medication.MedicationName} for {medication.PatientName}",
            "You",
            DateTime.Now
        );
    }
    // Make sure to dispose of the timer when page is unloaded
    protected override void OnDisappearing()
    {
        _medicationCheckTimer?.Stop();
        _medicationCheckTimer?.Dispose();
        _medicationCheckTimer = null;

        base.OnDisappearing();
    }

    //-----------------------------------------notes functions-------------------------------------------

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
            if (practid== 0)
            {
                practid = int.Parse(DisplayPromptAsync("Practitioner ID", "Please enter your Practitioner ID:").Result);
            }
            sharedNote.PractitionerID = practid;
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
        var receptionlist = practitioners.Where(a => a.Id.ToString().EndsWith("2")).ToList();
        int i = 0;
        foreach (var pract in receptionlist)
        {
            var filteredNotes = notes.Where(p => p.PractitionerID == pract.Id).ToList();
            foreach (var note in filteredNotes)
            {
                AddNoteToUI(note.content, pract.Name, DateTime.Now.AddMinutes(-3 + i));
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
    private async void turn_over(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            // Animate a bounce effect
            await button.ScaleTo(1.2, 100); // Scale up
            await button.ScaleTo(1.0, 100); // Scale back to normal

            // Toggle background color using .Equals()
            if (button.BackgroundColor.Equals(Colors.Red))
            {
                button.BackgroundColor = Colors.Green;
            }
            else if (button.BackgroundColor.Equals(Colors.Green))
            {
                button.BackgroundColor = Colors.White;
            }
            else
            {
                button.BackgroundColor = Colors.Red;
            }
        }
    }
    //private void scaleUP()
    //{
    //    var duplicate = new Button
    //    {
    //        ImageSource = condition.ImageSource,
    //        WidthRequest = condition.WidthRequest,
    //        HeightRequest = condition.HeightRequest,
    //        BackgroundColor = condition.BackgroundColor,
    //        CornerRadius = condition.CornerRadius
    //    };

    //    // Attach the same click handler
    //    duplicate.Clicked += turn_over;

    //    // Add the duplicate to the existing layout
    //    ConditionStack.Children.Add(duplicate);
    //}

    //private async void settings(object sender, EventArgs e)
    //{
    //    if (sender is Button button)
    //    {
    //        await button.ScaleTo(1.2, 100);
    //        await button.ScaleTo(1.0, 100);
    //        scaleUP();
    //    }

    //}
    private void LogOut_Click(object sender, EventArgs e)
    {
        // Example: Navigate back to the login page
        Application.Current.MainPage = new MainPage(); // Replace with your actual login page
    }

    private class Appointment
    {
        public string Time { get; internal set; }
        public string PatientName { get; internal set; }
        public string PatientPhone { get; internal set; }
        public string PatientID { get; internal set; }
    }
}
    public class MedicationSchedule
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string MedicationName { get; set; }
        public string DosageInstructions { get; set; }
        public DateTime ScheduledTime { get; set; }
        public DateTime? TakenTime { get; set; }
        public MedicationStatus Status { get; set; }
        public bool ReminderSent { get; set; }
    }
    public enum MedicationStatus
    {
        Pending,
        Taken,
        Missed,
        Late,
        Skipped
    }

