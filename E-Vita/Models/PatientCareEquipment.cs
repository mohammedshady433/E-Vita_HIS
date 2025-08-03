using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class PatientCareEquipment
    {
        public string Id { get; set; } // Unique identifier for the equipment
        public string DeviceName { get; set; } // Name of the equipment
        public DateTime Date_of_Adding { get; set; } // Date when the equipment was added
        public DateTime? ExpireDate { get; set; } // Expiration date of the equipment

        public string BedId { get; set; } // Foreign key to the Bed table
        [ForeignKey("BedId")]
        public Beds Bed { get; set; } // Navigation property to the Bed entity

    }
}
