using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class DoctorNursePatient
    {
        public string ID { get; set; }
        [ForeignKey("DoctorID")]
        public string DoctorID { get; set; }
        [ForeignKey("NurseID")]
        public string NurseID { get; set; }
        [ForeignKey("PatientID")]
        public string PatientID { get; set; }
        // Navigation properties
        public Doctor Doctor { get; set; }
        public Nurse Nurse { get; set; }
        public Patient Patient { get; set; }
        // Additional properties can be added as needed
    }
}
