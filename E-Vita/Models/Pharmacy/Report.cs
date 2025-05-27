using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pharmacy_ASP_API.Models.Entities
{
    public class Report
    {
        [Key]
        public string ReportId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Total sales cannot be negative")]
        public int TotalSales { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Order count cannot be negative")]
        public int OrderCount { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock acid price cannot be negative")]
        public int StockAcidPrice { get; set; }

        // Initialize the collection to prevent null reference
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
