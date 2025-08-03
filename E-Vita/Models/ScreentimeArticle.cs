using System;
using System.ComponentModel.DataAnnotations;

namespace E_Vita_APIs.Models
{
    public class ScreentimeArticle
    {
        [Key]
        public string Chat_ID { get; set; }
        public string Articles { get; set; }
        public TimeOnly Time { get; set; }
    }
}
