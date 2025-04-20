using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace E_Vita
{
    public class Notification
    {
        public DateTime Timestamp { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool IsEmergency { get; set; }
    }

    public partial class NotificationsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Notification> notifications;

        public NotificationsViewModel()
        {
            Notifications = new ObservableCollection<Notification>
            {
                new Notification
                {
                    Timestamp = DateTime.Now.AddMinutes(-10),
                    Type = "Lab Result",
                    Description = "Blood test results ready for Patient P1001",
                    IsEmergency = false
                },
                new Notification
                {
                    Timestamp = DateTime.Now.AddMinutes(-5),
                    Type = "Emergency",
                    Description = "Patient P1002 reporting chest pain",
                    IsEmergency = true
                },
                new Notification
                {
                    Timestamp = DateTime.Now.AddMinutes(-2),
                    Type = "Lab Result",
                    Description = "X-Ray results available for Patient P1003",
                    IsEmergency = false
                }
            };
        }
    }

    public class EmergencyColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Colors.Red : Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}