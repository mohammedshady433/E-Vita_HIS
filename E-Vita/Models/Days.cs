using System.ComponentModel.DataAnnotations;

namespace E_Vita_APIs.Models
{
    public class Days
    {
        [Key]
        public string Days_ID { get; set; }
        public string DayName { get; set; }

        // Navigation property for the relationship with Schedule
        public ICollection<Schedule> Schedules { get; set; }
     
    }
}
