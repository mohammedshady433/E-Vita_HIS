namespace E_Vita_APIs.Models
{
    public class Observation_Vital
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public string Note { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }

    }
}
