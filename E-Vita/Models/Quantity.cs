using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class Quantity
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int Observation_VitalsId { get; set; } // FK property

        [ForeignKey("Observation_VitalsId")]
        public Observation_Vital Observation_Vital { get; set; } // Navigation property
    }
}
