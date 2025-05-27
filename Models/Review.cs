namespace biblioteka_main.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int BookID { get; set; }
        public int ReaderID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}