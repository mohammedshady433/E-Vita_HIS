using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
namespace E_Vita_APIs.Models
{
    public class Beds
    {
        [Key]
        public string Id { get; set; }
        public bool Active { get; set; } // Is the bed currently occupied?
        public BedType BedType { get; set; } // Type of bed (e.g., single, double, etc.)
        [ForeignKey("RoomId")]
        public Rooms room { get; set; }
        public string RoomId { get; set; }
    }
}
public enum BedType
{
    Single,
    Double,
    ICU, // Intensive Care Unit
    Pediatric, // For children
    Maternity, // For mothers and newborns
    Isolation, // For infectious diseases
    Emergency, // For emergency cases
    Surgical, // For post-operative care
    General // For general use
}
