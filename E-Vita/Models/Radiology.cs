using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Radiology
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public byte[]? Photo { get; set; }  // This stores the image as binary
        public string Note { get; set; }
        public int PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        [JsonIgnore]
        public Patient Patient { get; set; } // Navigation property
    }
}
