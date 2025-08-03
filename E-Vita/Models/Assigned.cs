using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class Assigned
    {
        //FK for room and UserID

        [ForeignKey("RoomID")]
        public string RoomID { get; set; }
        public Rooms Rooms { get; set; }
        //******************************************
        [ForeignKey("NurseID")]
        public string NurseID { get; set; }
        public Nurse User { get; set; }
    }
}
