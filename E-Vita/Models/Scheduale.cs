using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Scheduale
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public ServiceType Service_Type { get; set; }
        public MedicalSpecialty speciality { get; set; }
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        [JsonIgnore]
        public Appointment Appointment { get; set; } // Navigation property
    }
}
