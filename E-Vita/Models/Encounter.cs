using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class Encounter
    {
        public int Id { get; set; }
        public DateTime time { get; set; }
        public OUTIN_Patient type { get; set; } // OUTIN_Patient enum
        public string Reason { get; set; }
        public int PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } // Navigation property
        public int PractitionerID { get; set; } 
        [ForeignKey("PractitionerID")]
        public Practitioner Practitioner { get; set; } // Navigation property   

    }

}
