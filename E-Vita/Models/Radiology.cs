using System.ComponentModel.DataAnnotations;

namespace E_Vita_APIs.Models
{
    public class Radiology
    {
        [Key]
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public StatusOfRadiology Status { get; set; }
        public string? Examination_Type { get; set; }
        // Many-to-many navigation
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
public enum StatusOfRadiology
{
    Pending = 0,
    Completed = 1,
    Cancelled = 2
}