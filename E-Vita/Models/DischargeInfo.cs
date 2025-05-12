namespace E_Vita.Models
{
    public class DischargeInfo
    {
        public string PatientId { get; set; }
        public DateTime DischargeDate { get; set; }
        public string Notes { get; set; }
        public string DischargeType { get; set; }
    }
} 