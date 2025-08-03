using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class WardRound
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly StartTime { get; set; } // Time of the ward round
        public TimeOnly EndTime { get; set; }
        public bool Active { get; set; }
        public string DoctorID { get; set; }
        [ForeignKey("DoctorID")]
        public Doctor Doctor { get; set; } // Navigation property
        // Many-to-many navigation
        public ICollection<Patient> Patients { get; set; }
    }
}