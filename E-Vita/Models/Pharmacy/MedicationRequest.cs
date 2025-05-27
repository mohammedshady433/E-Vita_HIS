using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Pharmacy_ASP_API.Models.Entities
{
    public class MedicationRequest
    {
        [Key]
        public string RequestId { get; set; }

        public string? DrOutBed { get; set; }

        public string? DrInBed { get; set; }

        public string? Status { get; set; }

        public DateTime? StatusTime { get; set; }

        public string? Note { get; set; }

        [Required]
        public string? DoseInstruction { get; set; }

        [Required]
        public string? authoredTime { get; set; }

        [Required]
        [ForeignKey("MedicationKnowledge")]
        public string? MedicationId { get; set; }

        [JsonIgnore]
        public virtual Order? Order { get; set; }

        public virtual MedicationKnowledge? MedicationKnowledge { get; set; }

    }
}

