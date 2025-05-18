using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Contact_fam
    {
        public string Relationship { get; set; } // e.g. Mother, Father, Spouse, etc.
        public string Name { get; set; } // Name of the contact
        public string Phone { get; set; } // Phone number of the contact
        public string Address { get; set; } // Address of the contact
        public Gender Gender { get; set; }
        public int PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        [JsonIgnore]
        public Patient Patient { get; set; } // Navigation property
    }
}
