namespace E_Vita_APIs.Models
{
    public class Bed
    {
        public int Id { get; set; }
        public int number { get; set; } // Bed number   
        public bool Active { get; set; } // Is the bed currently occupied?
    }
}
