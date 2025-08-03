using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Vita_APIs.Models
{
    public class PatientHistory
    {
        [Key]
        public string ID { get; set; } // Unique identifier for the patient history record
        public string Disease { get; set; } // Name of the disease or condition
        public string surgery { get; set; } // Details of any surgeries the patient has undergone

        public Patient patient { get; set; } // Navigation property to the Patient entity
        [ForeignKey("PatientId")]
        public string PatientId { get; set; } // Foreign key to the Patient table

    }
}