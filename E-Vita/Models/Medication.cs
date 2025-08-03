using E_Vita_APIs.Repositories;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace E_Vita_APIs.Models
{
    public class Medication
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Active_site { get; set; }
        public DateTime Date { get; set; }
        public string PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } // Navigation property

        public string PrescriptionID { get; set; } // FK property

        [ForeignKey("PrescriptionID")]
        public Prescription Prescription { get; set; } // Navigation property
    }
}
