using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using E_Vita.Services;
using E_Vita_APIs.Models;

namespace E_Vita.Views
{
    public partial class OperationRoomReservation : ContentPage
    {
        private class RoomReservation
        {
            public string Doctor { get; set; }
            public string Patient { get; set; }
            public DateTime Date { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public DateTime EndDateTime { get; set; }
            public string Specialty { get; set; }
            public string Operation { get; set; }
        }

        private Dictionary<string, RoomReservation> reservations = new Dictionary<string, RoomReservation>();
        private int roomCount = 0;
        private IDispatcherTimer timer;
        private readonly OperationRoomServices _operationRoomServices;
        private readonly Practitioner _loggedInDoctor;

        public OperationRoomReservation(Practitioner loggedInDoctor)
        {
            InitializeComponent();
            _operationRoomServices = new OperationRoomServices();
            _loggedInDoctor = loggedInDoctor;
            LoadOperationRooms();
            StartReservationTimer();
        }

        private async void LoadOperationRooms()
        {
            try
            {
                var rooms = await _operationRoomServices.GetAllAsync();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    RoomStack.Children.Clear();
                    foreach (var room in rooms)
                    {
                        var roomButton = new Button
                        {
                            Text = $"Room {room.Id}",
                            WidthRequest = 120,
                            HeightRequest = 120,
                            CornerRadius = 20,
                            BackgroundColor = room.RoomStatus == RoomStatus.Available ? 
                                Color.FromArgb("#3b0054") : Colors.Red,
                            TextColor = Colors.AntiqueWhite,
                            FontFamily = "Qatar2022 Arabic",
                            FontSize = 16,
                            Margin = 10
                        };
                        roomButton.Clicked += OnRoomClicked;
                        RoomStack.Children.Add(roomButton);
                    }
                    roomCount = rooms.Count;
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load operation rooms: " + ex.Message, "OK");
            }
        }

        private void StartReservationTimer()
        {
            timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Check every second
            timer.Tick += (s, e) =>
            {
                UpdateCurrentTime();
                CheckReservations();
            };
            timer.Start();
            Console.WriteLine($"Timer started at {DateTime.Now:HH:mm:ss}");
        }

