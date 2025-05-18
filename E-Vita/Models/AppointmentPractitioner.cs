using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class AppointmentPractitioner
    {
        public int AppointmentId { get; set; }
        [JsonIgnore]
        public Appointment Appointment { get; set; }

        public int PractitionersId { get; set; }
        [JsonIgnore]
        public Practitioner Practitioner { get; set; }
    }
}
