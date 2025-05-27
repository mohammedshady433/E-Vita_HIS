using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Pharmacy_ASP_API.Models.Entities
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Patient
    {
        [Key]
        public string PatientId { get; set; }
        public required string PatientName { get; set; }
        public required string PhoneNo { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        [JsonIgnore]
        public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();
                                                           //Many Finance records(they have multiple bills/financial reports)
    }

}