        private void UpdateCurrentTime()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CurrentTimeLabel.Text = $"Current Time: {DateTime.Now:HH:mm:ss}";
            });
        }

        private void CheckReservations()
        {
            var currentDateTime = DateTime.Now;
            Console.WriteLine($"\nChecking reservations at {currentDateTime:HH:mm:ss}");

            foreach (var reservation in reservations.ToList())
            {
                var roomButton = RoomStack.Children.FirstOrDefault(c => c is Button b && b.Text == reservation.Key) as Button;
                if (roomButton != null)
                {
                    try
                    {
                        var endDateTime = reservation.Value.EndDateTime;

                        Console.WriteLine($"\nRoom {reservation.Key}:");
                        Console.WriteLine($"  Current time: {currentDateTime:HH:mm:ss}");
                        Console.WriteLine($"  End time: {endDateTime:HH:mm:ss}");
                        Console.WriteLine($"  Current >= End: {currentDateTime >= endDateTime}");

                        // Compare only the time part
                        var currentTime = currentDateTime.TimeOfDay;
                        var endTime = endDateTime.TimeOfDay;
                        var isToday = endDateTime.Date == currentDateTime.Date;

                        if (isToday && currentTime >= endTime)
                        {
                            Console.WriteLine($"Unreserving room {reservation.Key} - Time condition met");
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                roomButton.BackgroundColor = Color.FromArgb("#3b0054");
                                reservations.Remove(reservation.Key);
                                DisplayAlert("Room Available", $"Room {reservation.Key} is now available", "OK");
                                Console.WriteLine($"Room {reservation.Key} unreserved successfully");
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing reservation for room {reservation.Key}: {ex.Message}");
                    }
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            timer?.Stop();
        }

        private async void OnAddRoomClicked(object sender, EventArgs e)
        {
            var addRoomForm = new AddOperationRoomForm();
            await Navigation.PushModalAsync(addRoomForm);
            
            // Refresh the room list after the form is closed
            LoadOperationRooms();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async void OnRoomClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                await button.ScaleTo(1.2, 100);
                await button.ScaleTo(1.0, 100);

                // Get room ID from button text (e.g., "Room 1" -> "1")
                var roomId = button.Text.Split(' ')[1];

                // Check if room is already reserved
                if (button.BackgroundColor == Colors.Red)
                {
                    if (reservations.TryGetValue(button.Text, out var reservation))
                    {
                        var timeRemaining = reservation.EndDateTime - DateTime.Now;
                        await DisplayAlert("Reservation Details",
                            $"Room: {button.Text}\n" +
                            $"Doctor: {reservation.Doctor}\n" +
                            $"Patient: {reservation.Patient}\n" +
                            $"Specialty: {reservation.Specialty}\n" +
                            $"Operation: {reservation.Operation}\n" +
                            $"Date: {reservation.Date:dd/MM/yyyy}\n" +
                            $"Start Time: {reservation.StartTime}\n" +
                            $"End Time: {reservation.EndTime}\n" +
                            $"Time Remaining: {timeRemaining.TotalMinutes:F1} minutes",
                            "OK");
                    }
                    return;
                }

                // Show reservation form for available rooms
                var reservationForm = new RoomReservationForm(roomId, _loggedInDoctor);
                await Navigation.PushModalAsync(reservationForm);

                reservationForm.ReservationCompleted += async (s, reservationData) =>
                {
                    if (reservationData != null)
                    {
                        // Parse the end time (format: HH:mm)
                        var timeParts = reservationData.EndTime.Split(':');
                        if (timeParts.Length == 2 &&
                            int.TryParse(timeParts[0], out int hours) &&
                            int.TryParse(timeParts[1], out int minutes))
                        {
                            // Set the end time for today
                            var now = DateTime.Now;
                            var endDateTime = now.Date.Add(new TimeSpan(hours, minutes, 0));

                            var newReservation = new RoomReservation
                            {
                                Doctor = reservationData.Doctor,
                                Patient = reservationData.Patient,
                                Date = endDateTime.Date,
                                StartTime = now.ToString("HH:mm"),
                                EndTime = $"{hours:D2}:{minutes:D2}",
                                EndDateTime = endDateTime,
                                Specialty = reservationData.Specialty,
                                Operation = reservationData.Operation
                            };

                            reservations[button.Text] = newReservation;
                            button.BackgroundColor = Colors.Red;

                            Console.WriteLine($"\nNew reservation created for {button.Text}:");
                            Console.WriteLine($"  Current time: {now:HH:mm:ss}");
                            Console.WriteLine($"  End time: {endDateTime:HH:mm:ss}");

                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                await DisplayAlert("Reservation Confirmed",
                                    $"Room reserved until {hours:D2}:{minutes:D2} today",
                                    "OK");
                            });

                            // Restart the timer to ensure it keeps running
                            if (timer != null)
                            {
                                timer.Stop();
                                timer.Start();
                            }
                            else
                            {
                                StartReservationTimer();
                            }
                        }
                    }
                };
            }
        }
    }
    public class ReservationForm : ContentPage
    {
        public event EventHandler<ReservationData> ReservationCompleted;

        private Picker doctorPicker;
        private Picker patientPicker;
        private Picker specialtyPicker;
        private Picker operationPicker;
        private DatePicker datePicker;
        private TimePicker startTimePicker;
        private TimePicker endTimePicker;
        private List<SurgicalSpecialty> surgicalSpecialties;

        public ReservationForm()
        {
            Title = "New Reservation";
            BackgroundColor = Color.FromArgb("#F5F5F5");

            // Initialize the list before loading
            surgicalSpecialties = new List<SurgicalSpecialty>();
            LoadSurgicalSpecialties();

            var mainLayout = new ScrollView
            {
                Content = new StackLayout { Spacing = 20, Padding = 20 } // Added padding for better appearance
            };

            // Add Back Button at the top
            var backButton = new Button
            {
                Text = "Back",
                FontFamily = "Qatar2022 Arabic",
                BackgroundColor = Color.FromArgb("#3b0054"),
                TextColor = Colors.AntiqueWhite,
                CornerRadius = 20,
                HeightRequest = 50,
                WidthRequest = 80,
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(0, 0, 0, 20)
            };
            backButton.Clicked += OnBackClicked;
            ((StackLayout)mainLayout.Content).Children.Add(backButton);

            // Doctor Selection
            var doctorFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15 // Increased padding for better appearance
            };
            var doctorStack = new StackLayout { Spacing = 5 };
            doctorStack.Children.Add(new Label
            {
                Text = "Select Doctor",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black // Explicitly set text color
            });
            doctorPicker = new Picker
            {
                ItemsSource = new List<string> { "Dr. Smith", "Dr. Johnson", "Dr. Williams", "Dr. Brown" },
                FontSize = 16,
                TextColor = Colors.Black // Explicitly set text color
            };
            doctorStack.Children.Add(doctorPicker);
            doctorFrame.Content = doctorStack;

            // Patient Selection
            var patientFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15 // Increased padding for better appearance
            };
            var patientStack = new StackLayout { Spacing = 5 };
            patientStack.Children.Add(new Label
            {
                Text = "Select Patient",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black // Explicitly set text color
            });
            patientPicker = new Picker
            {
                ItemsSource = new List<string> { "John Doe", "Jane Smith", "Robert Brown", "Alice Johnson" },
                FontSize = 16,
                TextColor = Colors.Black // Explicitly set text color
            };
            patientStack.Children.Add(patientPicker);
            patientFrame.Content = patientStack;

            // Specialty Selection
            var specialtyFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15 // Increased padding for better appearance
            };
            var specialtyStack = new StackLayout { Spacing = 5 };
            specialtyStack.Children.Add(new Label
            {
                Text = "Select Specialty",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black // Explicitly set text color
            });
            specialtyPicker = new Picker
            {
                ItemsSource = surgicalSpecialties?.Select(s => s.Specialty).ToList() ?? new List<string>(),
                FontSize = 16,
                TextColor = Colors.Black // Explicitly set text color
            };
            specialtyPicker.SelectedIndexChanged += OnSpecialtySelected;
            specialtyStack.Children.Add(specialtyPicker);
            specialtyFrame.Content = specialtyStack;

            // Operation Selection
            var operationFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15 // Increased padding for better appearance
            };
            var operationStack = new StackLayout { Spacing = 5 };
            operationStack.Children.Add(new Label
            {
                Text = "Select Operation",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black // Explicitly set text color
            });
            operationPicker = new Picker
            {
                FontSize = 16,
                IsEnabled = false,
                TextColor = Colors.Black // Explicitly set text color
            };
            operationStack.Children.Add(operationPicker);
            operationFrame.Content = operationStack;

            // Date Selection
            var dateFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15 // Increased padding for better appearance
            };
            var dateStack = new StackLayout { Spacing = 5 };
            dateStack.Children.Add(new Label
            {
                Text = "Select Date",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black // Explicitly set text color
            });
            datePicker = new DatePicker
            {
                Format = "dd/MM/yyyy",
                FontSize = 16,
                MinimumDate = DateTime.Today,
                TextColor = Colors.Black // Explicitly set text color
            };
            dateStack.Children.Add(datePicker);
            dateFrame.Content = dateStack;

            // Time Selection
            var timeFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15 // Increased padding for better appearance
            };
            var timeStack = new StackLayout { Spacing = 10 };
            timeStack.Children.Add(new Label
            {
                Text = "Select Time",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black // Explicitly set text color
            });

            var timeLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 10 };
            startTimePicker = new TimePicker
            {
                Format = "HH:mm",
                FontSize = 16,
                TextColor = Colors.Black // Explicitly set text color
            };
            var toLabel = new Label
            {
                Text = "to",
                VerticalOptions = LayoutOptions.Center,
                FontSize = 16,
                TextColor = Colors.Black // Explicitly set text color
            };
            endTimePicker = new TimePicker
            {
                Format = "HH:mm",
                FontSize = 16,
                TextColor = Colors.Black // Explicitly set text color
            };

            timeLayout.Children.Add(startTimePicker);
            timeLayout.Children.Add(toLabel);
            timeLayout.Children.Add(endTimePicker);
            timeStack.Children.Add(timeLayout);
            timeFrame.Content = timeStack;

            // Save Button
            var saveButton = new Button
            {
                Text = "Save Reservation",
                BackgroundColor = Color.FromArgb("#4CAF50"),
                TextColor = Colors.White,
                CornerRadius = 25,
                HeightRequest = 50,
                FontSize = 16,
                Margin = new Thickness(0, 20, 0, 0)
            };
            saveButton.Clicked += OnSaveClicked;

            // Add all frames to the main layout
            ((StackLayout)mainLayout.Content).Children.Add(doctorFrame);
            ((StackLayout)mainLayout.Content).Children.Add(patientFrame);
            ((StackLayout)mainLayout.Content).Children.Add(specialtyFrame);
            ((StackLayout)mainLayout.Content).Children.Add(operationFrame);
            ((StackLayout)mainLayout.Content).Children.Add(dateFrame);
            ((StackLayout)mainLayout.Content).Children.Add(timeFrame);
            ((StackLayout)mainLayout.Content).Children.Add(saveButton);

            Content = mainLayout;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private void LoadSurgicalSpecialties()
        {
            try
            {
                // Get the path to the JSON file
                var assembly = GetType().Assembly;
                var resourceName = "E_Vita.Resources.Raw.surgical_operations.json";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var jsonContent = reader.ReadToEnd();
                            var surgicalData = JsonConvert.DeserializeObject<SurgicalData>(jsonContent);
                            surgicalSpecialties = surgicalData.SurgicalOperations;
                            Console.WriteLine($"Loaded {surgicalSpecialties.Count} specialties");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Could not find the surgical operations JSON file");
                        surgicalSpecialties = new List<SurgicalSpecialty>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading surgical specialties: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                surgicalSpecialties = new List<SurgicalSpecialty>();
            }
        }

        private void OnSpecialtySelected(object sender, EventArgs e)
        {
            if (specialtyPicker.SelectedIndex >= 0 && surgicalSpecialties != null)
            {
                var selectedSpecialty = surgicalSpecialties[specialtyPicker.SelectedIndex];
                Console.WriteLine($"Selected specialty: {selectedSpecialty.Specialty}");
                Console.WriteLine($"Number of operations: {selectedSpecialty.Operations.Count}");

                operationPicker.ItemsSource = selectedSpecialty.Operations;
                operationPicker.IsEnabled = true;
                operationPicker.SelectedIndex = -1;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (doctorPicker.SelectedItem == null || patientPicker.SelectedItem == null ||
                specialtyPicker.SelectedItem == null || operationPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please fill in all required fields", "OK");
                return;
            }

            if (startTimePicker.Time >= endTimePicker.Time)
            {
                await DisplayAlert("Error", "End time must be after start time", "OK");
                return;
            }

            var reservationData = new ReservationData
            {
                Doctor = doctorPicker.SelectedItem.ToString(),
                Patient = patientPicker.SelectedItem.ToString(),
                Specialty = specialtyPicker.SelectedItem.ToString(),
                Operation = operationPicker.SelectedItem.ToString(),
                Date = datePicker.Date,
                StartTime = $"{startTimePicker.Time.Hours:D2}:{startTimePicker.Time.Minutes:D2}",
                EndTime = $"{endTimePicker.Time.Hours:D2}:{endTimePicker.Time.Minutes:D2}"
            };

            ReservationCompleted?.Invoke(this, reservationData);
            await Navigation.PopModalAsync();
        }
    }

    public class ReservationData
    {
        public string Doctor { get; set; }
        public string Patient { get; set; }
        public string Specialty { get; set; }
        public string Operation { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class SurgicalData
    {
        public List<SurgicalSpecialty> SurgicalOperations { get; set; }
    }

    public class SurgicalSpecialty
    {
        public string Specialty { get; set; }
        public List<string> Operations { get; set; }
    }
}
