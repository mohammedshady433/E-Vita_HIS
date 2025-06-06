﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Vita_APIs.Models
{
    public class Results
    {
        public int ResultId { get; set; }

        public string Range { get; set; }
        public DateTime Date { get; set; }
        public string TestComponent { get; set; }

        public int LabId { get; set; } // Foreign key to Lab
        public int PatientId { get; set; } // Foreign key to Patient

        [ForeignKey("LabId")]
        [JsonIgnore]
        public Lab Lab { get; set; }  // Navigation property

        [ForeignKey("PatientId")]
        [JsonIgnore]
        public Patient Patient { get; set; }  // Navigation property
    }
}
