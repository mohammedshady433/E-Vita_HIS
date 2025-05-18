using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Operation_Room
    {
        public RoomStatus RoomStatus { get; set; }
        public string Operation { get; set; }
        public string Equipment { get; set; }
        [JsonIgnore]
        public ICollection<Practitioner> Practitioners { get; set; } = new List<Practitioner>();
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        [JsonIgnore]
        public Room Room { get; set; } // Navigation property   

    }
}
