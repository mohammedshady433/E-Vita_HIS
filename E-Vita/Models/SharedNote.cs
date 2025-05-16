using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class SharedNote
    {
        public int Id { get; set; }
        public string content { get; set; } // Content of the note
        public int PractitionerID { get; set; }
        [ForeignKey("PractitionerID")]
        public Practitioner Practitioner { get; set; } // Navigation property   
    }
}
