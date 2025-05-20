using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Medication
    {
        public int Id { get; set; }
        public string MedID { get; set; }
        public string ActiveIngrediant { get; set; }
        public string Dose { get; set; }
        public TimeOnly Time { get; set; }
        public string Medication_name { get; set; }
        public int PractitionerID { get; set; }
        [ForeignKey("PractitionerID")]
        [JsonIgnore]
        public Practitioner Practitioner { get; set; } // Navigation property   
        public int PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        [JsonIgnore]
        public Patient Patient { get; set; } // Navigation property
                                             // Foreign key to Prescription
        public int? PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        [JsonIgnore]
        public Prescription Prescription { get; set; }
    }
}
