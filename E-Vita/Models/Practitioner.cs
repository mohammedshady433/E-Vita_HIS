using System.Numerics;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Practitioner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Rank { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public ICollection<Operation_Room> operation_Rooms { get; set; } = new List<Operation_Room>();
        [JsonIgnore]
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
        [JsonIgnore]
        public ICollection<AppointmentPractitioner> AppointmentPractitioners { get; set; } = new List<AppointmentPractitioner>();


    }
}
