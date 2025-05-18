using Microsoft.Maui.Controls;
using E_Vita_APIs.Models;
using E_Vita.Services;
using System.Collections.ObjectModel;

namespace E_Vita.Views
{
    public partial class RoomReservationForm : ContentPage
    {
        private readonly OperationRoomServices _operationRoomServices;
        private readonly PractitionerServices _practitionerServices;
        private readonly PatientServices _PatientServices;

        private readonly string _roomId;
        private readonly Practitioner _loggedInDoctor;
        private Entry patientSearchEntry;
        private ListView patientListView;
        private ObservableCollection<E_Vita_APIs.Models.Patient> filteredPatients;
        private List<E_Vita_APIs.Models.Patient> allPatients;
        private Picker specialtyPicker;
        private Picker operationPicker;
        private DatePicker datePicker;
        private TimePicker startTimePicker;
        private TimePicker endTimePicker;

        public event EventHandler<ReservationData> ReservationCompleted;

        public RoomReservationForm(string roomId, Practitioner loggedInDoctor)
        {
            InitializeComponent();
            _operationRoomServices = new OperationRoomServices();
            _PatientServices = new PatientServices();
            _practitionerServices = new PractitionerServices();
            _roomId = roomId;
            _loggedInDoctor = loggedInDoctor;
            filteredPatients = new ObservableCollection<E_Vita_APIs.Models.Patient>();
            InitializeForm();
            LoadPatients();
        }

        private async void LoadPatients()
        {
            try
            {
                // Get all patients from the database
                var allPatients = await _PatientServices.GetPatientsAsync();
                    
                    if (allPatients == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Patient list not loaded.", "OK");
                    return;
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load patients: " + ex.Message, "OK");
            }
        }

        private void UpdateFilteredPatients(string searchText)
        {
            filteredPatients.Clear();
            var filtered = allPatients
                .Where(p => string.IsNullOrEmpty(searchText) || 
                           p.ID.ToString().Contains(searchText) ||
                (!string.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(searchText.ToLower())))
                .ToList();
            
            foreach (var patient in filtered)
            {
                filteredPatients.Add(patient);
            }
        }

        private void InitializeForm()
        {
            Title = $"Reserve Room {_roomId}";
            BackgroundColor = Color.FromArgb("#F5F5F5");

            var mainLayout = new ScrollView
            {
                Content = new StackLayout { Spacing = 20, Padding = 20 }
            };

            // Back Button
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

            // Doctor Information (Read-only)
            var doctorFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15
            };
            var doctorStack = new StackLayout { Spacing = 5 };
            doctorStack.Children.Add(new Label
            {
                Text = "Doctor",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black
            });
            doctorStack.Children.Add(new Label
            {
                Text = _loggedInDoctor.Name,
                FontSize = 16,
                TextColor = Colors.Black
            });
            doctorFrame.Content = doctorStack;

            // Patient Search
            var patientFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15
            };
            var patientStack = new StackLayout { Spacing = 5 };
            patientStack.Children.Add(new Label
            {
                Text = "Search Patient (ID or Name)",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black
            });
            patientSearchEntry = new Entry
            {
                Placeholder = "Enter patient ID or name",
                FontSize = 16,
                TextColor = Colors.Black
            };
            patientSearchEntry.TextChanged += OnPatientSearchTextChanged;
            patientStack.Children.Add(patientSearchEntry);

            // Patient List
            patientListView = new ListView
            {
                ItemsSource = filteredPatients,
                ItemTemplate = new DataTemplate(() =>
                {
                    var cell = new ViewCell();
                    var layout = new StackLayout { Padding = new Thickness(10) };
                    var nameLabel = new Label { FontSize = 16 };
                    var idLabel = new Label { FontSize = 14, TextColor = Colors.Gray };
                    
                    nameLabel.SetBinding(Label.TextProperty, "Name");
                    idLabel.SetBinding(Label.TextProperty, "Id");
                    
                    layout.Children.Add(nameLabel);
                    layout.Children.Add(idLabel);
                    cell.View = layout;
                    return cell;
                }),
                HeightRequest = 200
            };
            patientListView.ItemSelected += OnPatientSelected;
            patientStack.Children.Add(patientListView);
            patientFrame.Content = patientStack;

