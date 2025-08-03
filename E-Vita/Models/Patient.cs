using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
// last edit: 31/7/2025
namespace E_Vita_APIs.Models
{
    public class Patient
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Nationality { get; set; }
        public string Address { get; set; }
        public OUTIN_Patient Status { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string password { get; set; }

        // FK to the financial table
        public string Finance_ID { get; set; } // FK property

        [ForeignKey("Finance_ID")]
        public Finance finance { get; set; } // Navigation property
        // Many-to-many navigation
        public ICollection<WardRound> wardRounds { get; set; }
    }
    public enum OUTIN_Patient
    {
        Out_Patient=0,
        In_Patient = 1,
        Dismissed = 2   
    }
    public enum Gender
    {
        Male= 0,
        Female = 1
    }
}
