using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// last edit: 31/7/2025
namespace E_Vita_APIs.Models
{
    public class Accountant : User
    {
        public string Rank { get; set; }
        public string Finance_ID { get; set; } // FK property

        [ForeignKey("Finance_ID")]
        public Finance finance { get; set; } // Navigation property
    }
}
