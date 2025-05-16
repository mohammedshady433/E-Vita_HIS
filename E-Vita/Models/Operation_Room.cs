using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class Operation_Room
    {
        public RoomStatus RoomStatus { get; set; }
        public string Operation { get; set; }
        public string Equipment { get; set; }
        public ICollection<Practitioner> Practitioners { get; set; } = new List<Practitioner>();
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; } // Navigation property   

    }
}
