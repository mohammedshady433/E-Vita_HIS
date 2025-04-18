
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace E_Vita;

/// <summary>
/// Interaction logic for Page1.xaml
/// </summary>
public partial class NurseDashboard : ContentPage
{
    private DateTime currentDate;

    public NurseDashboard()
    {
        InitializeComponent();
        currentDate = DateTime.Now;
        PopulateYearAndMonthSelectors();
        GenerateCalendar(currentDate);
        //var services = ((App)Application.Current)._serviceProvider;
        //_Appointment = services.GetService<IRepository<Appointment>>() ?? throw new InvalidOperationException("Data helper service is not available");
        //LoadAppointmentsFortoday();
    }
    //public async void LoadAppointmentsFortoday()
    //{
    //    var today = DateTime.Today; // Get today's date without time
    //    var todayAppointments = await _Appointment.GetAllAsync();

    //    //Filter appointments where the Date matches today's date
    //    var filteredAppointments = todayAppointments
    //        .Where(a => a.Date.Date == today)
    //        .ToList();

    //    ScheduleDataGrid.ItemsSource = todayAppointments;
    //}
    //public async void LoadAppointmentsForDate(DateTime specificDate)
    //{
    //    var specificDateAppointments = await _Appointment.GetAllAsync();

    //    // Filter appointments where the Date matches the specific date
    //    var filteredAppointments = specificDateAppointments
    //        .Where(a => a.Date.Date == specificDate.Date)
    //        .ToList();

    //    ScheduleDataGrid.ItemsSource = filteredAppointments;
    //}

    private int currentYear = DateTime.Now.Year;
    private int currentMonth = DateTime.Now.Month;

    //private void PopulateYearAndMonthSelectors()
    //{

    //    CalendarGrid.Children.Clear();
    //    CalendarGrid.ColumnDefinitions.Clear();
    //    CalendarGrid.RowDefinitions.Clear();

    //    // Define columns (7 for days of the week)
    //    for (int i = 0; i < 7; i++)
    //    {
    //        CalendarGrid.ColumnDefinitions.Add(new ColumnDefinition());
    //    }

    //    // Define rows (5 or 6 for weeks)
    //    for (int i = 0; i < 6; i++)
    //    {
    //        CalendarGrid.RowDefinitions.Add(new RowDefinition());
    //    }

    //    DateTime firstDayOfMonth = new DateTime(currentYear, currentMonth, 1);
    //    int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
    //    int startDay = (int)firstDayOfMonth.DayOfWeek; // Sunday = 0, Monday = 1, etc.

    //    int row = 1, col = startDay;

    //    for (int day = 1; day <= daysInMonth; day++)
    //    {
    //        var dayButton = new Button
    //        {
    //            Text = day.ToString(),
    //            BackgroundColor = Color.FromArgb("#e1b184"),
    //            TextColor = Color.FromArgb("#3b0054"),
    //            CornerRadius = 10,
    //            Padding = 5
    //        };

    //        // Assign click event
    //        dayButton.Clicked += (sender, e) =>
    //        {
    //            DisplayAlert("Date Selected", $"You selected {new DateTime(currentYear, currentMonth, day):dddd, MMMM d, yyyy}", "OK");
    //        };

    //        // Add to grid properly in .NET MAUI
    //        CalendarGrid.Children.Add(dayButton);
    //        Grid.SetRow(dayButton, row);
    //        Grid.SetColumn(dayButton, col);

    //        col++;
    //        if (col > 6) // Move to next row after Saturday
    //        {
    //            col = 0;
    //            row++;
    //        }
    //    }
    //}

