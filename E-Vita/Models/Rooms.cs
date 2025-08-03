using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Rooms
    {
        public string ID { get; set; }
        public RoomStatus availablity { get; set; }
        public RoomType Type { get; set; }
        public int Capacity { get; set; }
    }
    public enum RoomType
    {
        General = 0,
        Private = 1,
        SemiPrivate = 2,
        ICU = 3,
        Maternity = 4,
        Pediatric = 5,
        Surgical = 6,
        Emergency = 7,
        Isolation = 8,
        Recovery = 9
    }
    public enum RoomStatus
    {
        Available = 0,
        Occupied = 1,
        UnderMaintenance = 2
    }
}