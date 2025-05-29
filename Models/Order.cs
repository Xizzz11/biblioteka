using System;

namespace biblioteka_main.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int BookID { get; set; }
        public int ReaderID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}