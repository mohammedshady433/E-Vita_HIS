using System.ComponentModel.DataAnnotations;

namespace E_Vita_APIs.Models
{
    public class Finance
    {
        [Key]
        public string Finance_ID { get; set; }
        public decimal Salaries { get; set; }
        public decimal Appointment_Income { get; set; }
        public decimal RoomReservation_Income { get; set; }
        public decimal Appointment_Outcome { get; set; }
        public decimal RoomReservation_Outcome { get; set; }

        // One Finance to Many Patients (pay relationship)
        public ICollection<Patient> Patients { get; set; }
        // One Finance to Many Accountants 
        public ICollection<Accountant> Accountants { get; set; }
        
    }
}
