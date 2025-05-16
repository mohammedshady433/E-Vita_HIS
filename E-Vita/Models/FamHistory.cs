using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class FamHistory
    {
        public int ID { get; set; }
        public string Disease { get; set; }
        public DateTime Date { get; set; }
        public int PractitionerID { get; set; }
        [ForeignKey("PractitionerID")]
        public Practitioner Practitioner { get; set; } // Navigation property   
        public int PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } // Navigation property
    }
}
