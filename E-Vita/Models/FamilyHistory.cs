using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Vita_APIs.Models
{
    public class FamilyHistory
    {
        [Key]
        public string Fam_ID { get; set; }
        public string Explanation { get; set; }
        public string Relation { get; set; }
        [ForeignKey("PatientHistoryID")]
        public string PatientHistoryID { get; set; }
        public PatientHistory PatientHistory { get; set; }
    }
}
