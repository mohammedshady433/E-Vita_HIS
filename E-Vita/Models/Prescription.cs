using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public string ReasonForVisit { get; set; }
        public Medication Medication { get; set; } // Medication class
        public string Diseases { get; set; } // List of diseases
        public Lab Labtest { get; set; } // Lab test class
        public string RadiologyTest { get; set; } // Radiology test class
        public string Examination { get; set; } // Examination details
        public bool Reserve { get; set; } // Reserve for future use
        public bool sergery { get; set; } // Surgery details
        public int PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } // Navigation property
        public int PractitionerID { get; set; }
        [ForeignKey("PractitionerID")]
        public Practitioner Practitioner { get; set; } // Navigation property   
    }
}
