namespace E_Vita_APIs.Models
{
    public class Lab
    {
        public int Id { get; set; }
        public byte[]? Photo { get; set; }  // This stores the image as binary
        public string Note { get; set; }
        public string LabType { get; set; } // e.g. Blood Test, X-Ray, etc.
        public ICollection<Results> Results { get; set; }

    }
}
