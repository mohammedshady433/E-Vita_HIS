using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace E_Vita.ViewModels
{
    public class ChartDataPoint
    {
        public string Category { get; set; }
        public double Value { get; set; }
    }

    public partial class StatisticsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ChartDataPoint> chartData;

        [ObservableProperty]
        private string patientsSeenText;

        [ObservableProperty]
        private string avgAppointmentTimeText;

        [ObservableProperty]
        private string pendingTasksText;

        public StatisticsViewModel()
        {
            ChartData = new ObservableCollection<ChartDataPoint>
            {
                new ChartDataPoint { Category = "Patients Seen", Value = 12 },
                new ChartDataPoint { Category = "Pending Tasks", Value = 3 }
            };

            PatientsSeenText = "Patients Seen Today: 12";
            AvgAppointmentTimeText = "Avg Appointment Time: 15 mins";
            PendingTasksText = "Pending Tasks: 3";
        }
    }
}