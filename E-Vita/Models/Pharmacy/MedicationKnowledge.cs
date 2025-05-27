using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Pharmacy_ASP_API.Models.Entities
{
    public class MedicationKnowledge
    {
        [Key]
        public string MedicationId { get; set; }

        [Required]
        public int quantity { get; set; }

        [Required]
        public required string MedicationName { get; set; }

        [Required]
        public required string ClinicalUse { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Cost cannot be negative")]
        public decimal Cost { get; set; }

        [Required]
        public required string ProductType { get; set; }

        [Required]
        public required string Status { get; set; }

        [Required]
        public DateTime Expirydate { get; set; }

        [Required]
        [ForeignKey("Stock")]
        public string StockId { get; set; }

        [JsonIgnore]
        public virtual Stock? Stock { get; set; }

        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
