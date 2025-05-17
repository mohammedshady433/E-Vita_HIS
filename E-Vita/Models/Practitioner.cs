using System.Numerics;

namespace E_Vita_APIs.Models
{
    public class Practitioner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PasswordHash { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Appointment> Appointment { get; set; } = new List<Appointment>();
        public ICollection<Operation_Room> operation_Rooms { get; set; } = new List<Operation_Room>();
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();

    }
}
