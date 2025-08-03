using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class Prescription
    {
        [Key]
        public string Id { get; set; }
        public string Complaint { get; set; }
        public string Diagnosis { get; set; }
        public DateTime Date {  get; set; }
        public string Reason_for_visit { get; set; }
        public string Doctor_ID { get; set; } // FK property

        [ForeignKey("Doctor_ID")]
        public Doctor Doctor { get; set; } // Navigation property
        public string Patient_ID { get; set; } // FK property
        [ForeignKey("Patient_ID")]
        public Patient Patient { get; set; } // Navigation property

        // Many-to-many navigation
        public ICollection<Radiology> Radiologies { get; set; }
        // Many-to-many navigation
        public ICollection<Lab> Labs { get; set; }
    }
}
