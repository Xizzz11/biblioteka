using System.Collections.Generic;

namespace biblioteka_main.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public int PublicationYear { get; set; }
        public int AvailableCopies { get; set; }

        public virtual Author Author { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}