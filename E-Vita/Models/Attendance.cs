using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Vita_APIs.Models
{
    public class Attendance
    {
            public string ID { get; set; }
            public TimeOnly Attend_Time { get; set; }
            public TimeOnly Leave_Time { get; set; }
            public DateTime Date { get; set; }

            // Foreign key to users
            [ForeignKey("UserID")]
            public string UserID { get; set; }

            public User User { get; set; }
    }
}
