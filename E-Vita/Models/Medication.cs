using E_Vita.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class Medication
    {
        public int Id { get; set; }
        public float Dose { get; set; }
        public string Unit { get; set; }
        public TimeOnly Time { get; set; }
        public string Medication_name { get; set; }
        public int PractitionerID { get; set; }
        [ForeignKey("PractitionerID")]
        public Practitioner Practitioner { get; set; } // Navigation property   
        public int PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } // Navigation property
    }
}