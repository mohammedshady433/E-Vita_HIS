using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class Share
    {
        public string ID { get; set; } // Primary key
        // Doctor
        public string DoctorId { get; set; } // FK property

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } // Navigation property

        // Note
        public string NoteId { get; set; } // FK property

        [ForeignKey("NoteId")]
        public SharedNote Note { get; set; } // Navigation property

        // Nurse
        public string NurseId { get; set; } // FK property

        [ForeignKey("NurseId")]
        public Nurse Nurse { get; set; } // Navigation property

        // Receptionist
        public string ReceptionistId { get; set; } // FK property

        [ForeignKey("ReceptionistId")]
        public Receptionist Receptionist { get; set; } // Navigation property
    }
}
