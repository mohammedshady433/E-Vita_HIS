using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class Schedule
    {
        public string Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string User_Id { get; set; } // FK property

        [ForeignKey("User_Id")]
        public User User_ID { get; set; } // Navigation property
        public string Days_ID { get; set; } // FK property

        [ForeignKey("Days_ID")]
        public Days Days { get; set; } // Navigation property
    }
}
