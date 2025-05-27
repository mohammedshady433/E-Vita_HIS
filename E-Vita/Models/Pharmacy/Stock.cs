using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pharmacy_ASP_API.Models.Entities
{
    public class Stock
    {
        [Key]
        public string StockId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int Quantity { get; set; }

        [Required]
        public DateTime WarningDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<MedicationKnowledge> MedicationKnowledges { get; set; } = new List<MedicationKnowledge>();

        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}