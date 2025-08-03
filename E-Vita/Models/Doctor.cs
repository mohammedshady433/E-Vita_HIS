using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//// last edit: 31/7/2025
namespace E_Vita_APIs.Models
{
    public class Doctor : User
    {
        public string Rank { get; set; }
        public string Speciality { get; set; }
        public string? Screentime { get; set; }

        [ForeignKey("Screentime")]
        public ScreentimeArticle screentime { get; set; }
    }
} 