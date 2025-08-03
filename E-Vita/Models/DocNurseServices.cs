using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class DocNurseServices
    {
        public string ID { get; set; } // Primary key

        public string DoctorID { get; set; } // FK to Doctor
        public string NurseID { get; set; }  // FK to Nurse
        public string ServiceID { get; set; } // FK to Service

        // Navigation properties
        public Doctor Doctor { get; set; }
        public Nurse Nurse { get; set; }
        public Service Service { get; set; }
    }
}
