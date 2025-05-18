using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class WardRound
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly Time { get; set; } // Time of the ward round
        public string Note { get; set; } // Note from the ward round
        [JsonIgnore]
        public int PractitionerID { get; set; }
        [ForeignKey("PractitionerID")]
        [JsonIgnore]
        public Practitioner Practitioner { get; set; } // Navigation property   
    }
}
