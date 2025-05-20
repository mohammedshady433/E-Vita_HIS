using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public string ReasonForVisit { get; set; }

        [JsonIgnore]
        public ICollection<Medication> Medications { get; set; } = new List<Medication>();
        public string Diseases { get; set; } // List of diseases
        [JsonIgnore]
        public Lab Labtest { get; set; } // Lab test class
        public string LabTest { get; set; } // Lab test class
        public string patientcomplaint { get; set; } // Patient note
        public string RadiologyTest { get; set; } // Radiology test class
        public string Examination { get; set; } // Examination details
        public bool Reserve { get; set; } // Reserve for future use
        public string? Surgery { get; set; } // Surgery details
        public string familyHistory { get; set; } // Family history
        public int PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        [JsonIgnore]
        public Patient Patient { get; set; } // Navigation property
        public int PractitionerID { get; set; }
        [ForeignKey("PractitionerID")]
        [JsonIgnore]
        public Practitioner Practitioner { get; set; } // Navigation property   
    }
}
