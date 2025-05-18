using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class SharedNote
    {
        public int Id { get; set; }
        public string content { get; set; } // Content of the note
        public int PractitionerID { get; set; }
        [ForeignKey("PractitionerID")]
        [JsonIgnore]
        public Practitioner Practitioner { get; set; } // Navigation property   
    }
}
