namespace biblioteka_main.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public int AuthorID { get; set; }
        public int PublicationYear { get; set; }
        public int AvailableCopies { get; set; }
    }
}