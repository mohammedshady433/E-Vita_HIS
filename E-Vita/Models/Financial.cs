using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Financial
    {
        public int Id { get; set; }
        public financialStatus Status { get; set; } // Financial status of the patient
        public float Amount { get; set; } // Amount to be paid
        public float Paid_Amount { get; set; } // for spare use 
        public PaymentMethod Method { get; set; } // Payment method used
        public int PatientId { get; set; } // FK property

        [ForeignKey("PatientId")]
        [JsonIgnore]
        public Patient Patient { get; set; } // Navigation property
        public DateTime Payment_Date { get; set; } // Date of payment

    }
    public enum financialStatus
    {
        Paid = 0,
        Unpaid = 1,
        PartiallyPaid = 2,
        Hold = 3,
    }
    public enum PaymentMethod
    {
        Cash = 0,
        Card = 1,
        Insurance = 2,
        BankTransfer = 3
    }
}
