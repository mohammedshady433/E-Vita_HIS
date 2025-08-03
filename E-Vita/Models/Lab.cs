using System.ComponentModel.DataAnnotations;

namespace E_Vita_APIs.Models
{
    public class Lab
    {
        [Key]
        public string ID { get; set; }
        public DateTime Date { get; set; }
        public StatusOfLab Status { get; set; }
        public string? Type { get; set; }
        // Many-to-many navigation
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
public enum StatusOfLab
{
    Pending = 0,
    Completed = 1,
    Cancelled = 2
}