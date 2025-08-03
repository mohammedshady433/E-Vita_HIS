using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//last edit: 31/7/2025
namespace E_Vita_APIs.Models
{
    public class User
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public DateTime DOB { get; set; }
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string? Nationalality { get; set; }
        public string? degree { get; set; }
        public int? years_of_experience { get; set; }
        public float Salary { get; set; }
    }
} 