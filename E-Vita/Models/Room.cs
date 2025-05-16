using System.ComponentModel.DataAnnotations.Schema;
using E_Vita_APIs.Models;


namespace E_Vita_APIs.Models
{
    public class Room
    {
        public int ID { get; set; }
        public RoomStatus availablity { get; set; }
        public int Floor { get; set; }
        public string Name { get; set; }
        public int PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } // Navigation property

        public int BedId { get; set; } // FK property
        [ForeignKey("BedId")]
        public Bed Bed { get; set; }

    }
    public enum RoomType
    {
        General = 0,
        Private = 1,
        SemiPrivate = 2
    }
    public enum RoomStatus
    {
        Available = 0,
        Occupied = 1,
        UnderMaintenance = 2
    }
}