            // Specialty Selection
            var specialtyFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15
            };
            var specialtyStack = new StackLayout { Spacing = 5 };
            specialtyStack.Children.Add(new Label
            {
                Text = "Select Specialty",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black
            });
            specialtyPicker = new Picker
            {
                ItemsSource = Enum.GetNames(typeof(MedicalSpecialty)),
                FontSize = 16,
                TextColor = Colors.Black
            };
            specialtyStack.Children.Add(specialtyPicker);
            specialtyFrame.Content = specialtyStack;

            // Operation Selection
            var operationFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15
            };
            var operationStack = new StackLayout { Spacing = 5 };
            operationStack.Children.Add(new Label
            {
                Text = "Select Operation",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black
            });
            operationPicker = new Picker
            {
                ItemsSource = new List<string>
                {
                    "Appendectomy",
                    "Cholecystectomy",
                    "Hernia Repair",
                    "Mastectomy",
                    "Thyroidectomy"
                },
                FontSize = 16,
                TextColor = Colors.Black
            };
            operationStack.Children.Add(operationPicker);
            operationFrame.Content = operationStack;

            // Date Selection
            var dateFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15
            };
            var dateStack = new StackLayout { Spacing = 5 };
            dateStack.Children.Add(new Label
            {
                Text = "Select Date",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black
            });
            datePicker = new DatePicker
            {
                Format = "dd/MM/yyyy",
                MinimumDate = DateTime.Today,
                FontSize = 16
            };
            dateStack.Children.Add(datePicker);
            dateFrame.Content = dateStack;

            // Time Selection
            var timeFrame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 10,
                Padding = 15
            };
            var timeStack = new StackLayout { Spacing = 10 };
            timeStack.Children.Add(new Label
            {
                Text = "Select Time",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black
            });

            var timeLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 10 };
            startTimePicker = new TimePicker
            {
                Format = "HH:mm",
                FontSize = 16
            };
            var toLabel = new Label
            {
                Text = "to",
                VerticalOptions = LayoutOptions.Center,
                FontSize = 16
            };
            endTimePicker = new TimePicker
            {
                Format = "HH:mm",
                FontSize = 16
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

        private void OnPatientSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilteredPatients(e.NewTextValue);
        }

        private void OnPatientSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is E_Vita_APIs.Models.Patient selectedPatient)
            {
                patientSearchEntry.Text = $"{selectedPatient.Name} (ID: {selectedPatient.ID})";
                patientListView.SelectedItem = null;
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(patientSearchEntry.Text) ||
                specialtyPicker.SelectedItem == null || 
                operationPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please fill in all required fields", "OK");
                return;
            }

            if (startTimePicker.Time >= endTimePicker.Time)
            {
                await DisplayAlert("Error", "End time must be after start time", "OK");
                return;
            }

            try
            {
                // Extract patient ID from the search entry text
                var patientId = int.Parse(patientSearchEntry.Text.Split('(')[1].Split(':')[1].TrimEnd(')'));
                var selectedPatient = allPatients.FirstOrDefault(p => p.ID == patientId);

                if (selectedPatient == null)
                {
                    await DisplayAlert("Error", "Please select a valid patient", "OK");
                    return;
                }

                var reservationData = new ReservationData
                {
                    Doctor = _loggedInDoctor.Name,
                    Patient = selectedPatient.Name,
                    Specialty = specialtyPicker.SelectedItem.ToString(),
                    Operation = operationPicker.SelectedItem.ToString(),
                    Date = datePicker.Date,
                    StartTime = $"{startTimePicker.Time.Hours:D2}:{startTimePicker.Time.Minutes:D2}",
                    EndTime = $"{endTimePicker.Time.Hours:D2}:{endTimePicker.Time.Minutes:D2}"
                };

                // Update room status in the backend
                var room = await _operationRoomServices.GetByIdAsync(int.Parse(_roomId));
                if (room != null)
                {
                    room.RoomStatus = RoomStatus.Occupied;
                    var success = await _operationRoomServices.UpdateAsync(room.RoomId, room);
                    if (success)
                    {
                        ReservationCompleted?.Invoke(this, reservationData);
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to update room status", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Room not found", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
            }
        }
    }
} 