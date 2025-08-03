using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class NoteDoctorNurseReceptionist
    {
        public string Id { get; set; }

        // Foreign key properties

        public string NoteId { get; set; }
        public string DoctorId { get; set; }
        public string NurseId { get; set; }
        public string ReceptionistId { get; set; }

        // Navigation properties
        public SharedNote SharedNote { get; set; }
        public Doctor Doctor { get; set; }
        public Nurse Nurse { get; set; }
        public Receptionist Receptionist { get; set; }
    }
}