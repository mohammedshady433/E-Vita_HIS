using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Cancelation_Reason { get; set; }
        public DateTime? Cancelation_Date { get; set; }
        public ServiceType Service_Type { get; set; }
        public TimeOnly Duration { get; set; }
        public Service Actor { get; set; }
        public int PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        [JsonIgnore]
        public Patient? Patient { get; set; } // Navigation property
        [JsonIgnore]
        public ICollection<AppointmentPractitioner> AppointmentPractitioners { get; set; }


    }
    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled,
        NoShow
    }
    public enum ServiceType
    {
        Surgery,
        Emergency_Operation,
        clincal
    }
}