    private void PopulateYearAndMonthSelectors()
    {
        // Populate Years
        int currentYear = DateTime.Now.Year;
        for (int year = currentYear - 10; year <= currentYear + 10; year++)
        {
            YearSelector.Items.Add(year.ToString());
        }
        YearSelector.SelectedIndex = 10; // Default to current year

        // Populate Months
        for (int month = 1; month <= 12; month++)
        {
            MonthSelector.Items.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month));
        }
        MonthSelector.SelectedIndex = DateTime.Now.Month - 1;
    }
    private void GenerateCalendar(DateTime date)
    {
        CalendarGrid.Children.Clear();

        // Add day names (Sunday-Saturday)
        string[] dayNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
        foreach (string dayName in dayNames)
        {
            CalendarGrid.Clear();

            // Get the first day of the month
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            int startDayOffset = (int)firstDayOfMonth.DayOfWeek;

            // Add day headers
            string[] abbreviatedDayNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            foreach (var abbreviatedDayName in abbreviatedDayNames)
            {
                CalendarGrid.Add(new Label { Text = abbreviatedDayName, FontAttributes = FontAttributes.Bold });
            }

            // Add empty spaces before first day
            for (int i = 0; i < startDayOffset; i++)
            {
                CalendarGrid.Add(new Label { Text = "" });
            }

            //Add days
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDay = new DateTime(date.Year, date.Month, day);
                Button dayButton = new Button
                {
                    Text = day.ToString(),
                    BackgroundColor = currentDay == DateTime.Today ? Colors.DarkRed : Colors.White,
                    CommandParameter = currentDay
                };

                dayButton.Clicked += DayButton_Click;
                CalendarGrid.Add(dayButton);
            }

        }
    }
    private void DayButton_Click(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is DateTime selectedDate)
        {
            DisplayAlert("Date Selected", $"You selected {selectedDate:dddd, MMMM d, yyyy}", "OK");
        }
    }


    private void PreviousMonth_Click(object sender, EventArgs e)
    {
        currentDate = currentDate.AddMonths(-1);
        YearSelector.SelectedItem = currentDate.Year.ToString();
        MonthSelector.SelectedIndex = currentDate.Month - 1;
        GenerateCalendar(currentDate);
    }

    private void NextMonth_Click(object sender, EventArgs e)
    {
        currentDate = currentDate.AddMonths(1);
        YearSelector.SelectedItem = currentDate.Year;
        MonthSelector.SelectedIndex = currentDate.Month - 1;
        GenerateCalendar(currentDate);
    }

    //private void YearSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (YearSelector.SelectedItem != null && MonthSelector.SelectedIndex >= 0)
    //    {
    //        currentDate = new DateTime((int)YearSelector.SelectedItem, MonthSelector.SelectedIndex + 1, 1);
    //        GenerateCalendar(currentDate);
    //    }
    //}

    private void YearSelector_SelectionChanged(object sender, EventArgs e)
    {
        // Handle the selection change event here
        var picker = sender as Picker;
        if (picker != null)
        {
            string selectedYear = picker.SelectedItem?.ToString();
            DisplayAlert("Year Selected", $"You selected: {selectedYear}", "OK");
        }
    }

    //private void MonthSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (YearSelector.SelectedItem != null && MonthSelector.SelectedIndex >= 0)
    //    {
    //        currentDate = new DateTime((int)YearSelector.SelectedItem, MonthSelector.SelectedIndex + 1, 1);
    //        GenerateCalendar(currentDate);
    //    }
    //}
    private void MonthSelector_SelectionChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null)
        {
            string selectedMonth = picker.SelectedItem?.ToString();
            DisplayAlert("Month Selected", $"You selected: {selectedMonth}", "OK");
        }
    }
    //private void DayButton_Click(object sender, RoutedEventArgs e)
    //{
    //    if (sender is Button button && button.Tag is DateTime selectedDate)
    //    {
    //        //MessageBox.Show($"Clicked on {selectedDate:MMMM dd, yyyy}");
    //        LoadAppointmentsForDate(selectedDate);

    //    }
    //}

    //private void AddPatient_Click(object sender, RoutedEventArgs e)
    //{
    //    this.NavigationService.Navigate(new Add_Patient());
    //}

    //private void BookAppointment_Click(object sender, RoutedEventArgs e)
    //{
    //    BookAppointmentWindow bookAppointmentWindow = new BookAppointmentWindow();
    //    bookAppointmentWindow.ShowDialog();
    //}



    private async void ScheduleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await DisplayAlert("Selection", "Schedule item selected.", "OK");
    }


    private void BookAppointment_Click(object sender, EventArgs e)
    {
        // Example action when the button is clicked
        DisplayAlert("Appointment", "Booking appointment...", "OK");
    }
    private void AddPatient_Click(object sender, EventArgs e)
    {
        // Example action when the button is clicked
        DisplayAlert("Add Patient", "Adding new patient...", "OK");
    }

    private void LogOut_Click(object sender, EventArgs e)
    {
        // Example: Navigate back to the login page
        Application.Current.MainPage = new MainPage(); // Replace with your actual login page
    }

    //private void LogOut_Click(object sender, EventArgs e)
    //{
    //    // Navigate back to login page
    //    var mainWindow = (MainWindow)Window.GetWindow(this);
    //    if (mainWindow != null)
    //    {
    //        // Clear any stored credentials or session data
    //        mainWindow.pass_txt.Clear();
    //        mainWindow.user_txt.Clear();
    //        mainWindow.ID_txt.Clear();

    //        // Clear the navigation history and return to the main content
    //        while (mainWindow.MainFrame.CanGoBack)
    //        {
    //            mainWindow.MainFrame.RemoveBackEntry();
    //        }
    //        mainWindow.MainFrame.Content = null;
    //    }
    //}
}
