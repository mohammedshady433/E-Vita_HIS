using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Patient
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Nationality { get; set; }

        public string Address { get; set; }
        public OUTIN_Patient Status { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        [JsonIgnore]
        public ICollection<Practitioner> Practitioners { get; set; } = new List<Practitioner>();
        public int? WardRoundId { get; set; }
        [ForeignKey("WardRoundId")]
        [JsonIgnore]
        public WardRound? WardRound { get; set; } // Navigation property   
        [JsonIgnore]
        public ICollection<Results>? Results { get; set; }


    }
    public enum OUTIN_Patient
    {
        Out_Patient=0,
        In_Patient = 1
    }
    public enum Gender
    {
        Male= 0,
        Female = 1
    }
}
